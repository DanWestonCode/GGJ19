using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyManager : MonoBehaviour
{
    public static FlyManager instance;

    private void Awake()
    {
        instance = this;
    }


    public int atTableWinFlyCount;

    public GameObject[] AllFlys;

    public int atTableFlyCount;
    
    public GameStateController gsc;
    void Start()
    {
        
    }

    public void TryToSpawnfly(Node spawnNode)
    {
        GameObject flyPicked = null;
        //find a fly that is not active
        for (int i = 0; i < AllFlys.Length; i++)
        {
            NodeMovement.MoveState flyState = AllFlys[i].GetComponent<NodeMovement>().getState();
            if (flyState == NodeMovement.MoveState.none)
            {
                //try to spawn the fly
                flyPicked = AllFlys[i];
                break;
            }
        }

        if (flyPicked != null)
        {
            NodeMovement flyMoveNode = flyPicked.GetComponent<NodeMovement>();
            flyPicked.transform.position = spawnNode.transform.position;
            flyMoveNode.currentNode = spawnNode;
            flyMoveNode.setState(NodeMovement.MoveState.toFood);
        }
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
