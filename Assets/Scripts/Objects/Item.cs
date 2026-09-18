using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{

    public int ID;
    public string Name;
    public bool IsPickedUp = false;
    [SerializeField] private float dropRate;


    //Shop fields
    public int buyPrice = 10;
    [Range(0, 1)]
    public float sellPriceMultiplier = 0.5f;


    public int GetSellPrice()
    {
        return Mathf.RoundToInt(buyPrice * sellPriceMultiplier);
    }


    public virtual void Pickup()
    {
        Sprite itemIcon = GetComponent<Image>().sprite;
        if(ItemPickupUIController.Instance != null)
        {
            ItemPickupUIController.Instance.ShowItemPickup(Name, itemIcon);
        }
    }

    public virtual void UseItem()
    {
        Debug.Log($"Using item: {Name}");
    }


    public float GetDropRate()
    {
        return dropRate;
    }
}
