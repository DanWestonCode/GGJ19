using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {

    public enum NodeType { movement, food, spawn};

    [SerializeField]
    private NodeType Type;

    public List<Node> Neighbours;

    public NodeType getType()
    {
        return Type;
    }

    //need to reassign neighbors when food is dropped
    public void handleDropped()
    {
        //find nearest 4 other nodes that are above this position on the y axis
        //go through in order of distance, once one has been found that has a neighbour that is another node in the list, this must be the neighbour
    }

}
