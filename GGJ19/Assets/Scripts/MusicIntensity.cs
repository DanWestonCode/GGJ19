using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicIntensity : MonoBehaviour
{
    float Intensity;
    float Intro;

    GameObject fm_Obj;
    int FoodCount;
    FMODUnity.StudioEventEmitter FMODEmitter;
    void Start() {
        Intensity = 0f;
        FMODEmitter = gameObject.GetComponent<FMODUnity.StudioEventEmitter>();
        //FoodCount = fm_Obj.GetComponent<FoodManager>().FoodObjects.Length; 
    }

    void Update()
    {
        if(SceneManager.GetActiveScene().name == "Game")
        {
            if(fm_Obj == null)
            {
                fm_Obj = GameObject.FindGameObjectWithTag("FoodManager");
            }
            FoodCount = fm_Obj.GetComponent<FoodManager>().FoodObjects.Length; 
            switch (FoodCount)
            {
                    case 9:
                        Intensity = 0.25f;
                        break;
                    case 5:
                        Intensity = 0.5f;
                        break;
                    case 3:
                        Intensity = 1f;
                        break;
                    default:
                        break;
                }
            FMODEmitter.SetParameter("Intensity",Intensity);
        }
    }
}
