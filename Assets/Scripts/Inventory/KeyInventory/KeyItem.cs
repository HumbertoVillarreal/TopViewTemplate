using UnityEngine;

public abstract class KeyItem : ScriptableObject
{
    [SerializeField] public string itemName;

    [Header("UI")]
    [SerializeField] public Sprite icon;

    public virtual void Use(Player player) { }

    public virtual void OnPress(Player player) { }
    public virtual void OnRelease(Player player) { }
}
