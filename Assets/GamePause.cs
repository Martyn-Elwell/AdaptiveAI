using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GamePause : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton7) || Input.GetKeyDown(KeyCode.P))
        {
            EditorApplication.isPaused = true;

        }
    }
}
