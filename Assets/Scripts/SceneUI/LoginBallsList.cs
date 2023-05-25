using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class LoginBallsList : MonoBehaviour
{
    [SerializeField] List<GameObject> bubbles;
    //[SerializeField] TextMeshProUGUI headLine;
    public float colorChangeSpeed = 2f; 
    private float timer;

    void Start()
    {
        DOTween.Init();

        foreach (Transform child in transform)
        {
            child.GetComponent<Renderer>().material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
            child.GetComponent<MeshRenderer>().enabled = true;
            bubbles.Add(child.gameObject);

        }
    }

    //void Update()
    //{
    //    timer += Time.deltaTime * colorChangeSpeed;
    //    float r = Mathf.Sin(timer * 1f) * 0.5f + 0.5f;
    //    float g = Mathf.Sin(timer * 2f) * 0.5f + 0.5f;
    //    float b = Mathf.Sin(timer * 3f) * 0.5f + 0.5f;
    //    headLine.color = new Color(r, g, b);
    //}
}
