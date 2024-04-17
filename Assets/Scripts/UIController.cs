using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Slider playerHealthbar;
    [SerializeField] private TextMeshProUGUI playerScoreText;
    [SerializeField] private Slider AiHealthbar;
    [SerializeField] private TextMeshProUGUI AiScoreText;
    [SerializeField] private TextMeshProUGUI comboText;
    [SerializeField] private TextMeshProUGUI AiPredcitionCountText;
    [SerializeField] private TextMeshProUGUI AiAccuracyText;


    public void UpdateHealth(Controller character, float health)
    {
        if (character is PlayerController)
        {
            playerHealthbar.value = health;
        }
        else if (character is AIController)
        {
            AiHealthbar.value = health;
        }

    }

    public void UpdateScore(Controller character, int score)
    {
        if (character is PlayerController)
        {
            playerScoreText.text = "Score: " + score.ToString();
        }
        else if (character is AIController)
        {
            AiScoreText.text = "Score: " + score.ToString();
        }
    }

    public void UpdateComboCount(int comboCount)
    {
        if (comboCount == 0)
        {
            comboText.gameObject.SetActive(false);
        }
        else if (comboCount >= 1)
        {
            if (comboText.gameObject.activeSelf == false)
            {
                comboText.gameObject.SetActive(true);
            }

            comboText.text = comboCount.ToString();
        }
        
    }

    public void UpdateAccuracy(float count, float accuracy)
    {
        Debug.LogWarning(accuracy * 100);

        string input = (accuracy*100).ToString("0.0") + "%";
        AiPredcitionCountText.text = count.ToString();
        AiAccuracyText.text = input;

    }


}
