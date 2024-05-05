using UnityEngine;
using System.IO;
using UnityEditor;

// Example data structure
[System.Serializable]
public class PlayerData
{
    public int[] initialAttack = new int[6];
    // 2D array of ints, 6x6
    public int[,] attackTable = new int[6,6];
}

public class JsonManager : MonoBehaviour
{
    // Directory where JSON files are stored
    private string directoryPath;
    private string filePath = "";
    public AIController AI;
    private bool saved = false;

    private void Start()
    {
        // Set the directory path (You can change the path as per your requirement)
        directoryPath = Application.dataPath + "/PlayerData";
        Debug.Log(directoryPath);



        // Create directory if it doesn't exist
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        // Find the next available file name
        int fileNumber = 1;
        do
        {
            filePath = directoryPath + "/playerData_" + fileNumber + ".json";
            fileNumber++;
        }
        while (File.Exists(filePath)); // Continue until a non-existing file name is found

        
    }

    private void Update()
    {
        if (!saved)
        {
            if (Input.GetKeyDown(KeyCode.JoystickButton6) || Input.GetKeyDown(KeyCode.J))
            {
                saved = true;
                Record player = new Record();
                AI = FindAnyObjectByType<AIController>();
                player.initialAttack = AI.initialAttack;
                player.attackTable = AI.attackTable;

                SaveNewRecord(player);
            }
        }
        
    }

        // Method to write data to JSON file
        private void WriteToJson(string filePath, Record data)
    {
        string jsonData = JsonUtility.ToJson(data);
        File.WriteAllText(filePath, jsonData);
    }

    // Method to read data from JSON file
    private Record ReadFromJson(string filePath)
    {
        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            return JsonUtility.FromJson<Record>(jsonData);
        }
        else
        {
            Debug.LogError("JSON file not found!");
            return null;
        }
    }

    public Record CreateNewRecord(int[] intialInput, int[,] attackInput)
    {
        Record player = new Record();
        player.initialAttack = intialInput;
        player.attackTable = attackInput;

        return player;
    }

    public void SaveNewRecord(Record data)
    {
        // Write data to JSON file
        WriteToJson(filePath, data);
    }
}
