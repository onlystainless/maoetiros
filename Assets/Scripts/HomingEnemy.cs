using UnityEngine;

public class HomingEnemy : MonoBehaviour
{
    public float moveSpeed = 3f; // Movement speed of the enemy
    public float rotationSpeed = 120f; // How quickly the enemy rotates towards the player

    private Transform player;

    void Start()
    {
        // Find the player by tag (assuming the player has the "Player" tag)
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void Update()
    {
        if (player == null) return; // If there's no player, do nothing

        // Calculate direction to player
        Vector2 direction = (player.position - transform.position).normalized;

        // Calculate the rotation step based on rotationSpeed
        float rotationStep = rotationSpeed * Time.deltaTime;

        // Determine the new direction to smoothly rotate towards the player
        Vector2 newDirection = Vector2.Lerp(transform.up, direction, rotationStep).normalized;

        // Set the enemy's rotation to look in the new direction
        transform.up = newDirection;

        // Move the enemy forward in its current direction
        transform.position += transform.up * moveSpeed * Time.deltaTime;
    }
}

