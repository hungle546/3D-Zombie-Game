using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    private AudioSource musicData;
    
    void Start()
    {
        musicData = GetComponent<AudioSource>();
        musicData.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
