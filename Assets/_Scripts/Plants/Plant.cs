using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{


    public int daysToGrow = 3;
    public int currentDay = 0;
    public List<GameObject> phases = new List<GameObject>();
    public GameObject currentPhase;

    void OnEnable()
    {
        currentPhase = Instantiate(phases[currentDay], transform);
        ++currentDay;
    }

    void Update()
    {

    }

    public virtual void Grow()
    {
        if (currentDay != daysToGrow)
        {
            // Remove seeds
            if (currentDay == 1)
            {
                Destroy(currentPhase);
            }
            currentPhase = Instantiate(phases[currentDay], transform);
            ++currentDay;
        }
    }
}
