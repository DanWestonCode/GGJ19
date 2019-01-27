using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicIntensity : MonoBehaviour
{
    float Intensity;
    GameObject fm_Obj;
    int FoodCount;
    void Start() {
        Intensity = 0f;
        FoodCount = fm_Obj.GetComponent<FoodManager>().FoodObjects.Length; 

    }

    void Update()
    {
        
    }
}
