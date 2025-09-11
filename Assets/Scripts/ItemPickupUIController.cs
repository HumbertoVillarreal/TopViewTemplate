using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemPickupUIController : MonoBehaviour
{

    public static ItemPickupUIController Instance { get; private set; }
    public GameObject popUpPrefab;
    public int maxPopups = 7;
    public float popupDuration = 3f;

    private readonly Queue<GameObject> activePopups = new();

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("Multiple instances detected. Destroying the extra one.");
            Destroy(gameObject);
        }
    }


    public void ShowItemPickup(string itemName, Sprite itemIcon)
    {
        GameObject newPopup = Instantiate(popUpPrefab, transform);
        newPopup.GetComponentInChildren<TMP_Text>().text = itemName;

        Image itemImg = newPopup.transform.Find("ItemIcon")?.GetComponent<Image>();

        if (itemImg)
        {
            itemImg.sprite = itemIcon;
        }

        activePopups.Enqueue(newPopup);

        if(activePopups.Count > maxPopups)
        {
            Destroy(activePopups.Dequeue());
        }

        //Fadeout and destroy
        StartCoroutine(FadeOutAndDestroy(newPopup));
    }

    private IEnumerator FadeOutAndDestroy(GameObject popup) {
        yield return new WaitForSeconds(popupDuration);
        if (popup == null) { yield break; }

        CanvasGroup canvasGroup = popup.GetComponent<CanvasGroup>();

        for (float timePassed = 0f; timePassed < 1f; timePassed += Time.deltaTime) {
            if(popup == null) { yield break; }
            canvasGroup.alpha = 1f - timePassed;
            yield return null;
        }

        Destroy(popup);
    }
}
