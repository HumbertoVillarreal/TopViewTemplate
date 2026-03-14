using UnityEngine;

public class Enemy : MonoBehaviour, IAttacker
{
    private Teams team = Teams.Enemy;

    [Header("Stats")]
    [SerializeField] public float maxHealth;
    [SerializeField] public float spd;
    [SerializeField] public float def;
    [SerializeField] public float atk;

    [Header("Combat")]
    [SerializeField] public float detectionRange;
    [SerializeField] private bool canShoot;
    [SerializeField] private bool canMelee;
    private float currHealth;

    [SerializeField] GameObject[] dropItems;

    [Header("Spawn")]
    private Vector3 spawnPos;
    private Room room;

 

    private void Start()
    {
        currHealth = maxHealth;
        spawnPos = transform.position;

        room = GetComponentInParent<Room>();

        //Register enemy in room
        if(room != null)
        {
            room.RegisterEnemy(this);
        }
    }

    public float GetAttack()
    {
        return atk;
    }

    public bool CanShoot()
    {
        return canShoot;
    }


    public bool CanMelee()
    {
        return canMelee;
    }

    public void TakeDamage(float damage)
    {
        float finalDamage = Mathf.Max(damage - def, 1f);
        currHealth -= finalDamage;

        if (currHealth <= 0)
        {
            Die();
        }
    }


    public void Die()
    {
        DropItem();
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (room != null)
        {
            room.RemoveEnemy(this);
        }
    }


    void DropItem()
    {
        if (dropItems.Length == 0) return;

        int randomIndex = Random.Range(0, dropItems.Length);

        GameObject dropItem = dropItems[randomIndex];
        Item item = dropItem.GetComponent<Item>();

        if (Random.value > item.GetDropRate()) return;

        Instantiate(
            dropItem,
            transform.position,
            Quaternion.identity
        );
    }

    public Teams GetTeam()
    {
        return team;
    }


    public void ResetPosition()
    {
        transform.position = spawnPos;
    }
}
