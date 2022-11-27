using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public TextMeshProUGUI countDownText;
    float currCountdownValue;
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

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartCountdown());
    }

    // Update is called once per frame
    void Update()
    {
        ScoreDisplay();
    }

    public IEnumerator StartCountdown(float countdownValue = 5)
    {
        currCountdownValue = countdownValue;
        while (currCountdownValue > 0)
        {
            countDownText.text = currCountdownValue.ToString();
            yield return new WaitForSeconds(1.0f);
            currCountdownValue--;
        }
        countDownText.text = "SHOOT";
        StartCoroutine(DisplayEnabled());
    }
    public IEnumerator DisplayEnabled()
    {
        yield return new WaitForSeconds(1.0f);
        countDownText.enabled = false;
    }

    private void ScoreDisplay()
    {
        teamScores[0].text = "Blue: " + ScoreBoard.Instance.blueScore;
        teamScores[1].text = "Red: " + ScoreBoard.Instance.redScore;
        teamScores[2].text = "Yellow: " + ScoreBoard.Instance.yellowScore;
        teamScores[3].text = "Green: " + ScoreBoard.Instance.greenScore;
    }




}
