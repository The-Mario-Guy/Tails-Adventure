using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    private TextMeshProUGUI ringText;

    // Start is called before the first frame update
    void Start()
    {
        ringText = GetComponent<TextMeshProUGUI>();
    }

    public void UpdateDiamondText(PlayerInventory playerInventory)
    {
        ringText.text = playerInventory.NumberOfDiamonds.ToString();
    }
}

