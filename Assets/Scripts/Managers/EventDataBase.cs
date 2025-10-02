using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class EventListWrapper
{
    public List<GameEvent> events;
}

public class EventDataBase: MonoBehaviour
{

    public static EventDataBase instance;
    private Dictionary<string, GameEvent> eventDictionary = new Dictionary<string, GameEvent>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            LoadEvents();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void LoadEvents()
    {
        // Resources 폴더에서 JSON 파일 로드
        TextAsset jsonFile = Resources.Load<TextAsset>("Events/battle_events");

        if (jsonFile != null)
        {
            EventListWrapper wrapper = JsonUtility.FromJson<EventListWrapper>(jsonFile.text);

            foreach (var evt in wrapper.events)
            {
                eventDictionary[evt.eventID] = evt;
            }

            Debug.Log($"이벤트 {eventDictionary.Count}개 로드 완료!");
        }
    }

    // 이벤트 타입으로 랜덤하게 하나 가져오기
    public GameEvent GetRandomEventByType(string eventType)
    {
        var matchingEvents = eventDictionary.Values.Where(e => e.eventType == eventType).ToList();

        if (matchingEvents.Count > 0)
        {
            return matchingEvents[Random.Range(0, matchingEvents.Count)];
        }

        Debug.LogWarning($"{eventType} 타입 이벤트를 찾을 수 없습니다!");
        return null;
    }

    // ID로 특정 이벤트 가져오기
    public GameEvent GetEventByID(string eventID)
    {
        if (eventDictionary.ContainsKey(eventID))
        {
            return eventDictionary[eventID];
        }

        Debug.LogWarning($"{eventID} 이벤트를 찾을 수 없습니다!");
        return null;
    }


}
