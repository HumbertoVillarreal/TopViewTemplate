using TMPro;
using UnityEngine;

public class ShopSlot : MonoBehaviour
{
    public GameObject currItem;
    public int itemPrice;
    public TMP_Text priceText;
    public bool isShopSlot = true;

    private void Awake()
    {
        if (!priceText)
        {
            priceText = transform.Find("PriceText").GetComponent<TMP_Text>();
        }
    }


    public void UpdatePriceDisplay()
    {
        if (priceText && currItem)
        {
            priceText.text = itemPrice.ToString();
        }
    }


    public void SetItem(GameObject item, int price)
    {
        currItem = item;
        itemPrice = price;
        UpdatePriceDisplay();
    }
}
