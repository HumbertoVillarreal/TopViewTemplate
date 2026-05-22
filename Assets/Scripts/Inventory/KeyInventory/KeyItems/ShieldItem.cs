using UnityEngine;

[CreateAssetMenu(menuName = "Items/Shield")]
public class ShieldItem : KeyItem
{
    [SerializeField] public GameObject shieldPrefab;


    public override void OnPress(Player player)
    {
        player.shieldBlock.StartBlock();
    }

    public override void OnRelease(Player player)
    {
        player.shieldBlock.StopBlock();
    }
}
