using UnityEngine;

public class PlayerItemCollector : MonoBehaviour
{

    private InventoryController inventoryController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inventoryController = FindObjectOfType<InventoryController>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            
            Item item = collision.GetComponent<Item>();

            if (item == null) { return; }

            if (item.IsPickedUp) return;

            item.IsPickedUp = true;

            bool itemAdded = inventoryController.AddItem(collision.gameObject);


            if (itemAdded)
            {
                SoundEffectManager.Play("Pickup"); //Pickup sfx
                item.transform.localScale = Vector3.one;

                // Disable collider just to be extra safe
                Collider2D itemCollider = collision.GetComponent<Collider2D>();
                if (itemCollider != null)
                    itemCollider.enabled = false;

                item.Pickup();
                Destroy(collision.gameObject, 0.05f);
            }
            else
            {
                // If not added, unmark it
                item.IsPickedUp = false;
            }
        }
    }
}
