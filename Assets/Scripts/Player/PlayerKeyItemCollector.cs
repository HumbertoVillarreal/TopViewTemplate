using UnityEngine;

public class PlayerKeyItemCollector : MonoBehaviour
{

    private KeyInventoryController keyInventory;


    private void Start()
    {
        keyInventory = FindObjectOfType<KeyInventoryController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("KeyItem"))
        {
            KeyItemPickup pickup = collision.GetComponent<KeyItemPickup>();

            if (pickup == null) return;

            if (pickup.IsPickedUp) return;

            pickup.IsPickedUp = true;

            bool added = keyInventory.AddKeyItem(pickup.keyItemData);

            if (added)
            {
                SoundEffectManager.Play("Pickup");
                pickup.Pickup();
                Destroy(collision.gameObject);
            }
            else
            {
                pickup.IsPickedUp = false;
            }
        }
    }

}
