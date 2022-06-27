using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class ZombieController : MonoBehaviour
{
    [SerializeField] private Transform player;
    
    private NavMeshAgent agent;
    private Animator ac;
    private float maxAttackDistance;
    private bool inRange;
    private GameObject playObj;
    private float origSpeed;
    private float health = 100f;
    private PlayerController playerC;
    private bool AttackMode = false;
    private bool isDead = false;
    [SerializeField] private GameObject ammoItem;
    [SerializeField] private GameObject instantKillItem;
    [SerializeField] private GameObject doublePointsItem;
    [SerializeField] private ParticleSystem blood;
    private ParticleSystem bloodClip;
    private CurrencyController currency;
    private AudioSource zombieSnarl;
    public AudioClip zombieNoiseClip;
    private AudioSource zombieDie;
    public AudioClip zombieDeathClip;
    private Vector3 deathPosition; 

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        ac = GetComponent<Animator>();
        maxAttackDistance = 2.0f;
        inRange = false;
        origSpeed = agent.speed;
        
        playObj = GameObject.FindGameObjectWithTag("Player");
        player = playObj.transform;
        playerC = playObj.GetComponent<PlayerController>();

        GameObject currencyObj = GameObject.FindGameObjectWithTag(("Currency"));
        currency = currencyObj.GetComponent<CurrencyController>();
        bloodClip = transform.GetChild(0).GetComponent<ParticleSystem>();

        if (!isDead)
        {
            zombieSnarl = GetComponent<AudioSource>();
            zombieSnarl.clip = zombieNoiseClip;
            zombieSnarl.loop = true;
            zombieSnarl.Play();
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 distToPlayer = player.position - transform.position;
        if (!isDead)
        {
            InRangeCheck(distToPlayer);
        }
        if (AttackMode && isAttacking())
        {
            StartCoroutine("Damaged");
        }
    }
    private void MoveToPlayer()
    {
        agent.speed = origSpeed;
        agent.SetDestination(player.position);
        ac.SetBool("isIdle", false);
        ac.SetBool("isRun", true);
    }

    private void InRangeCheck(Vector3 distance) // checks if the player is in the zombie's attack range
    {
        if (distance.magnitude < maxAttackDistance) {
            ac.SetBool("isRun", false);
            ac.SetBool("isAttack", true);
            inRange = true;
            agent.speed = 0;
        }
        else
        {
            ac.SetBool("isAttack", false);
            inRange = false;
            MoveToPlayer();
        }
    }

    public void Stop()
    {
        agent.speed = 0;
    }

    private void IncreaseSpeed()
    {
        origSpeed = agent.speed * 2; //double the current agent’s speed
        ac.SetBool("isRun", true);//change the animation to run
    }

    public bool isAttacking()
    {
        return ac.GetBool("isAttack");
    }
    public void SetPlayer(Transform pos)
    {
        player = pos; 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && isAttacking())
        {
            playerC.takeDamage(20);
            StartCoroutine("Delay");
        }
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(2f);
        AttackMode = true;
    } 

    private IEnumerator Damaged()
    {
        AttackMode = false;
        yield return new WaitForSeconds(2f);
        playerC.takeDamage(20);
        AttackMode = true;
    }

    public void TakeDamage(float dam)
    {
        if (!isDead)
        {
            //blood.Play();
            bloodClip.Play();
            health -= dam;
            currency.increasePoints(10);
            if (health <= 0)
            {
                isDead = true;
                StartCoroutine("StartDeath");
                zombieDie = GetComponent<AudioSource>();
                zombieDie.clip = zombieDeathClip;
                zombieDie.PlayOneShot(zombieDeathClip);
            }
        }
    }
    
    private IEnumerator StartDeath()
    {
        Stop();
        ac.SetBool("isAttack", false);
        ac.SetBool("isRun", false);
        ac.SetBool("isDeath", true);
        currency.increasePoints(100);
        yield return new WaitForSeconds(2.5f);
        deathPosition = transform.position;
        deathPosition.y = 1f; 
        Destroy(gameObject);
        SpawnRandomItem();
    }

    private void SpawnRandomItem()
    {
        if (Random.Range(1, 8) == 1)
        {
            var num = Random.Range(0, 3);
            Debug.Log(num);
            switch (num)
            {
                case 0:
                    Instantiate(ammoItem, deathPosition, Quaternion.identity);
                    break;
                case 1:
                    Instantiate(instantKillItem, deathPosition, Quaternion.identity);
                    break;
                case 2:
                    Instantiate(doublePointsItem, deathPosition, Quaternion.identity);
                    break;
            }
        }
    }
}
