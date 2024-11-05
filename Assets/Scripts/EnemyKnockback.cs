using UnityEngine;

public class EnemyKnockback : MonoBehaviour
{
    public float knockbackForce = 10f;       // Force applied to enemy on hit
    public float knockbackDecayRate = 5f;    // Rate at which knockback effect decays
    private Rigidbody2D rb;
    private bool isKnockedback = false;

    void Start()
    {
        // Get the Rigidbody2D component on the enemy
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Gradually reduce knockback effect if currently being knocked back
        if (isKnockedback)
        {
            rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, knockbackDecayRate * Time.deltaTime);

            // Stop knockback effect when velocity is near zero
            if (rb.velocity.magnitude < 0.1f)
            {
                rb.velocity = Vector2.zero;
                isKnockedback = false;
            }
        }
    }

    public void ApplyKnockback(Vector2 direction)
    {
        // Apply knockback force in the specified direction
        rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);
        isKnockedback = true;
    }
}
