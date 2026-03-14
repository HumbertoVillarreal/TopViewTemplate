using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public void TakeDamage(float dmg, Transform attacker)
    {
        Player.Instance.TakeDamage(dmg);
        Debug.Log("Players HP: " + Player.Instance.getCurrHp());
    }
}
