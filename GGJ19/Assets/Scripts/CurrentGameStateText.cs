using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CurrentGameStateText : MonoBehaviour
{
    public GameObject gameStateController;
    string CurrentGameStateValue;
    string Note;
    // Start is called before the first frame update
    void Start()
    {
        CurrentGameStateValue = gameStateController.GetComponent<GameStateController>().GameState.ToString();
        Note = " - Press Space to Start!";
        gameObject.GetComponent<Text>().text = CurrentGameStateValue + Note;
    }

    // Update is called once per frame
    void Update()
    {
        if(CurrentGameStateValue != gameStateController.GetComponent<GameStateController>().GameState.ToString())
        {
            CurrentGameStateValue = gameStateController.GetComponent<GameStateController>().GameState.ToString();
            Note = "";
            if(CurrentGameStateValue == "Pending")
            {
                Note = " - Press Space to Start!";
            }
            gameObject.GetComponent<Text>().text = CurrentGameStateValue + Note;
        }
    }
}
