using UnityEngine;
using System.Collections.Generic;
using System.Linq;

//Will load json database


[System.Serializable]
public class EventListWrapper
{
    public List<GameEvent> events;
}

public class EventDataBase: MonoBehaviour
{

    public static EventDataBase instance; //make it singleton pattern
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
        TextAsset jsonFile = Resources.Load<TextAsset>("planetNodes_Events"); //load jsonfile from resources folder
        Debug.Log(jsonFile.text);

        if (jsonFile != null)
        {
            EventListWrapper wrapper = JsonUtility.FromJson<EventListWrapper>(jsonFile.text);
            // changes the texts within the textfile into c# object

            foreach (var events in wrapper.events)
            {
                //Add all the events into the dictionary
                eventDictionary[events.eventID] = events;
            }
        }
    }
    
    public GameEvent GetEventByID(string eventID)
    {

        //Get events by ID if it exist in the dictionary then u got it
        if (eventDictionary.ContainsKey(eventID))
        {
            //search for direct id
            return eventDictionary[eventID];
        }

        Debug.LogWarning($"{eventID} cnat find this event");
        return null;
    }

    /*
    //Get Random event by type
    public GameEvent GetRandomEventByType(string eventType)
    {
        List<GameEvent> matchingEvents = new List<GameEvent>();

        foreach (GameEvent events in eventDictionary.Values)
        {
            if (events.eventType == eventType)
            {
                 matchingEvents.Add(events);
            }
        }

        if (matchingEvents.Count > 0)
        {
            int randomIndex = Random.Range(0, matchingEvents.Count);
            return matchingEvents[randomIndex];
        }
 
        return null;
    }
    */


}
