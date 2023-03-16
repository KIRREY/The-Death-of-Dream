using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Button currentItem;
    public List<Sprite> itemSpriteList;
    public List<ItemName> itemNames;
    public ItemTooltip itemTooltip;
    public InventoryManager inventoryManager;

    public GameObject inventoryUI;
    public GameObject showItem;

    private void Start()
    {
        inventoryManager = InventoryManager.Instance;
    }

    /*public List<Vector2> pos;
    void SetTransform()
    {
        pos?.Clear();
        foreach (var transform in itemPos)
        {
            Debug.Log(transform.position);
            pos.Add(new Vector2(transform.anchoredPosition.x, transform.anchoredPosition.y));
        }
    }*/

    public List<RectTransform> itemPos;
    private void OnEnable()
    {
        EventHandler.OpenInventoryEvent += OnOpenInventoryEvent;
        EventHandler.CloseInventoryEvent += OnCloseInventoryEven;
    }

    private void OnDisable()
    {
        EventHandler.OpenInventoryEvent -= OnOpenInventoryEvent;
        EventHandler.CloseInventoryEvent -= OnCloseInventoryEven;
    }

    Coroutine select;

    private void OnCloseInventoryEven()
    {
        EventHandler.CallGameStateChangerEvent(GameState.GamePlay);
        StopAllCoroutines();
        inventoryUI.SetActive(false);
        showItem.SetActive(true);
    }

    private void OnOpenInventoryEvent()
    {
        EventHandler.CallGameStateChangerEvent(GameState.Pause);
        //SetTransform();
        itemSpriteList?.Clear();
        itemNames?.Clear();
        inventoryUI.SetActive(true);
        showItem.SetActive(false);
        if (select != null)
            select = null;
        select = StartCoroutine(SelectItem());
    }

    public List<Image> itemImageList;
    Coroutine Onplaying;
    IEnumerator SelectItem()
    {
        if (inventoryManager.currentItem == ItemName.None)
        {
            for (int i = 0; i < inventoryManager.itemList.Count; i++)
            {
                itemSpriteList.Add(inventoryManager.itemImages[i]);
                itemNames.Add(inventoryManager.itemList[i]);
            }
        }
        else
        {
            int index = inventoryManager.itemList.FindIndex((ItemName name) => name == inventoryManager.currentItem);
            for (int i = index; i < index + inventoryManager.itemList.Count; i++)
            {
                Debug.Log(inventoryManager.itemList.FindIndex((ItemName name) => name == inventoryManager.currentItem));
                itemSpriteList.Add(inventoryManager.itemImages[i > inventoryManager.itemList.Count ? i - 1 - inventoryManager.itemList.Count : i]);
                itemNames.Add(inventoryManager.itemList[i > inventoryManager.itemList.Count ? i - 1 - inventoryManager.itemList.Count : i]);
            }
        }
        if (itemSpriteList.Count == 0)
        {
            select = null;
            yield break;
        }

        ResetListIndex();
        yield return new WaitForSeconds(0.5f);
        while (true)
        {
            try
            {
                if (Onplaying != null)
                    continue;
                if (Input.GetKeyDown(KeyCode.D))
                {
                    Onplaying = StartCoroutine(UpdateUI(1));
                }
                if (Input.GetKeyDown(KeyCode.A))
                {
                    Onplaying = StartCoroutine(UpdateUI(-1));
                }
                if (Input.GetKeyDown(KeyCode.B))
                    EventHandler.CallCloseInventoryEvent();
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    try
                    {
                        inventoryManager.currentItem = inventoryManager.itemData.itemDetailsList[inventoryManager.itemData.itemDetailsList.FindIndex((ItemDetails name) => name.itemSprite == itemImageList[2].sprite)].itemName;
                        showItem.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = itemImageList[2].sprite;
                        inventoryManager.holdItem = true;
                    }
                    catch
                    {
                        inventoryManager.currentItem = ItemName.None;
                        showItem.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = null;
                        inventoryManager.holdItem = false;
                    }
                    EventHandler.CallCloseInventoryEvent();
                }
            }
            catch { }
            yield return null;
        }
    }

    bool firstRet;
    void ResetListIndex()
    {
        if (itemSpriteList.Count < itemImageList.Count )
        {
            if(firstRet)
                return;
            for (int j = 0; j < itemSpriteList.Count; j++)
            {
                itemImageList[j].sprite = itemSpriteList[j];
            }
            firstRet = true;
        }
        else
        {
            for (int j = 0; j < Mathf.Min(itemSpriteList.Count, itemImageList.Count); j++)
            {
                itemImageList[j].sprite = itemSpriteList[j];
            }
            for (int j = Mathf.Min(itemSpriteList.Count, itemImageList.Count); j < Mathf.Max(itemSpriteList.Count, itemImageList.Count); j++)
            {
                itemImageList[j].sprite = null;
            }
        }
    }

    public float keepTime;
    IEnumerator Move(int i, float speed, float speedA, int target)
    {
        while (itemImageList[i].transform.position.x >itemPos[target].anchoredPosition.x)
        {
            itemImageList[i].transform.position = new Vector2(itemImageList[i].transform.position.x + speed * Time.deltaTime, itemImageList[i].transform.position.y);
            Color color = itemImageList[i].color;
            if (i >2)
                color.a += speedA* Time.deltaTime;
            else
                color.a -= speedA * Time.deltaTime;
            itemImageList[i].color = color;
            yield return new WaitForEndOfFrame();
        }
        if(i==4)
        {
            for (int j = 0; j < itemImageList.Count - 1; j++)
            {
                itemImageList[j] = itemImageList[j + 1];
            }
            itemImageList[^1] = changeItemImage; 
            ResetListIndex();
            for (int k = 0; k < itemImageList.Count; k++)
            {
                itemImageList[k].transform.position = itemPos[k].position;
            }
            Onplaying = null;
        }
    }

    Image changeItemImage;
    IEnumerator UpdateUI(int change)
    {
        if (change > 0)
        {
            var image = itemSpriteList[0];
            var name = itemNames[0];
            for (int i = 0; i < itemSpriteList.Count - 1; i++)
            {
                itemSpriteList[i] = itemSpriteList[i + 1];
                itemNames[i] = itemNames[i + 1];
            }
            itemSpriteList[^1] = image;
            itemNames[^1] = name;
        }
        else
        {
            var image = itemSpriteList[^1];
            var name = itemNames[^1];
            for (int i = 1; i < itemSpriteList.Count; i++)
            {
                itemSpriteList[i] = itemSpriteList[i - 1];
                itemNames[i] = itemNames[i - 1];
            }
            itemSpriteList[0] = image;
            itemNames[0] = name;
        }

        /*int left = 0;
        int right = left;
        for (int j = 1; j < itemImageList.Count; j++)
        {
            left = itemImageList[left].transform.position.x > itemImageList[j].transform.position.x ? j : left;
            right = itemImageList[right].transform.position.x > itemImageList[j].transform.position.x ? right : j;
        }*/
        if (change > 0)
        {
            changeItemImage = itemImageList[0];
            //itemImageList[^1].transform.position = itemPos[0].anchoredPosition;
            itemImageList[0].transform.position = itemPos[^1].anchoredPosition;
            float speed = ( itemImageList[^1].transform.position.x-itemPos[^2].anchoredPosition.x) / keepTime;
            Debug.Log(speed);
            float speedA = 0.5f / keepTime;
            for (int i = 1; i < itemImageList.Count ; i++)
            {
                StartCoroutine(Move(i, -speed, speedA, i - change));
            }
            /*while (!Mathf.Approximately( itemImageList[^1].transform.position.x , itemPos[^2].anchoredPosition.x))
            {
                yield return new WaitForEndOfFrame();
            }*/
        }
        else
        {
            changeItemImage = itemImageList[^1];
            itemImageList[0].transform.position = itemPos[^1].anchoredPosition;
            itemImageList[^1].transform.position = itemPos[0].anchoredPosition;
            float speed = (itemImageList[0].transform.position.x - itemPos[^2].anchoredPosition.x) / keepTime;
            float speedA = 0.5f / keepTime;
            while (itemImageList[0].transform.position.x > itemPos[^2].anchoredPosition.x)
            {
                for (int i = 0; i < itemImageList.Count - 1; i++)
                {
                    itemImageList[i].transform.position = new Vector2(itemImageList[i].transform.position.x - speed * Time.fixedDeltaTime, itemImageList[i].transform.position.y);
                    Color color = itemImageList[i].color;
                    if (i < itemImageList.Count / 2)
                        color.a += speedA;
                    else
                        color.a -= speedA;
                    itemImageList[i].color = color;
                }
                yield return new WaitForFixedUpdate();
            }
            for (int i = 1; i < itemImageList.Count; i++)
            {
                itemImageList[i] = itemImageList[i - 1];
            }
            itemImageList[0] = changeItemImage;
        }

        
    }

}
