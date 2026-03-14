using UnityEngine;

public class Player : MonoBehaviour, IAttacker
{
    public static Player Instance { get; private set; }

    private Teams team = Teams.Player;

    [Header("Base Stats")]
    [SerializeField] private float maxHealth = 20f;
    [SerializeField] private float baseAttack = 3f;
    [SerializeField] private float baseDefense = 3f;
    [SerializeField] private float baseSpeed = 3f;
    [SerializeField] private float baseLuck = 3f;

    private float currHealth;

    [Header("Equipment")]
    private GameObject leftItem;
    private GameObject rightItem;
    private float gold;

    private Rigidbody2D rb;
    private Animator animator;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }


    public void Start()
    {
        currHealth = maxHealth;
    }


    public void TakeDamage(float damage)
    {
        float finalDamage = Mathf.Max(damage - baseDefense, 1f);
        currHealth -= finalDamage;

        if (currHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("Game Over");

        //Handle GAME OVER logic
    }


    public float getBaseAttack()
    {
        return baseAttack;
    }


    public float getBaseDefense()
    {
        return baseDefense;
    }


    public float getBaseSpeed()
    {
        return baseSpeed;
    }


    public float getMaxHp()
    {
        return baseAttack;
    }

    public float getCurrHp()
    {
        return currHealth;
    }

    public void SetLeftItem(GameObject item)
    {
        leftItem = item;
    }


    public void SetRightItem(GameObject item)
    {
        rightItem = item;
    }

    public float GetAttack()
    {
        return baseAttack;
    }

    public Teams GetTeam()
    {
        return team;
    }
}
