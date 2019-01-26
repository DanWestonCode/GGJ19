using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodManager : MonoBehaviour {

	public int StartQuantity;

	public GameObject FoodPrefab;

	public GameObject[] FoodObjects; 

	// Use this for initialization
	void Start () {
		// Spawn Food into the level
		for (int i = 0; i < StartQuantity; i++)
		{
			// Create Food Object
			Instantiate(FoodPrefab);
		}

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
