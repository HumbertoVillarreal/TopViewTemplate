using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TabController : MonoBehaviour
{
    public Image[] tabImages;
    public GameObject[] pages;
    public GameObject menuCanvas;
    public int activeTab = 0;
    //public GameObject selector;
    [SerializeField] public int invTabIndex = 1;

    public InventoryMovement inventoryMovement;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ActivateTab(activeTab);
    }

    private void Update()
    {

        if (menuCanvas != null && menuCanvas.activeSelf) {

            if (Input.GetKeyDown(KeyCode.Q)) { PreviousTab(); }

            if (Input.GetKeyDown(KeyCode.E)) { NextTab(); }

        }

    }


    public void ActivateTab(int tabNo)
    {
        for (int i = 0; i < tabImages.Length; i++) {
            pages[i].SetActive(false);
            tabImages[i].color = Color.grey;
        }

        pages[tabNo].SetActive(true);
        tabImages[tabNo].color = Color.white;

        if (tabNo == invTabIndex) { 
            //selector.SetActive(true);
            StartCoroutine(DelayedReset());

        }
        //else { selector.SetActive(false); }

            activeTab = tabNo;

    }

    public void NextTab()
    {
        int next = (activeTab + 1) % tabImages.Length;
        ActivateTab(next);
    }

    public void PreviousTab()
    {
        int prev = (activeTab - 1 + tabImages.Length) % tabImages.Length;
        ActivateTab(prev);
    }

    public IEnumerator DelayedReset()
    {
        yield return null;
        inventoryMovement.ResetSelector();
    }
}
