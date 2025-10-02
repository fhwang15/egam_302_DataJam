using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int currentResource1;
    public int currentResource2;
    public int currentResource3;

    public int maxResource1;
    public int maxResource2;
    public int maxResource3;

    public Node currentNode;

    public PlayerData(int maxRes1, int maxRes2, int maxRes3)
    {
        this.maxResource1 = maxRes1;
        this.maxResource2 = maxRes2;
        this.maxResource3 = maxRes3;
        this.currentResource1 = maxRes1;
        this.currentResource2 = maxRes2;
        this.currentResource3 = maxRes3;
    }

    public void ApplyEffect(EventChoice choice)
    {
        currentResource1 += choice.resourceChange1;
        currentResource2 += choice.resourceChange2;
        currentResource3 += choice.resourceChange3;
    }


}
