﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeMovement : MonoBehaviour {

    public enum MoveState { none, hover, toFood, toSpawn};

    [SerializeField]
    private GameObject nodesParent;

    [SerializeField]
    private MoveState state = MoveState.none;

    private MoveState prevState = MoveState.none;

    //all of the nodes
    private List<Node> nodeList;
    //only the spawn nodes
    private List<Node> spawnNodes;
    //only the food nodes
    private List<Node> foodNodes;

    //A* lists
    private List<Node> openList;
    private List<Node> closedList;

    [SerializeField]
    private Node currentNode;

    private Node currentTarget;

    private List<float> pathCosts;
    private List<Node> currentPath;

	void Start () {
		if (nodesParent != null)
        {
            //grab the list of nodes from the nodesParent for later use
            nodeList = new List<Node>(nodesParent.GetComponentsInChildren<Node>());

            List<Node> newFoodList = new List<Node>();
            List<Node> newSpawnList = new List<Node>();
            //filter these into food and spawn lists
            for (int i = 0; i < nodeList.Count; i++)
            {
                if (nodeList[i].getType() == Node.NodeType.food)
                {
                    newFoodList.Add(nodeList[i]);
                }
                else if (nodeList[i].getType() == Node.NodeType.spawn)
                {
                    newSpawnList.Add(nodeList[i]);
                }
            }

            foodNodes = newFoodList;
            spawnNodes = newSpawnList;
        }
	}

    public void setState(MoveState newState)
    {
        //clear the current target
        currentTarget = null;

        //change the state
        state = newState;

        handleNewState();
    }

    void handleNewState()
    {
        switch (state)
        {
            case MoveState.hover:
                //just hover
                break;

            case MoveState.toFood:
                //find and target the closest food
                targetClosest(Node.NodeType.food);

                pathToTarget();
                break;

            case MoveState.toSpawn:
                //find and target the closest spawn
                targetClosest(Node.NodeType.spawn);

                pathToTarget();
                break;

            default:
                //do nothing
                break;
        }
    }

    void pathToTarget()
    {
        //create a path to the goal node using the A* algorithm
        resetLists();

        pathCosts = new List<float>() { 0.0f };
        currentPath = new List<Node>() { currentNode };

        openList.Remove(currentNode);

        while (openList.Count > 0)
        {
            //find node with lowest f on the open list, that is a neighbour of the current node
            float lowestF = float.PositiveInfinity;
            Node bestNode = null;
            List<Node> neighbours = currentPath[currentPath.Count - 1].Neighbours;
            //check whether the target node is a neighbour
            if (neighbours.Contains(currentTarget))
            {
                bestNode = currentTarget;
            }
            else
            {
                for (int i = 0; i < neighbours.Count; i++)
                {
                    if (openList.Contains(neighbours[i]))
                    {
                        float nodeF = calculateF(neighbours[i], pathCosts);
                        if (nodeF < lowestF)
                        {
                            lowestF = nodeF;
                            bestNode = neighbours[i];
                        }
                    }
                }
            }

            //if one does not exist, move the path backwards, and remove the last entry into pathCosts
            if (bestNode == null)
            {
                //roll the search back one node
                pathCosts.RemoveAt(pathCosts.Count - 1);
                currentPath.RemoveAt(currentPath.Count - 1);
                continue;
            }
            else
            {
                //add it's distance to the total path
                float cost = Vector3.Distance(currentPath[currentPath.Count - 1].gameObject.transform.position, bestNode.gameObject.transform.position);
                pathCosts.Add(cost);

                currentPath.Add(bestNode);

                //remove it from the open list
                openList.Remove(bestNode);
            }

            if (bestNode == currentTarget)
            {
                break;
            }
        }

        //Debug.Log(currentPath);
    }

    float calculateF(Node nodeRef, List<float> pathCosts)
    {
        float distanceCost = Vector3.Distance(currentPath[currentPath.Count - 1].gameObject.transform.position, nodeRef.gameObject.transform.position);
        float g = distanceCost;
        for (int i = 0; i < pathCosts.Count; i++)
        {
            g += pathCosts[i];
        }

        float h = Vector3.Distance(currentTarget.gameObject.transform.position, nodeRef.gameObject.transform.position);

        return (g + h);
    }

    void resetLists()
    {
        openList = new List<Node>(nodeList);
    }

    void targetClosest(Node.NodeType targetType)
    {
        List<Node> consideredList = targetType == Node.NodeType.food ? foodNodes : spawnNodes;

        Node closestNode = null;
        for (int i = 0; i < consideredList.Count; i++)
        {
            if (closestNode == null)
            {
                closestNode = consideredList[i];
            } 
            else if (closestNode != consideredList[i])
            {
                float currentClosest = Vector3.Distance(gameObject.transform.position, closestNode.gameObject.transform.position);
                float currentConsidered = Vector3.Distance(gameObject.transform.position, consideredList[i].gameObject.transform.position);

                if (currentConsidered < currentClosest) 
                {
                    closestNode = consideredList[i];
                }
            }
        }

        if (closestNode != null)
        {
            currentTarget = closestNode;
        }
    }
	
	// Update is called once per frame
	void Update () {
		switch (state)
        {
            case MoveState.hover:
                //don't move at all, just bob up and down/left and right
                break;

            case MoveState.toFood:
                //make sure the food has not been picked up

                //move towards the target food

                break;

            case MoveState.toSpawn:
                //move towards spawn node
                

                break;

            default:
                break;
        }

        prevState = state;
	}
}