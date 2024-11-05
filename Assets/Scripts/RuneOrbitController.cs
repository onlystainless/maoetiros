using UnityEngine;

public class RuneOrbitController : MonoBehaviour
{
    public GameObject runePrefab;       // Prefab of the rune
    public int runeCount = 5;           // Number of runes to orbit around the enemy
    public float orbitRadius = 2f;      // Distance of each rune from the enemy
    public float orbitSpeed = 1f;       // Speed of orbit in rotations per second
    public float runeSize = 1f;         // Size of each rune

    private GameObject[] runes;
    private float[] runeAngles;         // Array to store the initial angle for each rune

    void Start()
    {
        // Initialize runes and angles
        runes = new GameObject[runeCount];
        runeAngles = new float[runeCount];

        // Create runes around the enemy in a circular formation
        for (int i = 0; i < runeCount; i++)
        {
            // Instantiate a rune and set it as a child of this GameObject (the enemy)
            runes[i] = Instantiate(runePrefab, transform.position, Quaternion.identity, transform);

            // Set the size of the rune by adjusting its scale
            runes[i].transform.localScale = Vector3.one * runeSize;

            // Calculate the initial angle for this rune in radians
            runeAngles[i] = i * Mathf.PI * 2 / runeCount;
        }
    }

    void Update()
    {
        // Update each rune's position in the orbit
        for (int i = 0; i < runeCount; i++)
        {
            // Increment the angle based on orbitSpeed
            runeAngles[i] += orbitSpeed * Time.deltaTime;

            // Calculate x and y position using Sin and Cos functions
            float x = transform.position.x + Mathf.Cos(runeAngles[i]) * orbitRadius;
            float y = transform.position.y + Mathf.Sin(runeAngles[i]) * orbitRadius;

            // Set the position of the rune
            runes[i].transform.position = new Vector2(x, y);
        }
    }
}
