using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;

    public PlayerData player;
    public List<Node> allNodes = new List<Node>(); //keeping track of what node i made
     
    public int nodesPerLayer = 3;         
    public float layerHeight = 2f;          

    private int nextNodeID = 0;
    private GameEvent currentEvent;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        StartGame();
    }

    void StartGame()
    {
        //setting up player
        player = new PlayerData(100, 50, 50);

        // Makes the very first node
        Node startNode = CreateNode(Vector2.zero, "start");
        startNode.isCurrentNode = true;
        player.currentNode = startNode;

        // Generates the next node
        GenerateNextNodes(startNode, new List<string> { "mainEventA", "mainEventB", "mainEventC" });
    }

    //create the initial node
    Node CreateNode(Vector2 position, string eventType)
    {
        Node node = new Node(nextNodeID++, position, eventType);
        allNodes.Add(node);
        return node;
    }

    //create the initial three nodes after the beginning node

    public void GenerateInitialNodes()
    {   
        // Generates the next node
        //
        
     
    }


    //next nodes
    public void GenerateNextNodes(Node fromNode, List<string> eventTypes)
    {
        if (eventTypes == null || eventTypes.Count == 0)
        {
            eventTypes = GetRandomEventTypes(nodesPerLayer);
        }

        int count = eventTypes.Count;
        float newY = fromNode.position.y + layerHeight;

        for (int i = 0; i < count; i++)
        {
            float xPos = (i - (count - 1) / 2f) * 1.5f;
            Vector2 newPos = new Vector2(xPos, newY);

            Node newNode = CreateNode(newPos, eventTypes[i]);
            fromNode.nextNodeID.Add(newNode.id);
        }
    }

    public void OnNodeClicked(int nodeID)
    {
    
        Node clickedNode = null;

        foreach (Node node in allNodes)
        {
            if (node.id == nodeID)
            {
                clickedNode = node;
                break; 
            }
        }

        if (clickedNode == null)
        {
            return;
        }

        bool canGo = false;  

        foreach (int nextID in player.currentNode.nextNodeID)
        {
            if (nextID == nodeID)
            {
                canGo = true; 
                break;
            }
        }

        if (!canGo)
        {
            return;
        }

        MoveToNode(clickedNode);
    }

    void MoveToNode(Node node)
    {
        player.currentNode.isCurrentNode = false;
        player.currentNode.isVisited = true;

        player.currentNode = node;
        node.isCurrentNode = true;

        TriggerEvent(node.eventType);
    }

    void TriggerEvent(string eventType)
    {
        // EventDatabase에서 이벤트 가져오기
        //currentEvent = EventDatabase.Instance.GetRandomEventByType(eventType);

        if (currentEvent == null)
        {
            Debug.LogWarning("Cannot find events");
        }

        // UIManager.Instance?.ShowEvent(currentEvent);

    }

    public void OnChoiceSelected(EventChoice choice)
    {
        player.ApplyEffect(choice);
        GenerateNextNodes(player.currentNode, choice.nextEventTypes);

        //update UI
    }

    List<string> GetRandomEventTypes(int count)
    {
        string[] types = { "mainEventA", "mainEventB", "mainEventC", "farming" };
        List<string> result = new List<string>();

        for (int i = 0; i < count; i++)
        {
            result.Add(types[Random.Range(0, types.Length)]);
        }

        return result;
    }
}
