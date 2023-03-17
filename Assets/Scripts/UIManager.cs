using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public TextMeshProUGUI countDownText;
    public GameObject door;
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
    void Update()
    {
        ScoreDisplay();
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
