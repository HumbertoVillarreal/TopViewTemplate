using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{

    Transform originalParent;
    CanvasGroup canvasGroup;

    [SerializeField] public float minDropDistance = .5f;
    [SerializeField] public float maxDropDistance = 1f;

    Vector3 originalScale;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;

        // check if current parent is a SmallSlot
        if (originalParent.CompareTag("SmallSlot"))
        {
            // restore to normal scale before saving
            originalScale = transform.localScale / 0.7f;
        }
        else
        {
            originalScale = transform.localScale;
        }

        transform.SetParent(transform.root);
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.6f; //semi transparent during drag
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true; //Enavles raycasts
        canvasGroup.alpha = 1f; //No longer transparent

        Slot dropSlot = eventData.pointerEnter?.GetComponent<Slot>(); //Slot where item dropped

        if (dropSlot == null)
        {
            GameObject dropItem = eventData.pointerEnter;
            if (dropItem != null) {
                dropSlot = dropItem.GetComponentInParent<Slot>();
            }
        }

        Slot originalSlot = originalParent.GetComponent<Slot>();

        if (dropSlot != null) {
            if (dropSlot.currentItem != null) {
                //Slot has an item -> swap items
                dropSlot.currentItem.transform.SetParent(originalSlot.transform);
                originalSlot.currentItem = dropSlot.currentItem;
                dropSlot.currentItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            }
            else
            {
                originalSlot.currentItem = null;
            }

            //Move item into drop slot
            transform.SetParent(dropSlot.transform); ;
            dropSlot.currentItem = gameObject;

            if (dropSlot.CompareTag("SmallSlot")) 
            {
                transform.localScale = originalScale * 0.7f;
            }
            else
            {
                transform.localScale = originalScale; // restore normal size
            }
        }
        else
        {

            //If we are dropping outside of inventory
            if (!IsWithinInventory(eventData.position)) {

                //Drop Item
                transform.localScale = originalScale;
                DropItem(originalSlot);
            }
            else
            {
                //Return to og slot
                transform.SetParent(originalParent);
                transform.localScale = originalScale;
            }

                
        }
        GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }


    bool IsWithinInventory(Vector2 mousePosition)
    {
        RectTransform inventoryRect = originalParent.parent.GetComponent<RectTransform>();
        return RectTransformUtility.RectangleContainsScreenPoint(inventoryRect, mousePosition);
    }


    void DropItem(Slot originalSlot)
    {
        originalSlot.currentItem = null;

        //Find Player
        Transform playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (playerTransform == null) {
            Debug.LogError("Missing Player Tag");
            return;
        }

        //Randrom drop position
        Vector2 dropOffSet = Random.insideUnitCircle * Random.Range(minDropDistance, maxDropDistance);
        Vector2 dropPosition = (Vector2)playerTransform.position + dropOffSet;

        //Instatiate drop item
        GameObject dropItem = Instantiate(gameObject, dropPosition, Quaternion.identity);
        dropItem.GetComponent<BounceEffect>().StartBounce();

        //D3estroy the UI item
        Destroy(gameObject);
    }

}
