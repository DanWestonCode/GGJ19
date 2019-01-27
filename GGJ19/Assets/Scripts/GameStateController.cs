using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateController : MonoBehaviour
{
    public enum EndGameStates {Pending, Good, Bad};
	public EndGameStates EndState;
    public enum GameStates {Pending, Running, Over};
	public GameStates GameState;

    // Start is called before the first frame update
    void Start()
    {
        EndState = EndGameStates.Pending;
        GameState = GameStates.Pending;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetGameStateToOver()
    {
        Debug.Log("Ending Game!");
        GameState = GameStates.Over;
        Debug.Log(GameState);
    }
}
