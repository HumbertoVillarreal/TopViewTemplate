using UnityEngine;

public class Weapon : MonoBehaviour, IDamageable
{

    [SerializeField] public float dmg = 1f;
    private float health;

    private Transform tf;
    private IAttacker attacker;

    private void Start()
    {
        tf = GetComponent<Transform>();

        attacker = GetComponentInParent<IAttacker>();
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Hurtbox")) { return; }

        IDamageable damageable = collision.GetComponent<IDamageable>();

        IAttacker targetAttacker = collision.GetComponentInParent<IAttacker>();

        //Prevent frindly fire
        if (damageable != null && targetAttacker.GetTeam() != attacker.GetTeam())
        {
            float damage = attacker.GetAttack() + dmg;
            damageable.TakeDamage(damage, tf);
        }
    }

    public void TakeDamage(float takeDamage, Transform attacker)
    {
        throw new System.NotImplementedException();
    }
}
