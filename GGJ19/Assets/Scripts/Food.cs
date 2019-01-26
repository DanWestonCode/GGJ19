using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Food : MonoBehaviour {
	public GameObject foodManagerGameObject;
	public FoodManager foodManagerComponent;
	public enum PickUpStates {gotPickedUp, isPickedUp, gotDropped, Static};
	public PickUpStates PickUpState;

	void Start () {
		PickUpState = PickUpStates.Static;
		foodManagerComponent = foodManagerGameObject.GetComponent<FoodManager>();
	}

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
