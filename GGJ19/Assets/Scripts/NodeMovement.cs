using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeMovement : MonoBehaviour {

    public enum MoveState { none, caught, toFood, toSpawn};

    [SerializeField]
    private GameObject nodesParent;

    [SerializeField]
    private MoveState state = MoveState.none;

    [SerializeField]
    private GameObject flyObject;

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

    public Node currentNode;

    private Node currentTarget;

    private List<float> pathCosts;
    private List<Node> currentPath;

    private Vector3 direction;
    private Vector3 postDirection;

    private List<Dictionary<Vector3, bool>> wobbleVectors;

    private GameObject pickedUpFood = null;


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

        startShake();
    }

    void startShake()
    {

    }

    public NodeMovement.MoveState getState()
    {
        return state;
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
            case MoveState.caught:
                //just caught
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
                    if (openList.Contains(neighbours[i]) && !neighbours[i].gameObject.GetComponent<Food>())
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
        //check whether all of the food has been picked up
        if (targetType == Node.NodeType.food) {
            bool foodAvailable = false;
            for (int i = 0; i < foodNodes.Count; i++) {
                if (foodNodes[i].gameObject.GetComponent<Food>().PickUpState == Food.PickUpStates.Static)
                {
                    foodAvailable = true;
                }
            }

            if (!foodAvailable)
            {
                setState(MoveState.toSpawn);
                return;
            }
        }

        List<Node> consideredList = targetType == Node.NodeType.food ? foodNodes : spawnNodes;

        Node closestNode = null;
        for (int i = 0; i < consideredList.Count; i++)
        {
            if (consideredList[i].GetComponent<Food>())
            {
                if (consideredList[i].GetComponent<Food>().PickUpState != Food.PickUpStates.Static)
                {
                    continue;
                }
            }

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
            case MoveState.caught:
                //don't move at all, just bob up and down/left and right
                break;

            case MoveState.toFood:
                //make sure the food has not been picked up
                if (currentTarget.GetComponent<Food>())
                {
                    if (currentTarget.GetComponent<Food>().PickUpState != Food.PickUpStates.Static)
                    {
                        currentTarget = null;
                        currentPath = null;
                    }
                }

                if (currentTarget == null)
                {
                    this.targetClosest(Node.NodeType.food);
                }

                if (currentPath == null || currentPath.Count == 0)
                {
                    pathToTarget();
                }

                //move towards the next node on the path
                moveAlongPath(MoveState.toFood);

                break;

            case MoveState.toSpawn:
                //move towards spawn node
                if (currentTarget == null)
                {
                    this.targetClosest(Node.NodeType.spawn);
                }

                if (currentPath == null || currentPath.Count == 0)
                {
                    pathToTarget();
                }

                moveAlongPath(MoveState.toSpawn);
                break;

            default:
                break;
        }

        if (pickedUpFood != null)
        {
            pickedUpFood.transform.position = transform.position + new Vector3(0.1f, 0.1f, 0.0f);
        }

        prevState = state;
	}

    void moveAlongPath(MoveState stateRef)
    {
        direction = Vector3.negativeInfinity;
        if (currentNode != null && currentPath != null && currentPath.Count != 0)
        {
            if (currentNode == currentPath[0])
            {
                //take the start off of the list
                currentPath.RemoveAt(0);
            }

            //move towards the top node of the current path
            direction = Vector3.Normalize(currentPath[0].gameObject.transform.position - gameObject.transform.position);

            gameObject.transform.position += (direction * Time.deltaTime * 10);
        }
    }

    private void LateUpdate()
    {
        switch (state)
        {
            case MoveState.caught:
                //don't move at all, just bob up and down/left and right
                break;

            case MoveState.toFood:
                //make sure the food has not been picked up

                //move towards the next node on the path
                moveAlongPathPost(MoveState.toFood);

                break;

            case MoveState.toSpawn:
                moveAlongPathPost(MoveState.toSpawn);
                break;

            default:
                break;
        }
    }

    void moveAlongPathPost(MoveState stateRef)
    {
        if (direction != Vector3.negativeInfinity)
        {
            postDirection = Vector3.Normalize(currentPath[0].gameObject.transform.position - gameObject.transform.position);
            if (postDirection != direction || direction == Vector3.zero)
            {
                //we have passed the point
                gameObject.transform.position = currentPath[0].gameObject.transform.position;

                currentNode = currentPath[0];

                //if we were moving towards the targetNode
                if (currentPath[0] == currentTarget)
                {
                    //we have reached the target, do the thing we need to do
                    if (stateRef == MoveState.toFood)
                    {
                        //reached food, pick it up
                        Debug.Log("finding food component");

                        Debug.Log(currentTarget.gameObject.tag);

                        if (currentTarget.gameObject.GetComponent<Food>())
                        {
                            Debug.Log("food component found");
                            currentTarget.gameObject.GetComponent<Food>().PickUp();
                            pickedUpFood = currentTarget.gameObject;
                        }

                        setState(MoveState.toSpawn);
                    }
                    else if (stateRef == MoveState.toSpawn)
                    {
                        //if reached spawn
                        //if carrying food
                        if (pickedUpFood != null)
                        {
                            pickedUpFood.GetComponent<Food>().PickUpState = Food.PickUpStates.Stolen;
                            pickedUpFood.transform.position = new Vector3(-2000, 0, 0);
                            pickedUpFood = null;
                        }

                        setState(MoveState.none);
                    }
                }
            }
        }
    }

    public void dropFood()
    {
        if (pickedUpFood != null)
        {
            pickedUpFood.GetComponent<Food>().SetToStartLocation();
            pickedUpFood = null;
        }
    }
}
