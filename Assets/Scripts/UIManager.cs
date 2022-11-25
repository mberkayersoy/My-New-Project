using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public TextMeshProUGUI countDownText;
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
        CountDown(); 
    }

    // Update is called once per frame
    void Update()
    {
        teamScores[0].text = "Blue: " + ScoreBoard.Instance.blueScore;
        teamScores[1].text = "Red: " + ScoreBoard.Instance.redScore;
        teamScores[2].text = "Yellow: " + ScoreBoard.Instance.yellowScore;
        teamScores[3].text = "Green: " + ScoreBoard.Instance.greenScore;
    }
    public void CountDown()
    {
        for (float i = 3; i >= 0;)
        {
            i -= Time.deltaTime;
            countDownText.text = i.ToString();
        }
    }


}
