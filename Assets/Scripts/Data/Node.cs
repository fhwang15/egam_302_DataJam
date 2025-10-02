using UnityEngine;
using System.Collections.Generic;

public class Node
{

    public int id;
    public Vector2 position; //position in the screen
    public string eventType; //what kind of event it is
    public bool isVisited; 
    public bool isCurrentNode; //if the player is currently on this node
    public List<int> nextNodeID; //next available nodes

    //Will create a new node
    public Node(int id, Vector2 position, string type)
    {
        this.id = id;
        this.position = position;
        this.eventType = type;
        this.isVisited = false;
        this.isCurrentNode = false;
        this.nextNodeID = new List<int>();
    }
}
