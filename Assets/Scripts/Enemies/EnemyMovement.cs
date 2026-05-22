using System.Collections;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float moveSpeed = 2f;

    private float lastInputX;
    private float lastInputY;

    private Rigidbody2D rb;
    private Transform _target;
    private Vector2 moveDirection;

    private Animator animator;

    public Transform Aim;
    public Transform MeleeAim;

    private bool isKnocked = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseController.IsDialogOpen || PauseController.IsMenuOpen)
        {
            moveDirection = Vector2.zero;
            rb.linearVelocity = Vector2.zero;

            animator.SetBool("isWalking", false);

            return;
        }

        if (_target != null)
        {
            Vector2 dir = (_target.position - transform.position).normalized;
            moveDirection = dir;

            if (dir.magnitude > 0)
            {
                lastInputX = dir.x;
                lastInputY = dir.y;
            }

            Vector3 vector3 = (Vector3.left * dir.x + Vector3.down * dir.y);
            Aim.rotation = Quaternion.LookRotation(Vector3.forward, vector3);
            MeleeAim.rotation = Quaternion.LookRotation(Vector3.forward, vector3);

            animator.SetFloat("InputX", dir.x);
            animator.SetFloat("InputY", dir.y);
            animator.SetBool("isWalking", true);
        }
        else
        {
            moveDirection = Vector2.zero;
            rb.linearVelocity = Vector2.zero;

            animator.SetBool("isWalking", false);
            animator.SetFloat("LastInputX", lastInputX);
            animator.SetFloat("LastInputY", lastInputY);
        }
    }


    private void FixedUpdate()
    {
        if (_target)
        {
            if (isKnocked) { return; }

            rb.linearVelocity = new Vector2(moveDirection.x, moveDirection.y) * moveSpeed;
            animator.SetBool("isWalking", true);
        }
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    public void KnockBack(float duration)
    {
        StartCoroutine(KnockbackRoutine(duration));
    }

    IEnumerator KnockbackRoutine(float duration)
    {
        isKnocked = true;

        yield return new WaitForSeconds(duration);

        isKnocked = false;
    }

}
