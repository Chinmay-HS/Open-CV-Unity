using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class BlueprintLoader : MonoBehaviour
{
    public GameObject wallPrefab;  // Assign the wall prefab in Unity Inspector
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
        }
    }
}