using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawnArea : MonoBehaviour
{
    /* 
    float gapSize = 2;
    float newPositionValue;
    public float AreaWidth;
    SpriteRenderer spriteRenderer;
    public List<PossableFoodPosition> FoodPositions;
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        AreaWidth = spriteRenderer.bounds.size.x;
        newPositionValue = gapSize;
        Debug.Log("AreaWidth: " + AreaWidth);
        Debug.Log("newPositionValue: " + newPositionValue);
        Debug.Log("gapSize: " + gapSize);
        Debug.Log("Min X: " + spriteRenderer.bounds.size.x);
        FoodPositions.Add(new PossableFoodPosition(spriteRenderer.bounds.min.x + newPositionValue));
        while(newPositionValue < AreaWidth + gapSize) 
        {
            newPositionValue = newPositionValue + gapSize;
            Debug.Log("Next newPositionValue: " + newPositionValue);
            Debug.Log("New X Position: " + spriteRenderer.bounds.min.x + newPositionValue);
            FoodPositions.Add(new PossableFoodPosition(spriteRenderer.bounds.min.x + newPositionValue));
            Debug.Log("AreaWidth: " + AreaWidth);
        }
        Debug.Log(FoodPositions);
    } */  
    void Update()
    {
    }
}

/* public class PossableFoodPosition
{
    public bool IsAvailable = true;
    public float x;
    public PossableFoodPosition(float _x)
    {
        x = _x;
    }
}*/
