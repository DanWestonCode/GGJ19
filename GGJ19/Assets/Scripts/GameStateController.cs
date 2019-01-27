using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateController : MonoBehaviour
{
    public enum EndGameStates {Pending, Good, Bad};
	public EndGameStates EndState;
    public enum GameStates {Pending, Running, Over};
	public GameStates GameState;

    //public List<Node> SpawnNodes;

    void Start()
    {
        EndState = EndGameStates.Pending;
        GameState = GameStates.Pending;
    }

    void Update()
    {
        if(Input.GetKeyDown("space") && GameState == GameStates.Pending)
        {
            SetGameStateToRunning();
        }
        if(GameState == GameStates.Over)
        {
            switch (EndState)
            {
                case EndGameStates.Good:
                    GameObject.FindGameObjectWithTag("end_win").SetActive(true);
                    break;
                case EndGameStates.Bad:
                    GameObject.FindGameObjectWithTag("end_lose").SetActive(true);
                    break;
                default:
                break;
            }
        }
    }

    public void SetGameStateToRunning()
    {
        GameState = GameStates.Running;
        Debug.Log("Game State set to Running!");

        //StartSpawntimers();

    }

/*    void StartSpawntimers()
    {
        if (SpawnNodes.Count != 0)
        {
            for (int i = 0; i < SpawnNodes.Count; i++)
            {
                float delay = i * 2;
                //SpawnNodes[i].StartSpawning(delay);
            }
        }
    }

    void StopSpawntimers()
    {
        if (SpawnNodes.Count != 0)
        {
            for (int i = 0; i < SpawnNodes.Count; i++)
            {
                //SpawnNodes[i].StopSpawning();
            }
        }
    }
*/
    public void SetGameStateToOver()
    {
        GameState = GameStates.Over;
    }
    public void SetEndStateToGood()
    {
        EndState = EndGameStates.Good;
    }
    public void SetEndStateToBad()
    {
        EndState = EndGameStates.Bad;
    }
}
