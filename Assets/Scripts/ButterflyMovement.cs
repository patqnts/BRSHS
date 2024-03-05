using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ButterflyMovement : MonoBehaviour
{
    public Tilemap tilemap;
    public float speed = 3f;

    public float changeDirectionInterval = 2f; // Time interval to change direction
    public float maxRandomAngle = 45f; // Maximum random angle change

    private float timer;

    private void Start()
    {
        timer = changeDirectionInterval;
    }

    void Update()
    {
        Move();
        UpdateTimer();
    }

    void Move()
    {
        Vector3 currentPosition = transform.position;

        // Move the butterfly
        currentPosition += transform.up * speed * Time.deltaTime;
        transform.position = currentPosition;

        // Check and correct if the butterfly goes out of the Tilemap bounds
        ClampToBounds();
    }

    void ClampToBounds()
    {

        Vector3 clampedPosition = transform.position;

        // Calculate the bounds based on the Tilemap
        BoundsInt bounds = tilemap.cellBounds;

        // Clamp the butterfly's position within the Tilemap bounds
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, bounds.x, bounds.x + bounds.size.x - 1);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, bounds.y, bounds.y + bounds.size.y - 1);

        transform.position = clampedPosition;

        // Change direction if the butterfly hits the bounds
        if (clampedPosition.x == bounds.x || clampedPosition.x == bounds.x + bounds.size.x - 1 ||
            clampedPosition.y == bounds.y || clampedPosition.y == bounds.y + bounds.size.y - 1)
        {
            ChangeDirection();
        }
    }

    void UpdateTimer()
    {
        timer -= Time.deltaTime;

        // Change direction after a certain interval
        if (timer <= 0f)
        {
            ChangeDirection();
            timer = changeDirectionInterval;
        }
    }

    void ChangeDirection()
    {
        // Add randomness to the butterfly's direction
        float randomAngle = Random.Range(-maxRandomAngle, maxRandomAngle);
        transform.Rotate(Vector3.forward, randomAngle);
    }
}
