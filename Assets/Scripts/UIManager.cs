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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        teamScores[0].text = "Blue: " + ScoreBoard.Instance.blueScore;
        teamScores[1].text = "Red: " + ScoreBoard.Instance.redScore;
        teamScores[2].text = "Yellow: " + ScoreBoard.Instance.yellowScore;
        teamScores[3].text = "Green: " + ScoreBoard.Instance.greenScore;
    }


}
