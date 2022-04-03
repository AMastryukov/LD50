using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterrogationManager : MonoBehaviour
{
    PlayerManager PM;
    Suspect Sus;
    InterrogationBench Bench;


    // Start is called before the first frame update
    void Start()
    {
        if (PM == null) Debug.LogError("PlayerManager is missing");
        if (Sus == null) Debug.LogError("Suspect is missing");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator InterrogationRoutine()
    {




        yield return null;
    }
}
