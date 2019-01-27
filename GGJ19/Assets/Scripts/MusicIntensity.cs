using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicIntensity : MonoBehaviour
{
    float Intensity;
    float Intro;

    GameObject fm_Obj;
    int FoodCount;
    void Start() {
        Intensity = 0f;
        Intro = 0;
        FoodCount = fm_Obj.GetComponent<FoodManager>().FoodObjects.Length; 
    }

    void Update()
    {
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
    }
}
