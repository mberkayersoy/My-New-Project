using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public List<TextMeshProUGUI> teamScores;
    public TextMeshProUGUI startCountDownText;
    int startCountDown = 2;

    private void Start()
    {

        StartCoroutine(CountDownDisplay());
        ScoreDisplay();
    }

    IEnumerator CountDownDisplay()
    {
        for (int i = startCountDown; i > 0; i--)
        {
            startCountDownText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }
        startCountDownText.text = "GO!";

        GameManager.Instance.StartTheGame();
        StartCoroutine(CleanUI());
    }

    IEnumerator CleanUI()
    {
        yield return new WaitForSeconds(1f);
        startCountDownText.text = "";
    }
    public void ScoreDisplay()
    {
        teamScores[0].text = "Blue: " + ScoreBoard.Instance.blueScore;
        teamScores[1].text = "Red: " + ScoreBoard.Instance.redScore;
        teamScores[2].text = "Green: " + ScoreBoard.Instance.greenScore;
        teamScores[3].text = "Yellow: " + ScoreBoard.Instance.yellowScore;

    }
}
