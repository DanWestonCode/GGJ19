using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Fly : MonoBehaviour {


	public GameObject foodManagerGameObject;
	public FoodManager foodManagerComponent;

	public enum FlyStates {Free, AtTable, Caught};
	public FlyStates FlyState;

	public int Gender;


	// Use this for initialization
	void Start () 
	{
		FoodManager.OnPickUpFood += SomeFoodPickedUp;
		FlyState = FlyStates.Free;
		Gender = Random.Range(0,1);
	}
    
    public void Caught () {
		
    }

	public void AtTable()
	{
		FlyState = FlyStates.AtTable;
	}


    public static void SomeFoodPickedUp(GameObject food)
	{
		Debug.Log("Some food was pickup check if its is my food?");
	}
}
