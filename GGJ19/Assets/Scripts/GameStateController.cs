using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateController : MonoBehaviour
{
    public enum EndGameStates {Pending, Good, Bad};
	public EndGameStates EndState;
    public enum GameStates {Pending, Running, Over};
	public GameStates GameState;

    public List<Node> SpawnNodes;

    void Start()
    {
        EndState = EndGameStates.Pending;
        GameState = GameStates.Pending;

        SetGameStateToRunning();
    }

    void Update()
    {
        if(Input.GetKeyDown("space") && GameState == GameStates.Pending)
        {
        }
    }

    public void SetGameStateToRunning()
    {
        GameState = GameStates.Running;

        StartSpawntimers();

    }

    void StartSpawntimers()
    {
        if (SpawnNodes.Count != 0)
        {
            for (int i = 0; i < SpawnNodes.Count; i++)
            {
                float delay = i;
                SpawnNodes[i].StartSpawning(delay);
            }
        }
    }

    void StopSpawntimers()
    {
        if (SpawnNodes.Count != 0)
        {
            for (int i = 0; i < SpawnNodes.Count; i++)
            {
                SpawnNodes[i].StopSpawning();
            }
        }
    }

    public void SetGameStateToOver()
    {
        StopSpawntimers();

        GameState = GameStates.Over;
    }
    public void SetEndStateToGood()
    {
        StopSpawntimers();

        StopFlies();

        EndState = EndGameStates.Good;
    }
    public void SetEndStateToBad()
    {
        StopSpawntimers();

        StopFlies();

        EndState = EndGameStates.Bad;
    }

    private void StopFlies()
    {
        //go through the flies on the fly manager and stop their movement
        GameObject[] allFlies = FlyManager.instance.AllFlys;

        for (int i = 0; i < allFlies.Length; i++)
        {
            allFlies[i].GetComponent<NodeMovement>().setState(NodeMovement.MoveState.none);
        }
    }
}
