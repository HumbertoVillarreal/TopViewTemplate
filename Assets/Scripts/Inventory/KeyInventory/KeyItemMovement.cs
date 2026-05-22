using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class KeyItemMovement : MonoBehaviour
{
    [SerializeField] private Transform keySlotParent;
    [SerializeField] private Transform actionSlotsParent;
    [SerializeField] int numColumns = 3;

    public Color selectedColor = Color.yellow;
    public Color unselectedColor;
    public Color selectorColor;

    private List<Slot> keySlots = new List<Slot>();
    private List<Slot> actionSlots = new List<Slot>();

    [SerializeField] private Image leftItemImage;
    [SerializeField] private Image rightItemImage;

    private int selectedIndex = 0;


    private bool waitingForAssignment = false;


    private bool hasSelection = false;
    private Slot ogSlot;
    private Slot selectedEquipSLot;

    private void OnEnable()
    {
        Invoke(nameof(Init), 0.01f);
    }

    private void OnDisable()
    {

    }

    void Init()
    {
        RefreshSlots();

        if (keySlots.Count > 0)
            Select(0);
    }


    private void Start()
    {
        UnityEngine.ColorUtility.TryParseHtmlString("#8C8989", out unselectedColor);
        UnityEngine.ColorUtility.TryParseHtmlString("#E0E0E0", out selectorColor);
    }


    // Update is called once per frame
    void Update()
    {
        if (!waitingForAssignment)
        {
            if (Input.GetKeyDown(KeyCode.W)) MoveVertical(-1);
            if (Input.GetKeyDown(KeyCode.A)) MoveHorizontal(-1);
            if (Input.GetKeyDown(KeyCode.S)) MoveVertical(1);
            if (Input.GetKeyDown(KeyCode.D)) MoveHorizontal(1);

        }

        //Empieza seleccion de item
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartItemSelection();
        }

        //Espera a que se asigne a que boton de mouse
        if (waitingForAssignment)
        {
            if (Input.GetMouseButtonDown(0))
            {
                AssignToActionSlot(0);
            }

            else if (Input.GetMouseButtonDown(1))
            {
                AssignToActionSlot(1);
            }
        }
    }


    void RefreshSlots()
    {
        keySlots.Clear();
        actionSlots.Clear();
            
        foreach (Transform t in keySlotParent)
        {
            Slot slot = t.GetComponent<Slot>();
            if (slot != null)
            {
                slot.GetComponent<Image>().color = unselectedColor;
                keySlots.Add(slot);
            }
                
        }

        foreach (Transform t in actionSlotsParent)
        {
            Slot slot = t.GetComponent<Slot>();
            if (slot != null)
                actionSlots.Add(slot);
        }
        }


    private void Move(int dir)
    {
        selectedIndex = Mathf.Clamp(selectedIndex + dir, 0, keySlots.Count - 1);
        Select(selectedIndex);
    }


    private void Select(int index)
    {
        if (keySlots.Count == 0) return;

        if (index < 0 || index >= keySlots.Count) return;

        // Limpia anterior
        if (keySlots[selectedIndex] != null)
        {
            keySlots[selectedIndex]
                .GetComponent<Image>().color = unselectedColor;
        }

        selectedIndex = index;

        // Nuevo seleccionado
        if (keySlots[selectedIndex] != null)
        {
            keySlots[selectedIndex]
                .GetComponent<Image>().color = selectorColor;
        }
    }


    private void AssignToActionSlot(int btn)
    {
        if (!waitingForAssignment)
            return;


        GameObject itemObj = selectedEquipSLot.currentItem;

        KeyItemUI ui = itemObj.GetComponent<KeyItemUI>();

        if (ui == null)
            return;

        // LEFT CLICK
        if (btn == 0)
        {
            rightItemImage.sprite = ui.itemData.icon;

            Player.Instance.SetLeftItem(ui.itemData);
        }
        // RIGHT CLICK
        else
        {
            leftItemImage.sprite = ui.itemData.icon;

            Player.Instance.SetRightItem(ui.itemData);
        }

        // RESET STATES
        selectedEquipSLot.GetComponent<Image>().color = selectorColor;

        hasSelection = false;
        waitingForAssignment = false;
        selectedEquipSLot = null;

    }


    private void MoveHorizontal(int direction)
    {

        int rowStart = (selectedIndex / numColumns) * numColumns;
        int rowEnd = rowStart + numColumns - 1;


        int next = selectedIndex + direction;
        next = Mathf.Clamp(next, rowStart, rowEnd);
        next = Mathf.Clamp(next, 0, keySlots.Count - 1);

        MoveSelectorTo(next, selectedIndex);

    }


    private void MoveVertical(int direction)
    {
        int next = selectedIndex + (direction * numColumns);
        next = Mathf.Clamp(next, 0, keySlots.Count - 1);

        MoveSelectorTo(next, selectedIndex);
        //HandleParent(next);
    }


    private void MoveSelectorTo(int index, int prev)
    {
        SoundEffectManager.Play("InvMovement");
        if (index < 0 || index >= keySlots.Count)
            return;
        
        // Restore color ONLY if prev is NOT the selected slot
        if (!hasSelection || keySlots[prev].GetComponent<Slot>() != ogSlot)
        {
            keySlots[prev].GetComponent<Image>().color = unselectedColor;
        }
        

        selectedIndex = index;

        Slot newSlot = keySlots[selectedIndex].GetComponent<Slot>();

        newSlot.GetComponent<Image>().color = selectedColor;
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


    private void StartItemSelection()
    {
        SoundEffectManager.Play("ItemSelect");
        Slot slot = keySlots[selectedIndex];

        if (hasSelection) {
            slot.GetComponent<Image>().color = selectorColor;
            hasSelection = false;
            waitingForAssignment = false;
            return;
        }


        selectedEquipSLot = slot;

        if (slot.currentItem == null) return;

        hasSelection = true;
        waitingForAssignment = true;

        ogSlot = slot;
        selectedEquipSLot = slot;

        slot.GetComponent<Image>().color = selectedColor;
    }

}
