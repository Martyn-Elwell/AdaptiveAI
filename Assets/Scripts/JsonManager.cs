using UnityEngine;
using System.IO;
using UnityEditor;

// Example data structure
[System.Serializable]
public class PlayerData
{
    public int[] initialAttack = new int[6];
    // 2D array of ints, 6x6
    public int[][] attackTable = new int[6][];

    public PlayerData()
    {
        for (int i = 0; i < 6; i++)
        {
            attackTable[i] = new int[6];
        }
    }
}

public class JsonManager : MonoBehaviour
{
    // Directory where JSON files are stored
    private string directoryPath;
    private string filePath = "";
    [SerializeField] AIController AI;
    private bool saved = false;

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
                PlayerData player = new PlayerData();
                player = CreateNewRecord(AI.initialAttack, AI.attackTable);
                SaveNewRecord(player);
            }
        }
        
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

    public PlayerData CreateNewRecord(int[] intialInput, int[][] attackInput)
    {
        PlayerData player = new PlayerData();
        player.attackTable = attackInput;

        for (int i = 0; i < intialInput.Length; i++)
        {
            player.initialAttack[i] = intialInput[i];
        }

        Debug.Log(attackInput.Length);
        Debug.Log(attackInput[0].Length);

        for (int i = 0; i < attackInput.Length; i++)
        {
            for (int j = 0; i < attackInput[0].Length; j++)
            {
                ;
            }
        }

        return player;

    }

    public void SaveNewRecord(PlayerData data)
    {
        // Write data to JSON file
        WriteToJson(filePath, data);
    }
}
