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
        GetComponent<NodeMovement>().setState(NodeMovement.MoveState.caught);
        GetComponent<NodeMovement>().dropFood();

        FlyState = FlyStates.Caught;
    }

    public void Kill () {
        GetComponent<NodeMovement>().setState(NodeMovement.MoveState.none);

        this.transform.position = new Vector2(2000, 2000);
        GetComponent<NodeMovement>().dropFood();
    }

    public void AtTable()
	{
        GetComponent<NodeMovement>().setState(NodeMovement.MoveState.caught);
        FlyState = FlyStates.AtTable;
	}


    public static void SomeFoodPickedUp(GameObject food)
	{
		Debug.Log("Some food was pickup check if its is my food?");
	}
}
