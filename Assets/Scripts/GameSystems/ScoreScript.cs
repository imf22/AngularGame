using System;
using UnityEngine;
using UnityEngine.UI;


public class ScoreScript : MonoBehaviour
{
    public Text scoreText;
    public int scoreValue;
    public Text comboText0;
    public TMPro.TextMeshProUGUI comboText;
    private int combo;

    // Start is called before the first frame update
    void Start()
    {
        scoreText = this.GetComponent<Text>();
        scoreValue = 0;
        scoreText.text = paddZeros(10, scoreValue.ToString());

        comboText = GameObject.Find("Combo").GetComponent<TMPro.TextMeshProUGUI>();
        combo = 1;
    }



    public void addToScore(int points)
    {
        scoreValue += (points * combo);

        updateScore();
    }

    private void updateScore()
    {
        scoreText.text = paddZeros(10, scoreValue.ToString());
    }


    private string paddZeros(int padding, string Str)
    {
        string paddedScore = "";
        int numZeros = padding - Str.Length;
        for(int i = 0; i < numZeros; i++)
        {
            paddedScore += "0";
        }

        paddedScore += Str;

        return paddedScore;
    }

    public void addCombo()
    {
        combo += 1;
        updateCombo();
    }

    public void resetCombo()
    {
        combo = 1;
        updateCombo();
    }

    public int getCombo()
    {
        return combo;
    }

    private void updateCombo()
    {
        comboText.text = combo.ToString() + "x";
    }

    public String getFinalScore()
    {
        return scoreText.text;
    }
}
