using UnityEngine;
using System.IO;
using System.Collections.Generic;
using Unity.Mathematics;

[System.Serializable]
public class ManipulationData
{
    public Vector3 targetPosition;
    public Quaternion targetRotation;
}

public class DataReader : MonoBehaviour
{
    public List<ManipulationData> manipulationDataList = new List<ManipulationData>();

    void Awake()
    {
        string filePath = Path.Combine(Application.dataPath, "aruco_data.txt");

        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);

            foreach (string line in lines)
            {
                string[] components = line.Split(':');//-------->Spliting line based on

                if (components.Length == 6) //-------> Ensure the line is in the correct format
                {
                    // Parse components to Vector3
                    Vector3 position = new Vector3(float.Parse(components[0]), float.Parse(components[1]), float.Parse(components[2]));
                    Quaternion rotation = new Quaternion(float.Parse(components[3]), float.Parse(components[4]), float.Parse(components[5]),1);

                    
                    ManipulationData entry = new ManipulationData { targetPosition = position, targetRotation = rotation };// Creating a new Data and adding it to the list
                    manipulationDataList.Add(entry);
                }
                else
                {
                    Debug.LogWarning("Invalid format in line: " + line);
                }
            }
        }
        else
        {
            Debug.LogError("File not found: " + filePath);
        }
    }
}
