using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, I_Interactable
{

    public bool IsOpened {  get; private set; }
    public string ChestID {  get; private set; }
    public GameObject itemPrefab; //Item in the chest
    public Sprite openedSprite;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ChestID ??= GlobalHelper.GenerateUniqueID(gameObject);
    }


    public bool CanInteract()
    {
        return !IsOpened;
    }

    public void Interact()
    {
        if(!CanInteract()) return;

        OpenChest();
    }

    private void OpenChest()
    {
        //Set chest as opened
        SetOpened(true);
        SoundEffectManager.Play("OpenChest");

        //Drop Item
        if (itemPrefab)
        {
            GameObject droppedItem = Instantiate(itemPrefab, transform.position + Vector3.down, Quaternion.identity);

            Collider2D itemCollider = droppedItem.GetComponent<Collider2D>();
            if (itemCollider != null)
            {
                itemCollider.enabled = false;
                // Reactivar después de 0.5 segundos
                droppedItem.GetComponent<MonoBehaviour>().StartCoroutine(EnableColliderAfterDelay(itemCollider, 0.5f));
            }

            droppedItem.GetComponent<BounceEffect>().StartBounce();
        }
    }

    private IEnumerator EnableColliderAfterDelay(Collider2D collider, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (collider != null)
            collider.enabled = true;
    }


    public void SetOpened(bool opened)
    {
        if(IsOpened = opened)
        {
            GetComponent<SpriteRenderer>().sprite = openedSprite;
        }
    }

}
