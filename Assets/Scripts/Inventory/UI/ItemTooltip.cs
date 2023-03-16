using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemTooltip : MonoBehaviour
{
    public Text itemDetiles;

    public void UpdateItemName(ItemName itemName)
    {
        itemDetiles.text = itemName switch
        {
            _ => ""
        };
    }
}
