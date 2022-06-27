using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject player; 
    public static PlayerManager instance; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 getLocation()
    {
        return this.transform.position;
    }

    private void Awake()
    {
        instance = this; 
    }
}
