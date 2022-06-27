using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUps : MonoBehaviour
{
    private string type;
    private CurrencyController currency;
    private GameObject gameObj;
    private GameController game;
    // Start is called before the first frame update
    void Start()
    {
        type = transform.tag;
        GameObject currencyObj = GameObject.FindWithTag("Currency");
        currency = currencyObj.GetComponent<CurrencyController>();
        gameObj = GameObject.FindWithTag("GameController");
        game = gameObj.GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (type == "MaxAmmo")
            {
                game.MaxAmmo();
                Debug.Log("Max Ammo");
            }
            else if (type == "InstantKill")
            {
                game.InstantKill();
                Debug.Log("Instant Kill");
            }
            else if (type == "DoublePoints")
            {
                currency.DoublePoints();
                Debug.Log("DoublePoints");
            }
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<SphereCollider>().enabled = false;
            Destroy(gameObject);
        }
        
        
    }
}
