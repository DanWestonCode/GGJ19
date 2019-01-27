using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLoseImage : MonoBehaviour
{
    public GameObject gsm;
    public GameObject image;
    void Update()
    {

        if(gsm.GetComponent<GameStateController>().GameState == GameStateController.GameStates.Over)
        {

            if(gsm.GetComponent<GameStateController>().EndState == GameStateController.EndGameStates.Bad)
            {
                image.SetActive(true);
            }
        }
    }
}
