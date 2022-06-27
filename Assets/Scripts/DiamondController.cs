using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondController : MonoBehaviour
{
    private GameController pScript;
    
    // Start is called before the first frame update
    void Start()
    {
        GameObject gameObj = GameObject.FindWithTag("GameController");
        pScript = gameObj.GetComponent<GameController>();
    }
    void OnTriggerEnter(Collider collect)
    {
        if (collect.CompareTag("Player"))
        {
            GameObject diamond = collect.gameObject;
            Debug.Log("You got a item!");
            pScript.CollectedItem();
            GetComponent<BoxCollider>().enabled = false;
            Destroy(gameObject);
        }
    }

}
