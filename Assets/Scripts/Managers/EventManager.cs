using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;

    [Header("게임 상태")]
    public PlayerData player;
    public List<Node> allNodes = new List<Node>();

    [Header("설정")]
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

    // 게임 시작
    void StartGame()
    {
        // 플레이어 초기화
        player = new PlayerData(100, 50, 50);

        // 첫 노드 생성
        Node startNode = CreateNode(Vector2.zero, "시작");
        startNode.isCurrentNode = true;
        player.currentNode = startNode;

        // 첫 다음 노드들 생성
        GenerateNextNodes(startNode, new List<string> { "전투", "상점", "전투" });

        Debug.Log("게임 시작!");
    }

    // 노드 생성
    Node CreateNode(Vector2 position, string eventType)
    {
        Node node = new Node(nextNodeID++, position, eventType);
        allNodes.Add(node);
        return node;
    }

    // 다음 노드들 생성
    public void GenerateNextNodes(Node fromNode, List<string> eventTypes)
    {
        // 비어있으면 랜덤으로
        if (eventTypes == null || eventTypes.Count == 0)
        {
            eventTypes = GetRandomEventTypes(nodesPerLayer);
        }

        int count = eventTypes.Count;
        float newY = fromNode.position.y + layerHeight;

        for (int i = 0; i < count; i++)
        {
            // X 위치를 균등하게 분배
            float xPos = (i - (count - 1) / 2f) * 1.5f;
            Vector2 newPos = new Vector2(xPos, newY);

            Node newNode = CreateNode(newPos, eventTypes[i]);

            // 연결
            fromNode.nextNodeID.Add(newNode.id);
        }

        Debug.Log($"{count}개 노드 생성됨. 총: {allNodes.Count}개");
    }

    // 노드 클릭 처리
    public void OnNodeClicked(int nodeID)
    {
        Node clickedNode = allNodes.FirstOrDefault(n => n.id == nodeID);

        if (clickedNode == null)
        {
            Debug.LogError("노드를 찾을 수 없음!");
            return;
        }

        // 갈 수 있는 노드인지 확인
        if (!player.currentNode.nextNodeID.Contains(nodeID))
        {
            Debug.Log("갈 수 없는 노드!");
            return;
        }

        // 이동
        MoveToNode(clickedNode);
    }

    // 노드로 이동
    void MoveToNode(Node node)
    {
        // 이전 노드 업데이트
        player.currentNode.isCurrentNode = false;
        player.currentNode.isVisited = true;

        // 새 노드로
        player.currentNode = node;
        node.isCurrentNode = true;

        Debug.Log($"노드 {node.id} ({node.eventType})로 이동!");

        // 이벤트 발동
        TriggerEvent(node.eventType);
    }

    // 이벤트 발동
    void TriggerEvent(string eventType)
    {
        // EventDatabase에서 이벤트 가져오기
        //currentEvent = EventDatabase.Instance.GetRandomEventByType(eventType);

        if (currentEvent == null)
        {
            Debug.LogWarning("이벤트를 찾을 수 없음! 더미 이벤트 사용");
            currentEvent = CreateDummyEvent(eventType);
        }

        // UI에 표시 (나중에 만들 UIManager)
        // UIManager.Instance?.ShowEvent(currentEvent);

        Debug.Log($"이벤트: {currentEvent.title}");
    }

    // 선택지 선택 처리
    public void OnChoiceSelected(EventChoice choice)
    {
        Debug.Log($"선택: {choice.choiceText}");
        Debug.Log($"결과: {choice.resultText}");

        // 플레이어 상태 적용
        player.ApplyEffect(choice);

        // 다음 노드들 생성
        GenerateNextNodes(player.currentNode, choice.nextEventTypes);

        // UI 업데이트
        // UIManager.Instance?.UpdateStats();
        // UIManager.Instance?.HideEvent();
    }

    // 랜덤 이벤트 타입들 생성
    List<string> GetRandomEventTypes(int count)
    {
        string[] types = { "전투", "상점", "휴식", "보물" };
        List<string> result = new List<string>();

        for (int i = 0; i < count; i++)
        {
            result.Add(types[Random.Range(0, types.Length)]);
        }

        return result;
    }

    // 임시 더미 이벤트 (EventDatabase 없을 때)
    GameEvent CreateDummyEvent(string eventType)
    {
        GameEvent evt = new GameEvent();
        evt.eventID = "dummy";
        evt.eventType = eventType;
        evt.title = $"{eventType} 이벤트";
        evt.description = "무언가 일어났다...";
        evt.choices = new List<EventChoice>
        {
            new EventChoice
            {
                choiceText = "계속한다",
                resultText = "앞으로 나아간다.",
                nextEventTypes = new List<string> { "전투", "상점", "전투" },
                nextNodeCount = 3
            }
        };

        return evt;
    }
}
