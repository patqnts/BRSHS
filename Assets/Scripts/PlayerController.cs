using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public FixedJoystick joystick;
    public float moveSpeed = 5f;
    private Vector2 movementInput;
    private Animator animator;
    public CircleCollider2D circleCollider;
    private Rigidbody2D rb;
    private bool isAttacking = false;
    private void Start()
    {
        joystick = FindObjectOfType<FixedJoystick>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
      
       movementInput = new Vector2(joystick.Horizontal, joystick.Vertical);
        
        

        if (Input.GetKeyDown(KeyCode.Space) && !isAttacking)
        {
            StartCoroutine(Attack());
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }


    void MovePlayer()
    {
        if (!isAttacking)
        {
            Vector2 movement = new Vector2(movementInput.x, movementInput.y);
            movement.Normalize();

            rb.velocity = movement * moveSpeed;

            UpdateAnimatorParameters(movement);
        }
        else
        {
            // Stop the player's movement while attacking
            rb.velocity = Vector2.zero;
        }
    }

    public void AttackButton()
    {
        if(!isAttacking)
        {
            Attack();
        }
    }

    private void UpdateAnimatorParameters(Vector2 movement)
    {
        animator.SetFloat("x", movement.x);
        animator.SetFloat("y", movement.y);
    }

    public void OnInteractionButtonClick()
    {
        StartCoroutine(EnableDisableCollider());
    }

    private IEnumerator EnableDisableCollider()
    {
        circleCollider.enabled = true;
        yield return new WaitForSeconds(1f);
        circleCollider.enabled = false;
    }

    private IEnumerator Attack()
    {
        isAttacking = true;

        Vector2 attackDirection = new Vector2(animator.GetFloat("x"), animator.GetFloat("y"));

        // Determine the direction of the attack and play the corresponding animation
        if (Mathf.Abs(attackDirection.x) > Mathf.Abs(attackDirection.y))
        {
            // Horizontal attack
            if (attackDirection.x > 0)
                animator.Play("attack right");
            else
                animator.Play("attack left");
        }
        else
        {
            // Vertical attack
            if (attackDirection.y > 0)
                animator.Play("attack up");
            else
                animator.Play("attack down");
        }

        // Add a delay to sync with the attack animation
        yield return new WaitForSeconds(0.4f);

        isAttacking = false;
        animator.Play("Movement");
    }
}
