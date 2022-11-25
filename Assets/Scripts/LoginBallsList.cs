using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoginBallsList : MonoBehaviour
{
    [SerializeField] List<GameObject> bubbles;
    [SerializeField] TextMeshProUGUI headLine;
    Color currentColor;
    //Color timeColor;
    //float r = 0.2f, g = 0.3f, b = 0.7f, a = 0.6f;

    void Start()
    {
        //timeColor = new Color(r, g, b, a);
        //headLine.color = timeColor;
        foreach (Transform child in transform)
        {
            child.GetComponent<Renderer>().material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
            bubbles.Add(child.gameObject);

        }
    }

    void Update()
    {
         
    }
}
