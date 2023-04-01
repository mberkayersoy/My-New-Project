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

    public PreGameUI PreGameUISection;
    public GameUI GameUISection;
    public GameEndUI GameEndUISection;


    private void Start()
    {
        PreGameUISection.gameObject.SetActive(true);
        GameUISection.gameObject.SetActive(false);
        GameEndUISection.gameObject.SetActive(false);
    }
}
