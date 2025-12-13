using NUnit.Framework;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class InventoryMovement : MonoBehaviour
{
    //[SerializeField] private GameObject selector;
    [SerializeField] private GameObject invPage;
    //[SerializeField] private RectTransform selectorRect;
    [SerializeField] private Transform slotParent;
    [SerializeField] private int numColumns = 6;
    [SerializeField] private GameObject trashIcon;


    [SerializeField] private Transform hotbarSlotsParent;

    public Color selectedColor = Color.yellow;
    public Color unselectedColor;
    public Color selectorColor;

    private List<RectTransform> slots = new List<RectTransform>();
    private int selectedIndex = 0;
    private int inventorySlotCount;

    private bool hasSelection = false;
    private Slot ogSlot;
    private Slot dropSlot;
    private bool inTrash = false;

    Vector3 originalScale;

    private void Start()
    {
        UnityEngine.ColorUtility.TryParseHtmlString("#8C8989", out unselectedColor);
        UnityEngine.ColorUtility.TryParseHtmlString("#E0E0E0", out selectorColor);
    }

    private void OnDisable()
    {
        // When menu opens, refresh the slot list
        RefreshSlots();
        ResetSelector();
        trashIcon.SetActive(false);
        hasSelection = false;
    }

    private void OnEnable()
    {
        // When menu opens, refresh the slot list
        RefreshSlots();
        ResetSelector();
    }


    private void RefreshSlots()
    {
        slots.Clear();  // prevents duplicates

        //Inv slots
        foreach (Transform child in slotParent)
        {
            if (child.GetComponent<Slot>() != null)
            {
                slots.Add(child.GetComponent<RectTransform>());
            }
        }

        //Count how many inv slots
        inventorySlotCount = slots.Count;

        //Hotbar slots
        foreach (Transform child in hotbarSlotsParent)
        {
            if (child.GetComponent<Slot>() != null)
            {
                slots.Add(child.GetComponent<RectTransform>());
            }
        }


        if (slots.Count > 0)
        {
            //selector.SetActive(true);
            //MoveSelectorTo(0, selectedIndex);
            selectedIndex = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {

        //if (!selector.activeInHierarchy || slots.Count == 0) { return; }
        if (!invPage.activeInHierarchy || slots.Count == 0) { return; }


        //Up
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveVertical(-1);
        }

        //Down
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveVertical(1);
        }


        //Left
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveHorizontal(-1);
        }

        //Right
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveHorizontal(1);
        }

        //X -> Trash
        if (Input.GetKeyDown(KeyCode.X) && hasSelection)
        {
            MoveSelectorToTrash(selectedIndex);
        }

        //Quick Assign
        if (Input.anyKeyDown)
        {
            HandleQuckAssign();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
                TrySelectOrSwap();
        }

    }


    private void MoveHorizontal(int direction)
    {

        int rowStart = (selectedIndex / numColumns) * numColumns;
        int rowEnd = rowStart + numColumns -1;


        if (direction == -1 && selectedIndex == 12 && hasSelection)
        {
            MoveSelectorToTrash(selectedIndex);
            return;
        }

        if (inTrash)
        {
            if (direction == 1)    // Move RIGHT out of trash
            {
                inTrash = false;
                trashIcon.GetComponent<UnityEngine.UI.Image>().color = Color.white;

                int returnIndex = 12; // first slot in third row
                MoveSelectorTo(returnIndex, selectedIndex);
            }
            return; // stop normal slot movement
        }


        int next = selectedIndex + direction;
        next = Mathf.Clamp(next, rowStart, rowEnd);
        next = Mathf.Clamp(next, 0, slots.Count -1);

        MoveSelectorTo(next, selectedIndex);

    }


    private void MoveVertical(int direction)
    {

        if (direction == 1 && selectedIndex == 6 && hasSelection)
        {
            MoveSelectorToTrash(selectedIndex);
            return;
        }

        if (inTrash)
        {
            if (direction == -1)    // Move UP out of trash
            {
                inTrash = false;
                trashIcon.GetComponent<UnityEngine.UI.Image>().color = Color.white;

                int returnIndex = 6; // first slot in second row
                MoveSelectorTo(returnIndex, selectedIndex);
            }
            
            return; // stop normal slot movement
        }


        int next = selectedIndex + (direction * numColumns);
        next = Mathf.Clamp(next, 0, slots.Count -1);

        MoveSelectorTo(next, selectedIndex);
        //HandleParent(next);
    }


    private void MoveSelectorTo(int index, int prev)
    {

        if (index < 0 || index >= slots.Count)
            return;

        // Restore color ONLY if prev is NOT the selected slot
        if (!hasSelection || slots[prev].GetComponent<Slot>() != ogSlot)
        {
            slots[prev].GetComponent<Image>().color = unselectedColor;
        }

        selectedIndex = index;

        Slot newSlot = slots[selectedIndex].GetComponent<Slot>();

        // If cursor lands on the selected slot -> keep it yellow
        if (hasSelection && newSlot == ogSlot)
        {
            newSlot.GetComponent<Image>().color = selectedColor;
        }
        else
        {
            newSlot.GetComponent<Image>().color = selectorColor;
        }

        //selector.transform.position = slots[index].transform.position;
        //selectorRect.sizeDelta = slots[index].sizeDelta;
    }

    public void ResetSelector()
    {

        if (slots.Count == 0) { return; }

        foreach (Transform child in slots)
            {

            child.GetComponent<Slot>().GetComponent<Image>().color = unselectedColor;

            }

        selectedIndex = 0;
        slots[0].GetComponent<Slot>().GetComponent<Image>().color = selectorColor;
        //selector.transform.position= slots[0].transform.position;

    }

    public void HandleParent(int next)
    {

        if (next < inventorySlotCount)
        {
            //selector.transform.SetParent(slotParent, worldPositionStays:false);
            //selector.transform.SetAsLastSibling();
        }
        else
        {
            //selector.transform.SetParent(hotbarSlotsParent, worldPositionStays: false);
            //selector.transform.SetAsLastSibling();
        }

    }

    public void TrySelectOrSwap()
    {

        if (!hasSelection)
        {
            ogSlot = slots[selectedIndex].GetComponent<Slot>();
            trashIcon.SetActive(true);
            ogSlot.GetComponent<Image>().color = selectedColor;
            hasSelection = true;
        }
        else
        {
            dropSlot = slots[selectedIndex].GetComponent<Slot>();

            if (!inTrash)
                SwapItems(ogSlot, dropSlot);
            else
                DropTrash(ogSlot);

         
        }

    }


    public void SwapItems(Slot ogSlot, Slot dropSlot)
    {

        GameObject ogItem = ogSlot.currentItem;
        GameObject dropItem = dropSlot.currentItem;

        // Swap position in UI
        if (ogItem != null)
        {
            ogItem.transform.SetParent(dropSlot.transform);
            ogItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

            if (dropSlot.CompareTag("SmallSlot"))
                ogItem.transform.localScale = Vector3.one * 0.7f;
            else
                ogItem.transform.localScale = Vector3.one;
        }

        if (dropItem != null)
        {
            dropItem.transform.SetParent(ogSlot.transform);
            dropItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

            if (ogSlot.CompareTag("SmallSlot"))
                dropItem.transform.localScale = Vector3.one * 0.7f;
            else
                dropItem.transform.localScale = Vector3.one;
        }

        // Swap item refrence
        dropSlot.currentItem = ogItem;
        ogSlot.currentItem = dropItem;
        trashIcon.SetActive(false);

        hasSelection = false;
        ogSlot.GetComponent<Image>().color = unselectedColor;
        ogSlot = null;
        dropSlot = null;
    }

    private void MoveSelectorToTrash(int prev)
    {
        inTrash = true;
        trashIcon.GetComponent<Image>().color = Color.red;
        slots[prev].GetComponent<Image>().color = unselectedColor;

    }


    private void DropTrash(Slot slot)
    {


        if (slot.currentItem == null)
        {

            inTrash = false;
            trashIcon.SetActive(false);
            ResetSelector();
            return;
        }



        ItemDragHandler drag = slot.currentItem.GetComponent<ItemDragHandler>();
        drag.DropItem(slot);

        trashIcon.SetActive(false);
        inTrash = false;
        trashIcon.GetComponent<UnityEngine.UI.Image>().color = Color.white;

        ResetSelector();

    }


    //Quick assign to hotbar -> Check wich slot
    private void HandleQuckAssign()
    {
        for (int i = 0; i < 6; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                MoveSelectedItemToHotbar(i);
                break;
            }
        }
    }


    //Quicj assign to hotbar
    private void MoveSelectedItemToHotbar(int index)
    {
        index += 12;
        dropSlot = slots[index].GetComponent<Slot>();
        SwapItems(ogSlot, dropSlot);
        ogSlot.GetComponent<Image>().color = selectorColor;
    }
}