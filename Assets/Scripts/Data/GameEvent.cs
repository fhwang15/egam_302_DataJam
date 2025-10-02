using UnityEngine;
using System.Collections.Generic;

//Shown in the inspector
[System.Serializable]
public class GameEvent
{
    public string eventID;
    public string eventType;
    public string title;
    public string description;
    public List<EventChoice> choices;
}
