using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OpeningInventories : MonoBehaviour
{
    public Button Inventory;
    public Button Closing_Inventory;
    public GameObject Inventory_Panel;

    void Start()
    {
        Inventory_Panel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnInventoryPressed()
    {
        //Coroutine Animation
        Inventory_Panel.SetActive(true);
    }

    public void OnXButtonPressed()
    {
        if(Closing_Inventory.onClick != null)
        {
            //Coroutine Animation
            Inventory_Panel.SetActive(false);
        }
    }

}
