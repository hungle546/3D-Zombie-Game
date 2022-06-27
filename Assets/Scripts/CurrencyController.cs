using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyController : MonoBehaviour
{
    private float totalPoints = 0;
    private float currentPoints;
    private float multiplier = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        currentPoints = totalPoints;
        //currentPoints = 9999;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float getPoints()
    {
        return currentPoints;
    }

    public void increasePoints(float points)
    {
        currentPoints += points*multiplier;
        //Debug.Log("Points:" + currentPoints);
    }

    public void decreasePoints(float points)
    {
        currentPoints -= points;
    }
    
    public void DoublePoints()
    {
        multiplier = 2;
        StartCoroutine("DoublePointsDelay");
    }

    private IEnumerator DoublePointsDelay()
    {
        yield return new WaitForSeconds(20f);
        multiplier = 1;
    }
}
