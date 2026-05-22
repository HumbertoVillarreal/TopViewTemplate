using UnityEngine;

public class KeyItemPickup : MonoBehaviour
{

    public KeyItem keyItemData;
    public bool IsPickedUp = false;

    public void Pickup()
    {
        if (keyItemData == null) return;

        if (ItemPickupUIController.Instance != null)
        {
            ItemPickupUIController.Instance.ShowItemPickup(
                keyItemData.itemName,
                keyItemData.icon
                );
        }
    }

}
