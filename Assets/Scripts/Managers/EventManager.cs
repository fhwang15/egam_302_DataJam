using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;

    [Header("���� ����")]
    public PlayerData player;
    public List<Node> allNodes = new List<Node>();

    [Header("����")]
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

    // ���� ����
    void StartGame()
    {
        // �÷��̾� �ʱ�ȭ
        player = new PlayerData(100, 50, 50);

        // ù ��� ����
        Node startNode = CreateNode(Vector2.zero, "����");
        startNode.isCurrentNode = true;
        player.currentNode = startNode;

        // ù ���� ���� ����
        GenerateNextNodes(startNode, new List<string> { "����", "����", "����" });

        Debug.Log("���� ����!");
    }

    // ��� ����
    Node CreateNode(Vector2 position, string eventType)
    {
        Node node = new Node(nextNodeID++, position, eventType);
        allNodes.Add(node);
        return node;
    }

    // ���� ���� ����
    public void GenerateNextNodes(Node fromNode, List<string> eventTypes)
    {
        // ��������� ��������
        if (eventTypes == null || eventTypes.Count == 0)
        {
            eventTypes = GetRandomEventTypes(nodesPerLayer);
        }

        int count = eventTypes.Count;
        float newY = fromNode.position.y + layerHeight;

        for (int i = 0; i < count; i++)
        {
            // X ��ġ�� �յ��ϰ� �й�
            float xPos = (i - (count - 1) / 2f) * 1.5f;
            Vector2 newPos = new Vector2(xPos, newY);

            Node newNode = CreateNode(newPos, eventTypes[i]);

            // ����
            fromNode.nextNodeID.Add(newNode.id);
        }

        Debug.Log($"{count}�� ��� ������. ��: {allNodes.Count}��");
    }

    // ��� Ŭ�� ó��
    public void OnNodeClicked(int nodeID)
    {
        Node clickedNode = allNodes.FirstOrDefault(n => n.id == nodeID);

        if (clickedNode == null)
        {
            Debug.LogError("��带 ã�� �� ����!");
            return;
        }

        // �� �� �ִ� ������� Ȯ��
        if (!player.currentNode.nextNodeID.Contains(nodeID))
        {
            Debug.Log("�� �� ���� ���!");
            return;
        }

        // �̵�
        MoveToNode(clickedNode);
    }

    // ���� �̵�
    void MoveToNode(Node node)
    {
        // ���� ��� ������Ʈ
        player.currentNode.isCurrentNode = false;
        player.currentNode.isVisited = true;

        // �� ����
        player.currentNode = node;
        node.isCurrentNode = true;

        Debug.Log($"��� {node.id} ({node.eventType})�� �̵�!");

        // �̺�Ʈ �ߵ�
        TriggerEvent(node.eventType);
    }

    // �̺�Ʈ �ߵ�
    void TriggerEvent(string eventType)
    {
        // EventDatabase���� �̺�Ʈ ��������
        //currentEvent = EventDatabase.Instance.GetRandomEventByType(eventType);

        if (currentEvent == null)
        {
            Debug.LogWarning("�̺�Ʈ�� ã�� �� ����! ���� �̺�Ʈ ���");
            currentEvent = CreateDummyEvent(eventType);
        }

        // UI�� ǥ�� (���߿� ���� UIManager)
        // UIManager.Instance?.ShowEvent(currentEvent);

        Debug.Log($"�̺�Ʈ: {currentEvent.title}");
    }

    // ������ ���� ó��
    public void OnChoiceSelected(EventChoice choice)
    {
        Debug.Log($"����: {choice.choiceText}");
        Debug.Log($"���: {choice.resultText}");

        // �÷��̾� ���� ����
        player.ApplyEffect(choice);

        // ���� ���� ����
        GenerateNextNodes(player.currentNode, choice.nextEventTypes);

        // UI ������Ʈ
        // UIManager.Instance?.UpdateStats();
        // UIManager.Instance?.HideEvent();
    }

    // ���� �̺�Ʈ Ÿ�Ե� ����
    List<string> GetRandomEventTypes(int count)
    {
        string[] types = { "����", "����", "�޽�", "����" };
        List<string> result = new List<string>();

        for (int i = 0; i < count; i++)
        {
            result.Add(types[Random.Range(0, types.Length)]);
        }

        return result;
    }

    // �ӽ� ���� �̺�Ʈ (EventDatabase ���� ��)
    GameEvent CreateDummyEvent(string eventType)
    {
        GameEvent evt = new GameEvent();
        evt.eventID = "dummy";
        evt.eventType = eventType;
        evt.title = $"{eventType} �̺�Ʈ";
        evt.description = "���� �Ͼ��...";
        evt.choices = new List<EventChoice>
        {
            new EventChoice
            {
                choiceText = "����Ѵ�",
                resultText = "������ ���ư���.",
                nextEventTypes = new List<string> { "����", "����", "����" },
                nextNodeCount = 3
            }
        };

        return evt;
    }
}
