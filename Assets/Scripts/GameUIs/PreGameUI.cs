using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PreGameUI : MonoBehaviour
{
    private TextMeshProUGUI startCountDownText;
    readonly int startCountDown = 2;
    // Start is called before the first frame update
    void Start()
    {
        startCountDownText = GetComponentInChildren<TextMeshProUGUI>();
        StartCoroutine(CountDownDisplay());
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
        gameObject.SetActive(false);
        UIManager.Instance.GameUISection.gameObject.SetActive(true);
    }
}
