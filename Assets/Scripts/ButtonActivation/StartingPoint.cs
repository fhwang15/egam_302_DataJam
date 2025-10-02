using UnityEngine;

public class StartingPoint : MonoBehaviour
{

    public GameObject initialDialogue;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initialDialogue.SetActive(false);
    }

    public void OnClick()
    {
        initialDialogue.SetActive(true);
        //generate new nodes
    }
}
