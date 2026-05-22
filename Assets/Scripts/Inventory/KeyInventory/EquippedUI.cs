using UnityEngine;
using UnityEngine.UI;

public class EquippedUI : MonoBehaviour
{
    public Image rightItem;
    public Image leftItem;

    public void UpdateRight(KeyItem item)
    {
        if (item == null)
        {
            leftItem.sprite = null;
            leftItem.enabled = false;
            return;
        }

        leftItem.enabled = true;
        leftItem.sprite = item.icon;
    }


    public void UpdateLeft(KeyItem item)
    {
        if (item == null)
        {
            rightItem.sprite = null;
            rightItem.enabled = false;
            return;
        }

        rightItem.enabled = true;
        rightItem.sprite = item.icon;
    }
}
