using UnityEngine;

public class KeyItemDictionary : MonoBehaviour
{

    [System.Serializable]
    public class Entry
    {
        public string id;
        public KeyItem item;
    }

    public Entry[] items;


    public KeyItem GetItem(string id)
    {
        foreach (var entry in items)
        {
            if (entry.id == id)
                return entry.item;
        }
        return null;
    }

}
