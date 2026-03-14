using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    private EnemyMovement enemyMovement;
    private Enemy enemy;
    private CircleCollider2D detectionCollider;

    private EnemyAttack enemyAttack;

    private void Awake()
    {
        enemyMovement = GetComponentInParent<EnemyMovement>();
        enemy = GetComponentInParent<Enemy>();
        detectionCollider = GetComponent<CircleCollider2D>();
        enemyAttack = GetComponentInParent<Enemy>().GetComponentInChildren<EnemyAttack>();

        detectionCollider.radius = enemy.detectionRange;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            enemyMovement.SetTarget(collision.transform);
            enemyAttack.SetTarget(collision.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            enemyMovement.SetTarget(null);
            enemyAttack.SetTarget(null);
        }
    }

}
