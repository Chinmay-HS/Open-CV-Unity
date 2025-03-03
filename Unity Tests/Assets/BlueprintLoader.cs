/*
using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class BlueprintLoader : MonoBehaviour
{
    public GameObject wallPrefab;  // Assign the wall prefab in Unity Inspector
    public GameObject brokenWallPrefab;  // Optional: Assign a destructible version
    private string filePath;

    void Start()
    {
        filePath = Path.Combine(Application.streamingAssetsPath, "walls.txt");
        LoadBlueprint();
    }

    void LoadBlueprint()
    {
        if (!File.Exists(filePath))
        {
            Debug.LogError("Blueprint file not found: " + filePath);
            return;
        }

        string[] lines = File.ReadAllLines(filePath);
        List<Vector2> wallPoints = new List<Vector2>();

        foreach (string line in lines)
        {
            string[] values = line.Split(' ');
            if (values.Length == 2)
            {
                float x = float.Parse(values[0]) / 100f; // Scale down for Unity
                float y = float.Parse(values[1]) / 100f;
                wallPoints.Add(new Vector2(x, y));
            }
        }

        GenerateWalls(wallPoints);
    }

    void GenerateWalls(List<Vector2> points)
    {
        for (int i = 0; i < points.Count - 1; i++)
        {
            Vector2 start = points[i];
            Vector2 end = points[i + 1];

            Vector3 position = new Vector3((start.x + end.x) / 2, 1, (start.y + end.y) / 2);
            float length = Vector2.Distance(start, end);
            float angle = Mathf.Atan2(end.y - start.y, end.x - start.x) * Mathf.Rad2Deg;

            GameObject wall = Instantiate(wallPrefab, position, Quaternion.Euler(0, -angle, 0));
            wall.transform.localScale = new Vector3(length, 2, 0.1f); // Adjust wall height & thickness

            // ✅ Attach Structural Integrity Components
            AttachStructuralIntegrity(wall);
        }
    }

    void AttachStructuralIntegrity(GameObject wall)
    {
        if (!wall.GetComponent<Rigidbody>())
        {
            Rigidbody rb = wall.AddComponent<Rigidbody>();
            rb.mass = 50; // Adjust mass for structural behavior
            rb.isKinematic = false; // Allow physics interactions
        }

        if (!wall.GetComponent<BoxCollider>())
        {
            wall.AddComponent<BoxCollider>(); // Ensure collisions
        }

        if (!wall.GetComponent<WallStress>())
        {
            WallStress stressScript = wall.AddComponent<WallStress>();
            stressScript.stressThreshold = 100f; // Adjust threshold for breaking
            stressScript.brokenWallPrefab = brokenWallPrefab; // Assign broken prefab
        }
    }
}
*/
using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class BlueprintLoader : MonoBehaviour
{
    public GameObject wallPrefab; // Assign wall prefab in Unity Inspector
    public GameObject brokenWallPrefab; // Assign destructible version if available
    private string filePath;

    void Start()
    {
        filePath = Path.Combine(Application.streamingAssetsPath, "walls.txt");
        LoadBlueprint();
    }

    void LoadBlueprint()
    {
        if (!File.Exists(filePath))
        {
            Debug.LogError("Blueprint file not found: " + filePath);
            return;
        }

        string[] lines = File.ReadAllLines(filePath);
        List<Vector2> wallPoints = new List<Vector2>();

        foreach (string line in lines)
        {
            string[] values = line.Split(' ');
            if (values.Length == 2)
            {
                float x = float.Parse(values[0]) / 100f; // Scale down for Unity
                float y = float.Parse(values[1]) / 100f;
                wallPoints.Add(new Vector2(x, y));
            }
        }

        GenerateWalls(wallPoints);
    }

    void GenerateWalls(List<Vector2> points)
    {
        for (int i = 0; i < points.Count - 1; i++)
        {
            Vector2 start = points[i];
            Vector2 end = points[i + 1];

            Vector3 position = new Vector3((start.x + end.x) / 2, 1, (start.y + end.y) / 2);
            float length = Vector2.Distance(start, end);
            float angle = Mathf.Atan2(end.y - start.y, end.x - start.x) * Mathf.Rad2Deg;

            GameObject wall = Instantiate(wallPrefab, position, Quaternion.Euler(0, -angle, 0));
            wall.transform.localScale = new Vector3(length, 2, 0.1f); // Adjust wall height & thickness

            // Attach Structural Integrity Components
            AttachStructuralIntegrity(wall);
        }
    }

    void AttachStructuralIntegrity(GameObject wall)
    {
        if (!wall.GetComponent<Rigidbody>())
        {
            Rigidbody rb = wall.AddComponent<Rigidbody>();
            rb.mass = 50; // Adjust mass for structural behavior
            rb.isKinematic = false; // Allow physics interactions
        }

        if (!wall.GetComponent<BoxCollider>())
        {
            wall.AddComponent<BoxCollider>(); // Ensure collisions
        }

        if (!wall.GetComponent<WallStress>())
        {
            WallStress stressScript = wall.AddComponent<WallStress>();
            stressScript.stressThreshold = 100f; // Adjust threshold for breaking
            stressScript.brokenWallPrefab = brokenWallPrefab; // Assign broken prefab
        }
    }
}
