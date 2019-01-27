using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Fly : MonoBehaviour {

	public GameObject foodManagerGameObject;
	public FoodManager foodManagerComponent;

	// Use this for initialization
	void Start () 
	{
		FoodManager.OnPickUpFood += SomeFoodPickedUp;
	}
    
    public void Caught () {

    }

    public static void SomeFoodPickedUp(GameObject food)
	{
		Debug.Log("Some food was pickup check if its is my food?");
	}
}
