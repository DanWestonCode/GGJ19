using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateController : MonoBehaviour
{
    public enum EndGameStates {Pending, Good, Bad};
	public EndGameStates EndState;
    public enum GameStates {Pending, Running, Over};
	public GameStates GameState;

    void Start()
    {
        EndState = EndGameStates.Pending;
        GameState = GameStates.Pending;
    }

    void Update()
    {
        if(Input.GetKeyDown("space") && GameState == GameStates.Pending)
        {
            GameState = GameStates.Running;
            Debug.Log("Game State set to Running!");
        }
    }

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
