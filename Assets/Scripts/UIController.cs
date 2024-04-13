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
        comboText.text = comboCount.ToString();
    }


}
