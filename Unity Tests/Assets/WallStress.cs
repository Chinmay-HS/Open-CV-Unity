/*using UnityEngine;

public class WallStress : MonoBehaviour
{
    public float stressThreshold = 100f;  // Total force required to collapse
    private float currentStress = 0f;
    public GameObject brokenWallPrefab;  // Optional: Assign a destructible prefab

    void Start()
    {
        if (!GetComponent<Rigidbody>())
        {
            Rigidbody rb = gameObject.AddComponent<Rigidbody>();
            rb.mass = 50; // Adjust mass for structural behavior
            rb.isKinematic = false;
        }

        if (!GetComponent<BoxCollider>())
        {
            gameObject.AddComponent<BoxCollider>(); // Ensure collisions are detected
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        float impactForce = collision.relativeVelocity.magnitude;
        currentStress += impactForce;

        Debug.Log($"Wall {gameObject.name} Stress: {currentStress}/{stressThreshold}");

        if (currentStress >= stressThreshold)
        {
            CollapseWall();
        }
    }

    void CollapseWall()
    {
        Debug.LogWarning(gameObject.name + " has collapsed!");

        if (brokenWallPrefab != null)
        {
            Instantiate(brokenWallPrefab, transform.position, transform.rotation);
        }

        Destroy(gameObject);  // Destroy the original wall
    }
}*/

/*
using UnityEngine;
using UnityEngine.UI;

public class WallStress: MonoBehaviour
{
    [Header("Structural Properties")]
    [SerializeField] private float stressThreshold = 200f; // Max stress before collapse
    [SerializeField] private float mass = 50f; // Mass of the wall
    [SerializeField] private Material weakenedMaterial; // Cracked material when weakened
    [SerializeField] private GameObject brokenWallPrefab; // Optional destructible prefab

    [Header("UI Elements")]
    [SerializeField] private Text integrityUI; // Assign a UI Text in Unity

    private float currentStress = 0f;
    private Material originalMaterial;
    private Rigidbody rb;
    private float surfaceArea; // Used to calculate pressure

    void Start()
    {
        // Ensure Rigidbody is present
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
        rb.mass = mass;
        rb.isKinematic = false;

        // Ensure Collider is present
        if (!GetComponent<BoxCollider>())
        {
            gameObject.AddComponent<BoxCollider>();
        }

        // Calculate surface area (Assuming a rectangular wall)
        BoxCollider box = GetComponent<BoxCollider>();
        surfaceArea = box.size.x * box.size.y;

        originalMaterial = GetComponent<Renderer>().material;
    }

    void OnCollisionEnter(Collision collision)
    {
        float impactForce = collision.relativeVelocity.magnitude;
        float pressure = impactForce / surfaceArea; // Simulating force per unit area

        currentStress += pressure;
        float integrityPercentage = 100f - (currentStress / stressThreshold * 100f);
        integrityPercentage = Mathf.Clamp(integrityPercentage, 0f, 100f);

        Debug.Log($"{gameObject.name} Pressure: {pressure} | Total Stress: {currentStress}/{stressThreshold}");

        // Update UI dynamically
        if (integrityUI != null)
        {
            integrityUI.text = $"Wall Integrity: {integrityPercentage}%";
        }

        // Change material to weakened state before collapsing
        if (currentStress >= stressThreshold * 0.7f && weakenedMaterial != null) // 70% damage threshold
        {
            GetComponent<Renderer>().material = weakenedMaterial;
        }

        // Collapse when stress exceeds threshold
        if (currentStress >= stressThreshold)
        {
            CollapseWall();
        }
    }

    void CollapseWall()
    {
        Debug.LogWarning(gameObject.name + " has collapsed!");

        if (brokenWallPrefab != null)
        {
            Instantiate(brokenWallPrefab, transform.position, transform.rotation);
        }

        Destroy(gameObject);
    }
}
*/
using UnityEngine;
using UnityEngine.UI;

public class WallStress : MonoBehaviour
{
    [Header("Structural Properties")]
    public float stressThreshold = 200f; // Max stress before collapse
    public float mass = 50f; // Mass of the wall
    public Material weakenedMaterial; // Material to change when near collapse
    public GameObject brokenWallPrefab; // Optional destructible prefab

    [Header("UI Elements")]
    public Text integrityUI; // Assign a UI Text in Unity to display integrity

    private float currentStress = 0f;
    private Material originalMaterial;
    private Rigidbody rb;
    private float surfaceArea; // Used to calculate pressure

    void Start()
    {
        // Ensure Rigidbody is present
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
        rb.mass = mass;
        rb.isKinematic = false;

        // Ensure Collider is present
        if (!GetComponent<BoxCollider>())
        {
            gameObject.AddComponent<BoxCollider>();
        }

        // Calculate surface area (Assuming a rectangular wall)
        BoxCollider box = GetComponent<BoxCollider>();
        surfaceArea = box.size.x * box.size.y;

        originalMaterial = GetComponent<Renderer>().material;
    }

    void OnCollisionEnter(Collision collision)
    {
        float impactForce = collision.relativeVelocity.magnitude;
        float pressure = impactForce / surfaceArea; // Simulating force per unit area

        currentStress += pressure;
        float integrityPercentage = 100f - (currentStress / stressThreshold * 100f);
        integrityPercentage = Mathf.Clamp(integrityPercentage, 0f, 100f);

        Debug.Log($"{gameObject.name} Pressure: {pressure} | Total Stress: {currentStress}/{stressThreshold}");

        // Update UI dynamically
        if (integrityUI != null)
        {
            integrityUI.text = $"Wall Integrity: {integrityPercentage}%";
        }

        // Change material to weakened state before collapsing
        if (currentStress >= stressThreshold * 0.7f && weakenedMaterial != null) // 70% damage threshold
        {
            GetComponent<Renderer>().material = weakenedMaterial;
        }

        // Collapse when stress exceeds threshold
        if (currentStress >= stressThreshold)
        {
            CollapseWall();
        }
    }

    void CollapseWall()
    {
        Debug.LogWarning(gameObject.name + " has collapsed!");

        if (brokenWallPrefab != null)
        {
            Instantiate(brokenWallPrefab, transform.position, transform.rotation);
        }

        Destroy(gameObject);
    }
}
