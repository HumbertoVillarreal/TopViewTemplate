using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, IAttacker
{
    public static Player Instance { get; private set; }

    public PlayerSwordAttack swordAttack;
    public PlayerShieldBlock shieldBlock;

    private Teams team = Teams.Player;

    [Header("Base Stats")]
    [SerializeField] private float maxHealth = 20f;
    [SerializeField] private float baseAttack = 3f;
    [SerializeField] private float baseDefense = 3f;
    [SerializeField] private float baseSpeed = 3f;
    [SerializeField] private float baseLuck = 3f;

    private float currHealth;

    [Header("Equipment")]
    [SerializeField] private KeyItem leftItem;
    [SerializeField] private KeyItem rightItem;
    [SerializeField] private float gold;

    private Rigidbody2D rb;
    private Animator animator;

    public EquippedUI equippedUI;

    private enum ActiveHand
    {
        None,
        Left,
        Right
    }

    private ActiveHand activeHand = ActiveHand.None;


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

        if (shieldBlock.IsBlocking) {
            finalDamage = finalDamage / 3; 
        }

        currHealth -= finalDamage;

        currHealth = MathF.Round(currHealth, 2);

        if (currHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        //Debug.Log("Game Over");

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


    public float getBaseLuck()
    {
        return baseLuck;
    }


    public float getMaxHp()
    {
        return maxHealth;
    }

    public float getCurrHp()
    {
        return currHealth;
    }

    public void SetLeftItem(KeyItem item)
    {
        SoundEffectManager.Play("EquipItem");
        leftItem = item;
        equippedUI.UpdateLeft(item);
        UnityEngine.Debug.Log("Assigned to left click");
    }


    public void SetRightItem(KeyItem item)
    {
        SoundEffectManager.Play("EquipItem");
        rightItem = item;
        equippedUI.UpdateRight(item);
        UnityEngine.Debug.Log("Assigned to RIGHT click");
    }

    public float GetAttack()
    {
        return baseAttack;
    }

    public Teams GetTeam()
    {
        return team;
    }

    public void UseLeftItem(InputAction.CallbackContext context)
    {
        if (PauseController.IsGamePaused || PauseController.IsDialogOpen || PauseController.IsMenuOpen) return;

        if (leftItem == null) return;

        if (activeHand == ActiveHand.Right) return;

        if (context.started)
        {
            activeHand = ActiveHand.Left;
            leftItem.OnPress(this);
        }

        if (context.performed)
        {
            activeHand = ActiveHand.Left;
            leftItem.Use(this);
        }


        if (context.canceled)
        {
            leftItem.OnRelease(this);
            activeHand = ActiveHand.None;
        }

    }

    public void UseRightItem(InputAction.CallbackContext context)
    {
        if (PauseController.IsGamePaused || PauseController.IsDialogOpen || PauseController.IsMenuOpen) return;

        if (rightItem == null) return;

        if (activeHand == ActiveHand.Left) return;

        if (context.started)
        {
            activeHand = ActiveHand.Right;
            rightItem.OnPress(this);
        }

        if (context.performed)
        {
            activeHand = ActiveHand.Right;
            rightItem.Use(this);
        }


        if (context.canceled)
        {
            rightItem.OnRelease(this);
            activeHand = ActiveHand.None;
        }
    }
}
