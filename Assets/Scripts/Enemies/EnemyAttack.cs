using System.Collections;
using UnityEngine;
using UnityEngine.Animations;

public class EnemyAttack : MonoBehaviour
{
    public GameObject bullet;

    [SerializeField] public Transform Aim;
    [SerializeField] public Transform ShootPoint;
    [SerializeField] private float spawnOffset = 0.3f;
    private Transform target;

    [SerializeField] private Transform meleeHitbox;
    [SerializeField] private float meleeCooldown = 1f;
    private float meleeTimer;

    [SerializeField] public float fireForce = 10f;
    [SerializeField] public float shootCooldown = 0.5f;
    [SerializeField] public float shootTimer = 0.5f;
    [SerializeField] private float randomShootChance = 0.4f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        meleeHitbox.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) return;

        float dist = Vector2.Distance(transform.position, target.position);

        shootTimer += Time.deltaTime;
        meleeTimer += Time.deltaTime;

        if (dist < 1.5f) // melee range
        {
            if (meleeTimer >= meleeCooldown)
            {
                if (Random.value < randomShootChance)
                {
                    Melee();
                    meleeTimer = 0f;
                }
            }
        }
        else
        {
            if (shootTimer >= shootCooldown)
            {
                if (Random.value < randomShootChance)
                {
                    Shoot();
                }

                shootTimer = 0f;
            }
        }
    }


    void Shoot()
    {
        if (PauseController.IsGamePaused || PauseController.IsDialogOpen || PauseController.IsMenuOpen) return;

        SoundEffectManager.Play("Shoot");
        Vector3 spawnPos = ShootPoint.position + ShootPoint.up * spawnOffset;
        GameObject intBullet = Instantiate(bullet, spawnPos  , Aim.rotation, transform);
        intBullet.GetComponent<Rigidbody2D>().AddForce(-Aim.up * fireForce, ForceMode2D.Impulse);
        Destroy(intBullet, 2f);
    }


    void Melee()
    {
        if (PauseController.IsGamePaused || PauseController.IsDialogOpen || PauseController.IsMenuOpen) return;

        StartCoroutine(MeleeAttack());
    }

    IEnumerator MeleeAttack()
    {
        SoundEffectManager.Play("Melee");
        meleeHitbox.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.2f);

        meleeHitbox.gameObject.SetActive(false);
    }


    public void SetTarget(Transform tf)
    {
        target = tf;
    }
}
