using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.IO;

/// <summary>
/// Manager class for room
/// </summary>
public class RoomManager : MonoBehaviourPunCallbacks
{
    public PlayerManage playerManage;
    /// <summary>
    /// Singleton instance
    /// </summary>
    public static RoomManager Instance;


    public void PlayerCreate()
    {
        Instantiate(playerManage);
    }

    /// <summary>
    /// Event when enabled
    /// </summary>
    public override void OnEnable()
    {
        //base.OnEnable();
        //SceneManager.sceneLoaded += OnSceneLoaded;
        //playerManage = FindObjectOfType<PlayerManage>();
    }

    /// <summary>
    /// Event when disabled
    /// </summary>
    public override void OnDisable()
    {
        //base.OnDisable();
        //SceneManager.sceneLoaded -= OnSceneLoaded;

    }

    /// <summary>
    /// Instance initialization
    /// </summary>
    void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }
}
