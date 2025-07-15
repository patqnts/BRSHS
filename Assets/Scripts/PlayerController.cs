using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerController : MonoBehaviour
{
    public FixedJoystick joystick;
    public float moveSpeed = 5f;
    private Vector2 movementInput;
    private Animator animator;
    public CircleCollider2D circleCollider;
    private Rigidbody2D rb;
    public bool canMove = true;
    public Vector2 lastMoveDir;

    private void Start()
    {
        canMove = true;
        joystick = FindObjectOfType<FixedJoystick>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (movementInput != Vector2.zero)
        {
            lastMoveDir = movementInput.normalized;
        }
        movementInput = new Vector2(joystick.Horizontal, joystick.Vertical);
    }

    private void FixedUpdate()
    {

        MovePlayer();
    }

    void MovePlayer()
    {
        if (!canMove)
        {
            rb.velocity = Vector2.zero; // Stop movement immediately
            return;
        }


        Vector2 movement = new Vector2(movementInput.x, movementInput.y);
        movement.Normalize();

        rb.velocity = movement * moveSpeed;

        UpdateAnimatorParameters(movement);
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
}
