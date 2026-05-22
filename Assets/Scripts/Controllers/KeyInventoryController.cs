using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KeyInventoryController : MonoBehaviour
{
    [Header("Stat Text")]
    [SerializeField] TextMeshProUGUI AtkText;
    [SerializeField] TextMeshProUGUI DefText;
    [SerializeField] TextMeshProUGUI SpdText;
    [SerializeField] TextMeshProUGUI LckText;
    [SerializeField] TextMeshProUGUI MaxHpText;
    [SerializeField] TextMeshProUGUI CurrHpText;

    [Header("Key Inventory")]
    [SerializeField] Transform keyItemsPanel;
    [SerializeField] GameObject keySlotPrefab;
    [SerializeField] GameObject keyItemUIPrefab;
    [SerializeField] int keySloyCount;
    [SerializeField] private KeyItemDictionary keyItemDictionary;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateKeySlots();
    }

    // Update is called once per frame
    void Update()
    {
        SetStatsUIValues();

    }


    private void GenerateKeySlots()
    {
        //Clean grid
        foreach (Transform child in keyItemsPanel)
        {
            Destroy(child);
        }

        //Create slots
        for (int i = 0; i < keySloyCount; i++)
        {
            Instantiate(keySlotPrefab, keyItemsPanel);
        }
    }
     

    public bool AddKeyItem(KeyItem itemData)
    {
        foreach (Transform keySlot in keyItemsPanel)
        {
            Slot slot = keySlot.GetComponent<Slot>();

            if (slot != null && slot.currentItem == null)
            {
                GameObject newItem = Instantiate(keyItemUIPrefab, keySlot);
                newItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

                KeyItemUI ui = newItem.GetComponent<KeyItemUI>();

                ui.Setup(itemData);

                slot.currentItem = newItem;
                return true;
            }
            }
        
        return false;
    }


    private void SetStatsUIValues()
    {
        AtkText.text = Player.Instance.getBaseAttack().ToString();
        DefText.text = Player.Instance.getBaseDefense().ToString();
        SpdText.text = Player.Instance.getBaseSpeed().ToString();
        LckText.text = Player.Instance.getBaseLuck().ToString();
        MaxHpText.text = Player.Instance.getMaxHp().ToString();
        CurrHpText.text = Player.Instance.getCurrHp().ToString();
    }


    public List<KeyInventorySaveData> GetKeyItems()
    {
        List<KeyInventorySaveData> data = new List<KeyInventorySaveData>();

        foreach (Transform slotTransform in keyItemsPanel)
        {
            Slot slot = slotTransform.GetComponent<Slot>();

            if (slot == null) { continue; }

            if (slot.currentItem != null)
            {
                KeyItemUI ui = slot.currentItem.GetComponent<KeyItemUI>();

                data.Add(new KeyInventorySaveData
                {
                    itemID = ui.itemID,
                    slotIndex = slotTransform.GetSiblingIndex()
                });
            }
        }

        return data; 

    }


    public void SetKeyItems(List<KeyInventorySaveData> saveData)
    {
        //Clear data
        foreach (Transform child in keyItemsPanel)
        {
            Destroy(child.gameObject);
        }

        //Recreate slots
        for (int i = 0; i < keySloyCount; i++)
        {
            Instantiate(keySlotPrefab, keyItemsPanel);
        }

        //Populate key items
        foreach (var data in saveData)
        {
            if (data.slotIndex >= keySloyCount) { continue; }

            Slot slot = keyItemsPanel.GetChild(data.slotIndex).GetComponent<Slot>();

            KeyItem item = keyItemDictionary.GetItem(data.itemID);

            if (item != null)
            {
                GameObject uiItem = Instantiate(keySlotPrefab, slot.transform);

                KeyItemUI ui = uiItem.GetComponent<KeyItemUI>();
                ui.Setup(item);

                slot.currentItem = uiItem;
            }
        }

    }


    public void AddKeyItemByID(string id, int slotIndex)
    {
        KeyItem item = keyItemDictionary.GetItem(id);

        if (item == null) { return; }

        Slot slot = keyItemsPanel.GetChild(slotIndex).GetComponent<Slot>();

        GameObject uiItem = Instantiate(keySlotPrefab, slot.transform);

        KeyItemUI ui = uiItem.GetComponent<KeyItemUI>();
        ui.Setup(item);

        slot.currentItem = uiItem;
    }

}
