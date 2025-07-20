using System.Collections;
using UnityEngine;


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
    public Transform directionArrow; // assign in inspector

    private void Start()
    {
        canMove = true;
        joystick = FindObjectOfType<FixedJoystick>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Read WASD input
        Vector2 keyboardInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        // Read joystick input
        Vector2 joystickInput = new Vector2(joystick.Horizontal, joystick.Vertical);

        // Combine both inputs, prioritizing keyboard if used
        movementInput = joystickInput;

        if (keyboardInput != Vector2.zero)
            movementInput = keyboardInput;

        // Update lastMoveDir only when there's movement
        if (movementInput != Vector2.zero)
        {
            lastMoveDir = movementInput.normalized;
        }

        if (lastMoveDir != Vector2.zero && directionArrow != null)
        {
            float angle = Mathf.Atan2(lastMoveDir.y, lastMoveDir.x) * Mathf.Rad2Deg;
            directionArrow.rotation = Quaternion.Euler(0, 0, angle - 90f);

        }
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
