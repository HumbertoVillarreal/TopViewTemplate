using UnityEngine;

public class ShieldCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        SoundEffectManager.Play("ShieldBlock");
        if (collision.CompareTag("Projectile"))
        {
            Destroy(collision.gameObject);
        }
    }
}
