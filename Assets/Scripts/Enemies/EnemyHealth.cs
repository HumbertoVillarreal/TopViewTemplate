using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    private Enemy enemy;
    private float health;
    private Rigidbody2D rb;

    [SerializeField] private float knockbackForce = 5f;
    [SerializeField] private float knockbackTime = 0.2f;

    private EnemyMovement movement;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemy = GetComponentInParent<Enemy>();
        health = enemy.maxHealth;
        movement = GetComponentInParent<EnemyMovement>();

        rb = GetComponentInParent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage, Transform attacker)
    {
        enemy.TakeDamage(damage);


        //Damage knockback
        Vector2 knockBackDir = (transform.position - attacker.position).normalized;

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(knockBackDir * knockbackForce, ForceMode2D.Impulse);

        movement.KnockBack(knockbackTime);
    }

    public float getHP()
    {
        return health;
    }

}
