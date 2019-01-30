using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Food : MonoBehaviour {

	public Vector3 StartLocation;

	public GameObject foodManagerGameObject;
	public FoodManager foodManagerComponent;
	public enum PickUpStates {gotPickedUp, isPickedUp, gotDropped, Static,Stolen};
	public PickUpStates PickUpState;

	void Start () {
		StartLocation = transform.position;
		PickUpState = PickUpStates.Static;
		foodManagerComponent = foodManagerGameObject.GetComponent<FoodManager>();
		Vector3 targetPosition = gameObject.transform.position - (new Vector3(0,1,0));
		//iTween.MoveTo(gameObject,targetPosition,10f);
		iTween.PunchPosition(gameObject,(new Vector3(1,0,0)),1f);
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
		
        FMODUnity.RuntimeManager.PlayOneShot("event:/Flappy Fly/Fly_Food");

    }
    public void SetToStartLocation()
	{
		transform.position = StartLocation;
	}
}
