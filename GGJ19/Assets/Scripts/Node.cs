using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {

    public enum NodeType { movement, food, spawn};

    [SerializeField]
    private NodeType Type;

    public List<Node> Neighbours;

    private bool CountForSpawn = false;

    private float SpawnStartDelay = 0.0f;
    private float SpawnTimeStart = 10.0f;
    private float SpawnTimer;

    public NodeType getType()
    {
        return Type;
    }

    public void StartSpawning(float delay)
    {
        //start spawning every 5 seconds after the delay
        SpawnStartDelay = delay;
        CountForSpawn = true;
        SpawnTimer = SpawnTimeStart;
    }

    public void StopSpawning()
    {
        //stop spawning
        CountForSpawn = false;
        SpawnTimer = SpawnTimeStart;
    }

    //need to reassign neighbors when food is dropped
    public void handleDropped()
    {
        //go back to the food spawn area stored in Food.cs
    }

    private void Update()
    {
        if (CountForSpawn)
        {
            if (SpawnStartDelay > 0.0f)
            {
                SpawnStartDelay -= Time.deltaTime;
                if (SpawnStartDelay <= 0.0f)
                {
                    SpawnStartDelay = 0.0f;

                    TryToSpawnFly();
                }
            }
            else
            {
                SpawnTimer -= Time.deltaTime;

                if (SpawnTimer <= 0.0f)
                {
                    TryToSpawnFly();

                    ResetTimer();
                }
            }
        }
    }

    private void ResetTimer()
    {
        SpawnTimer = SpawnTimeStart;
    }


    private void TryToSpawnFly()
    {
        Debug.Log("spawn fly plz (Node.cs)");
        //go through the list of flies, if one is not spawned, spawn it here
        FlyManager.instance.TryToSpawnfly(this);
    }
}
