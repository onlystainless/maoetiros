using UnityEngine;

public class PlayerChargedShooting : MonoBehaviour
{
    public GameObject magicShotPrefab;        // Reference to the magic shot prefab
    public float shotSpeed = 10f;             // Regular speed of the magic shot
    public float chargedShotSpeed = 20f;      // Speed of the charged magic shot
    public float shotSize = 1f;               // Regular size of the magic shot
    public float chargedShotSize = 2f;        // Size of the charged magic shot
    public float maxShrinkFactor = 0.5f;      // Minimum scale of player when fully charged
    public float chargeTime = 2f;             // Time to fully charge the shot
    public float pulseFrequency = 3f;         // Frequency of the pulse effect
    public float pulseMagnitude = 0.1f;       // Intensity of the pulse effect
    public float knockbackForce = 5f;         // Knockback force applied to the player

    private Rigidbody2D rb;
    private Vector3 originalScale;
    private bool isCharging = false;
    private bool fullyCharged = false;
    private float chargeTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalScale = transform.localScale;
    }

    void Update()
    {
        RotateTowardsCursor();

        if (Input.GetMouseButton(0))
        {
            StartCharging();
        }
        else if (isCharging)
        {
            ReleaseCharge();
        }
    }

    void RotateTowardsCursor()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        Vector2 direction = (mousePos - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    void StartCharging()
    {
        isCharging = true;
        chargeTimer += Time.deltaTime;

        // Calculate the dynamic shrink factor based on charge time
        float shrinkProgress = Mathf.Clamp01(chargeTimer / chargeTime);
        float currentShrinkFactor = Mathf.Lerp(1f, maxShrinkFactor, shrinkProgress);

        // Apply the shrinking and pulse effect
        float pulse = Mathf.Sin(Time.time * pulseFrequency) * pulseMagnitude;
        transform.localScale = originalScale * currentShrinkFactor * (1 + pulse);

        if (chargeTimer >= chargeTime)
        {
            fullyCharged = true;
        }
    }

    void ReleaseCharge()
    {
        // Get the position of the mouse in world space
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        // Calculate the direction from the player to the mouse
        Vector2 shotDirection = (mousePos - transform.position).normalized;

        // Determine shot properties based on charge state
        float currentShotSpeed = fullyCharged ? chargedShotSpeed : shotSpeed;
        float currentShotSize = fullyCharged ? chargedShotSize : shotSize;

        // Create and configure the shot
        GameObject magicShot = Instantiate(magicShotPrefab, transform.position, Quaternion.identity);
        magicShot.transform.localScale = Vector3.one * currentShotSize;

        // Set the velocity of the shot based on the direction to the cursor
        Rigidbody2D shotRb = magicShot.GetComponent<Rigidbody2D>();
        if (shotRb != null)
        {
            shotRb.velocity = shotDirection * currentShotSpeed;
        }

        // Apply knockback to the player in the opposite direction of the shot
        rb.AddForce(-shotDirection * knockbackForce, ForceMode2D.Impulse);

        // Reset scaling and charge variables
        transform.localScale = originalScale;
        isCharging = false;
        fullyCharged = false;
        chargeTimer = 0f;

        Debug.Log("Shot released" + (fullyCharged ? " - Charged!" : " - Regular shot"));
    }
}
