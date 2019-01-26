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

	public GameObject[] FoodSpawnAreas;
	public Sprite[] FoodSprites;

	void Start () {
		CreateStartingFood();		
	}
	void Update () {
	}
	void CreateStartingFood()
	{
		FoodSpawnAreas = GameObject.FindGameObjectsWithTag("FoodSpawnArea");
		//SpriteRenderer sr = Home.GetComponent<SpriteRenderer>();

		for (int i = 0; i < StartQuantity; i++)
		{
			int number = Random.Range(0,FoodSpawnAreas.Length);
			SpriteRenderer sr = FoodSpawnAreas[number].GetComponent<SpriteRenderer>();
			float maxXpos = sr.bounds.max.x;
			float maxYpos = sr.bounds.max.y;
			float minXpos = sr.bounds.min.x;
			float minYpos = sr.bounds.min.y;
			float randomXpos = Random.Range(minXpos,maxXpos);
			float randomYpos = maxYpos;	
			Vector2 pos = new Vector2(randomXpos,randomYpos);
			// Create Food Object
			GameObject newGameObject = Instantiate(FoodPrefab,pos,Quaternion.identity);
			newGameObject.GetComponent<SpriteRenderer>().sprite = SelectFoodSprite();
			newGameObject.name = ("Food " + i);
			//Debug.Log("Food Size: " + newGameObject.GetComponent<SpriteRenderer>().bounds.size.x);
			FoodObjects.Add(newGameObject);
		}
		foreach (GameObject Areas in FoodSpawnAreas)
		{
			//Areas.GetComponent<SpriteRenderer>().enabled = false;
		}
	}


	Sprite SelectFoodSprite()
	{
		int number = Random.Range(0,FoodSprites.Length);
		return FoodSprites[number];
	}

	public void RecreateStartingFood()
	{
		foreach (var food in FoodObjects)
		{
			Destroy(food);
		}
		FoodObjects.Clear();
		CreateStartingFood();
	}

	public void LogPickedUp(GameObject f)
	{
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
			GameObject fm = GameObject.Find("FoodManager");
			FoodManager fMComponent = fm.GetComponent<FoodManager>();
            fMComponent.RecreateStartingFood();
        }
    }
}