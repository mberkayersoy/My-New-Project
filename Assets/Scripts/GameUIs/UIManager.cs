using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;

public class UIManager : MonoBehaviourPunCallbacks
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
    public PhotonView pw;


    private void Start()
    {
        PreGameUISection.gameObject.SetActive(true);
        GameUISection.gameObject.SetActive(false);
        GameEndUISection.gameObject.SetActive(false);
    }
}
