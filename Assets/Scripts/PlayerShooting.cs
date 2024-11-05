using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject magicShotPrefab;       // Reference to the magic shot prefab
    public float shotSpeed = 10f;            // Speed of the magic shot
    public float knockbackForce = 5f;        // Knockback force applied to the player
    public float shotSize = 1f;              // Size of the magic shot
    public float shrinkFactor = 0.9f;        // Factor to shrink the player when shooting
    public float deshrinkSpeed = 5f;         // Speed to return to original scale

    private Rigidbody2D rb;
    private Vector3 originalScale;
    private bool isDeshrinking = false;

    void Start()
    {
        // Get the Rigidbody2D component and store the player's original scale
        rb = GetComponent<Rigidbody2D>();
        originalScale = transform.localScale;
    }

    void Update()
    {
        // Update player rotation to face the cursor
        RotateTowardsCursor();

        // Shoot when the left mouse button is pressed
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }

        // Smoothly return to original scale if shrinking is active
        if (isDeshrinking)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale, deshrinkSpeed * Time.deltaTime);

            // Stop deshrinking once close to the original scale
            if (Vector3.Distance(transform.localScale, originalScale) < 0.01f)
            {
                transform.localScale = originalScale;
                isDeshrinking = false;
            }
        }
    }

    void RotateTowardsCursor()
    {
        // Get the mouse position in world space
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0; // Ensure we're working in 2D

        // Calculate the direction from the player to the mouse
        Vector2 direction = (mousePos - transform.position).normalized;

        // Calculate the angle in degrees and rotate the player
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    void Shoot()
    {
        // Get the position of the mouse in world space
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0; // Ensure we're working in 2D

        // Calculate the direction from the player to the mouse
        Vector2 shotDirection = (mousePos - transform.position).normalized;

        // Instantiate the magic shot at the player's position
        GameObject magicShot = Instantiate(magicShotPrefab, transform.position, Quaternion.identity);

        // Set the size of the magic shot
        magicShot.transform.localScale = Vector3.one * shotSize;

        // Set the velocity of the shot based on the direction to the mouse position
        Rigidbody2D shotRb = magicShot.GetComponent<Rigidbody2D>();
        if (shotRb != null)
        {
            shotRb.velocity = shotDirection * shotSpeed;
        }

        // Apply knockback to the player in the opposite direction of the shot
        rb.AddForce(-shotDirection * knockbackForce, ForceMode2D.Impulse);

        // Temporarily shrink the player's scale
        transform.localScale = originalScale * shrinkFactor;

        // Start deshrinking back to original scale
        isDeshrinking = true;

        Debug.Log("Magic Shot Fired towards cursor with knockback and shrinking effect!");
    }
}
