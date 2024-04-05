using UnityEngine;
using System.IO;

// Example data structure
[System.Serializable]
public class PlayerData
{
    // 2D array of ints, 6x6
    public int[,] playerGrid = new int[6, 6];
    public string test = "test";
}

public class JsonManager : MonoBehaviour
{
    // Directory where JSON files are stored
    private string directoryPath;

    private void Start()
    {
        // Set the directory path (You can change the path as per your requirement)
        directoryPath = Application.persistentDataPath + "/PlayerData";
        Debug.Log(directoryPath);

        // Create directory if it doesn't exist
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        // Find the next available file name
        int fileNumber = 1;
        string filePath = "";
        do
        {
            filePath = directoryPath + "/playerData_" + fileNumber + ".json";
            fileNumber++;
        }
        while (File.Exists(filePath)); // Continue until a non-existing file name is found

        // Example usage
        PlayerData player = new PlayerData();
        player.playerGrid[0, 0] = 1; // Example data
        player.playerGrid[1, 1] = 2; // Example data

        // Write data to JSON file
        WriteToJson(filePath, player);

        // Read data from JSON file
        PlayerData loadedPlayer = ReadFromJson(filePath);

        // Example usage of loaded data
        Debug.Log("Player Grid[0, 0]: " + loadedPlayer.playerGrid[0, 0]);
        Debug.Log("Player Grid[1, 1]: " + loadedPlayer.playerGrid[1, 1]);
    }

    // Method to write data to JSON file
    private void WriteToJson(string filePath, PlayerData data)
    {
        string jsonData = JsonUtility.ToJson(data);
        File.WriteAllText(filePath, jsonData);
    }

    // Method to read data from JSON file
    private PlayerData ReadFromJson(string filePath)
    {
        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            return JsonUtility.FromJson<PlayerData>(jsonData);
        }
        else
        {
            Debug.LogError("JSON file not found!");
            return null;
        }
    }
}
