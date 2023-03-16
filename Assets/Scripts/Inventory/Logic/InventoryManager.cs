using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : Singleton<InventoryManager>
{
    public ItemDataList_SO itemData;
    public List<ItemName> itemList = new List<ItemName>();
    public List<Sprite> itemImages;
    public bool holdItem;
    public ItemName currentItem;
    public void AddItem(ItemName itemName)
    {
        if (!itemList.Contains(itemName))
        {
            itemList.Add(itemName);
            itemImages.Add(itemData.itemDetailsList[ itemData.itemDetailsList.FindIndex((ItemDetails name) => name.itemName == itemName)].itemSprite);
        }
    }

    private void Start()
    {
        foreach(var item in itemList)
        {
            Sprite sprite = itemData.itemDetailsList[itemData.itemDetailsList.FindIndex((ItemDetails name) => name.itemName == item)].itemSprite;
            if (!itemImages.Contains(sprite))
            itemImages.Add(sprite);
        }
    }

    public GameObject inventoryUI;
    void OpenUI()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (!inventoryUI.activeInHierarchy)
                EventHandler.CallOpenInventoryEvent();
        }
    }

    private void Update()
    {
        OpenUI();
    }

    
}
