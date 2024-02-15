using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public FixedJoystick joystick;
    public float moveSpeed = 5f;
    private Vector2 movementInput;

    private void Start()
    {
        joystick = FindObjectOfType<FixedJoystick>();
    }
    private void Update()
    {
        // Get joystick input
        movementInput = new Vector2(joystick.Horizontal, joystick.Vertical);
    }

    private void FixedUpdate()
    {
        // Move the player based on joystick input
        MovePlayer();
    }

    private void MovePlayer()
    {
        // Calculate movement direction
        Vector3 movement = new Vector3(movementInput.x, movementInput.y, 0f);

        // Normalize the movement vector to prevent faster movement diagonally
        movement.Normalize();

        // Move the player
        transform.Translate(movement * moveSpeed * Time.fixedDeltaTime);
    }
}
