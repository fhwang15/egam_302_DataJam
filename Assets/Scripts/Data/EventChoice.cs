using System.Collections.Generic;
using UnityEngine;

//Shown in the inspector
[System.Serializable]
public class EventChoice
{
    public string choiceText;
    public string resultText;

    public int resourceChange1;
    public int resourceChange2;
    public int resourceChange3;

    public List<string> nextEventTypes;
    public int nextNodeCount = 3;

}
