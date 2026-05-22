using UnityEngine;

[CreateAssetMenu(menuName = "Items/Sword")]
public class SwordItem : KeyItem
{
    [SerializeField] public GameObject swordPrefab;

    public override void Use(Player player)
    {
        SoundEffectManager.Play("SwordAttack");
        player.swordAttack.StartAttack();
    }
}
