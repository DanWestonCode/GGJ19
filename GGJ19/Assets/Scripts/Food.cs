using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Food : MonoBehaviour {
	public GameObject foodManagerGameObject;
	public FoodManager foodManagerComponent;
	public enum PickUpStates {gotPickedUp, isPickedUp, gotDropped, Static};
	public PickUpStates PickUpState;

	// Use this for initialization
	void Start () {
		PickUpState = PickUpStates.Static;
		foodManagerComponent = foodManagerGameObject.GetComponent<FoodManager>();
	}

	// Update is called once per frame
	void Update () 
	{
		if(PickUpState == PickUpStates.gotPickedUp)
		{
			PickUp();
		}
	}
	public void PickUp()
	{
		PickUpState = PickUpStates.gotPickedUp;
		foodManagerComponent.LogPickedUp(this.gameObject);
		PickUpState = PickUpStates.isPickedUp;
	}
}
