using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlyCounterText : MonoBehaviour
{
    public GameObject flyManagerGameObject;
    FlyManager flyManager;
    void Start()
    {
        flyManagerGameObject = GameObject.FindGameObjectWithTag("FlyManager");
        flyManager = flyManagerGameObject.GetComponent<FlyManager>();
        gameObject.GetComponent<Text>().text = flyManager.atTableWinFlyCount.ToString();
    }

    
    void Update()
    {
        //Debug.Log(gameObject.GetComponent<Text>().text);
        //Debug.Log(flyManager.atTableFlyCount.ToString());
        int count =  flyManager.atTableWinFlyCount - flyManager.atTableFlyCount;
        if(gameObject.GetComponent<Text>().text != count.ToString())
        {
            gameObject.GetComponent<Text>().text = count.ToString();
        }
    }
}
