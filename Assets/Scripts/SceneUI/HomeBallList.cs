using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeBallList : MonoBehaviour
{
    [SerializeField] List<GameObject> bubbles;

    void Start()
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<Renderer>().material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
            child.GetComponent<MeshRenderer>().enabled = true;
            bubbles.Add(child.gameObject);
        }
    }
}
