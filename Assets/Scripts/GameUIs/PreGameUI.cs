using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class PreGameUI : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI startCountDownText;
    readonly int startCountDown = 3;

    void Start()
    {
        startCountDownText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void StartCountDown()
    {
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
        //GameManagerr.Instance.pw.RPC("StartTheGame", RpcTarget.All);
        StartCoroutine(CleanUI());
    }

    IEnumerator CleanUI()
    {
        yield return new WaitForSeconds(1f);
        startCountDownText.text = "";
        gameObject.SetActive(false);
        UIManager.Instance.GameUISection.gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        StartCountDown();
    }

}
