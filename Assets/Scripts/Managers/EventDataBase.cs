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
        // Resources �������� JSON ���� �ε�
        TextAsset jsonFile = Resources.Load<TextAsset>("Events/battle_events");

        if (jsonFile != null)
        {
            EventListWrapper wrapper = JsonUtility.FromJson<EventListWrapper>(jsonFile.text);

            foreach (var evt in wrapper.events)
            {
                eventDictionary[evt.eventID] = evt;
            }

            Debug.Log($"�̺�Ʈ {eventDictionary.Count}�� �ε� �Ϸ�!");
        }
    }

    // �̺�Ʈ Ÿ������ �����ϰ� �ϳ� ��������
    public GameEvent GetRandomEventByType(string eventType)
    {
        var matchingEvents = eventDictionary.Values.Where(e => e.eventType == eventType).ToList();

        if (matchingEvents.Count > 0)
        {
            return matchingEvents[Random.Range(0, matchingEvents.Count)];
        }

        Debug.LogWarning($"{eventType} Ÿ�� �̺�Ʈ�� ã�� �� �����ϴ�!");
        return null;
    }

    // ID�� Ư�� �̺�Ʈ ��������
    public GameEvent GetEventByID(string eventID)
    {
        if (eventDictionary.ContainsKey(eventID))
        {
            return eventDictionary[eventID];
        }

        Debug.LogWarning($"{eventID} �̺�Ʈ�� ã�� �� �����ϴ�!");
        return null;
    }


}
