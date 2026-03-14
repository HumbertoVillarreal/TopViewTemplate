using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    public GameObject Melee;
    bool isAttacking = false;
    float attkDuration = 0.3f;
    float attkTimer = 0f;

    // Update is called once per frame
    void Update()
    {
        CheckMeleeTimer();
    }


    public void Attack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            StartAttack();
        }
    }


    void StartAttack()
    {
        if (!isAttacking)
        {
            Melee.SetActive(true);
            isAttacking = true;

            //Play animation
        }
    }


    void CheckMeleeTimer()
    {
        if (isAttacking)
        {
            attkTimer += Time.deltaTime;
            if (attkTimer > attkDuration)
            {
                attkTimer = 0f;
                isAttacking = false;
                Melee.SetActive(false);
            }
        }
    }
}
