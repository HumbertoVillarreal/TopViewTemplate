using UnityEngine;

[CreateAssetMenu(menuName = "Items/Wrench")]
public class WrenchItem : KeyItem
{
    [SerializeField] public GameObject wrenchPrefab;
    [SerializeField] public float range = 1.5f;

    public override void Use(Player player)
    {

        Debug.Log("Object reppaired !!");
    }
}