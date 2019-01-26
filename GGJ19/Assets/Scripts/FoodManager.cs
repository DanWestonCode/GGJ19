using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;

public class FoodManager : MonoBehaviour {
	public int StartQuantity;
	public GameObject FoodPrefab;
	public GameObject Home;
	public List<GameObject> FoodObjects; 
	public delegate void PickUpFoodEvent(GameObject food);
	public static event PickUpFoodEvent OnPickUpFood;
	void Start () {
		CreateStartingFood();		
	}
	void Update () {
	}
	void CreateStartingFood()
	{
		SpriteRenderer sr = Home.GetComponent<SpriteRenderer>();

		float maxXpos = sr.bounds.max.x;
		float maxYpos = sr.bounds.max.y;

		float minXpos = sr.bounds.min.x;
		float minYpos = sr.bounds.min.y;

		for (int i = 0; i < StartQuantity; i++)
		{
			float randomXpos = Random.Range(minXpos,maxXpos);
			float randomYpos = Random.Range(minYpos,maxYpos);
			Vector2 pos = new Vector2(randomXpos,randomYpos);
			// Create Food Object
			GameObject newGameObject = Instantiate(FoodPrefab,pos,Quaternion.identity);
			newGameObject.name = ("Food " + i);
			FoodObjects.Add(newGameObject);
		}
	}
	public void LogPickedUp(GameObject f)
	{
		Debug.Log("Calling Event for picked up fly!");
		if(OnPickUpFood != null)
			OnPickUpFood(f);
	}

	public bool IsFoodPickUp(GameObject food)
	{
		Food foodComponent = food.GetComponent<Food>();
		if(foodComponent.PickUpState == Food.PickUpStates.isPickedUp)
		{
			return true;
		}
		return false;
	}

}


[CustomEditor(typeof(FoodManager))]
public class FoodManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        FoodManager myScript = (FoodManager)target;
        if(GUILayout.Button("Remove and Recreate Food Objects"))
        {
            Debug.Log("Pressed Button");
        }
    }
}