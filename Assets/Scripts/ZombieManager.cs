using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ZombieManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private TextMeshProUGUI waveNum;
    private GameObject[] respawns;
    private int wave;
    private int enemyMultipler;
    private int totalEnemies;
    private int maxEnemies;
    private bool inProgress = true;
    private string prefix = "Wave: ";
    private Vector3[] spawnPoints =
    {
        new Vector3(137,0,44), new Vector3(118,0,21), new Vector3(102,0,-5),
        new Vector3(140,0,-44), new Vector3(96,0,-68), new Vector3(99,0,40)
    };

    // Start is called before the first frame update
    void Start()
    {
        maxEnemies = 3;
        waveNum.SetText(prefix + wave);
    }
    
    // Update is called once per frame
    void Update()
    {
        WaveChecker();
    }
    
    private void CreateEnemyWaves()
    {
        for (int i = 0; i < maxEnemies; i++)
        {
            SetUpEnemy(RandomEnemyPosition());
        }
    }
    private Vector3 RandomEnemyPosition()
    {
        Vector3 enemyPos = spawnPoints[Random.Range(0, 6)];
        //Vector3 enemyPos = transform.position;
        //enemyPos.x += Random.Range(-100, 0);
        //enemyPos.z += Random.Range(100, 0);
        return enemyPos; 
    }
    private void SetUpEnemy(Vector3 enemyPos)
    {
        Instantiate(enemyPrefab, enemyPos, Quaternion.identity);
    }

    private void WaveChecker()
    {
        if (GameObject.FindGameObjectsWithTag("Zombie").Length == 0 && inProgress)
        {
            inProgress = false;
            wave++;
            maxEnemies = wave * 3;
            waveNum.SetText(prefix + wave);
            StartCoroutine("WaveDelay");
        }
    }

    private IEnumerator WaveDelay()
    {
        yield return new WaitForSeconds(7f);
        CreateEnemyWaves();
        inProgress = true;
    }
}
