using NUnit.Framework.Interfaces;
using UnityEngine;
using UnityEngine.UI;

public class KeyItemUI : MonoBehaviour
{
    public KeyItem itemData;
    public string itemID;
    public Image icon;

    public void Setup(KeyItem item)
    {
        itemData = item;
        itemID = item.name;
        icon.sprite = item.icon;
    }

}

