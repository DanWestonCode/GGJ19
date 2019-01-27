using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
  #if UNITY_EDITOR
    using UnityEditor;
  #endif

public class FoodManager : MonoBehaviour {
	public int StartQuantity;
	public GameObject FoodPrefab;
	public GameObject Home;
	public GameObject gameStateController;
	public List<GameObject> FoodObjects; 
	public delegate void PickUpFoodEvent(GameObject food);
	public static event PickUpFoodEvent OnPickUpFood;

	public GameObject[] FoodSpawnAreas;
	public Sprite[] FoodSprites;
	public bool ShowFoodSpawnAreasInGame;
	void Start () {
		//CreateStartingFood();		
	}
	void Update () 
	{
		CheckFoodStolenCount();
	}

	void CheckFoodStolenCount()
	{
		int StolenCount = 0;
		foreach (GameObject food in FoodObjects)
		{
			if(food.GetComponent<Food>().PickUpState == Food.PickUpStates.Stolen)
			{
				StolenCount ++;
			}
		}
		if(StolenCount >= FoodObjects.Count)
		{
			gameStateController.GetComponent<GameStateController>().SetGameStateToOver();
			gameStateController.GetComponent<GameStateController>().SetEndStateToBad();

		}
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
			float randomYpos = minYpos;	
			Vector2 pos = new Vector2(randomXpos,randomYpos);
			// Create Food Object
			GameObject newGameObject = Instantiate(FoodPrefab,pos,Quaternion.identity);
			newGameObject.GetComponent<SpriteRenderer>().sprite = SelectFoodSprite();
			Vector3 currnetPostion = newGameObject.transform.position;
			newGameObject.transform.position = new Vector3(currnetPostion.x, currnetPostion.y + (newGameObject.GetComponent<SpriteRenderer>().bounds.size.x)/2, currnetPostion.z);
			newGameObject.name = ("Food " + i);
			//Debug.Log("Food Size: " + newGameObject.GetComponent<SpriteRenderer>().bounds.size.x);
			FoodObjects.Add(newGameObject);
		}
		if(ShowFoodSpawnAreasInGame)
		{
			foreach (GameObject Areas in FoodSpawnAreas)
			{
				Areas.GetComponent<SpriteRenderer>().enabled = false;
			}
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

#if UNITY_EDITOR
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
#endif