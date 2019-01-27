using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyManager : MonoBehaviour
{

    public int atTableWinFlyCount;

    public GameObject[] AllFlys;

    public int atTableFlyCount;
    
    public GameStateController gsc;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        atTableFlyCount = 0;
        AllFlys = GameObject.FindGameObjectsWithTag("Fly");
        foreach (GameObject fly in AllFlys)
        {
            if(fly.GetComponent<Fly>().FlyState == Fly.FlyStates.AtTable)
            {
                atTableFlyCount ++;
            }
        }
        if(atTableFlyCount >= atTableWinFlyCount)
        {
            gsc.GetComponent<GameStateController>().SetGameStateToOver();
            gsc.GetComponent<GameStateController>().SetEndStateToGood();
        }

    }
}
