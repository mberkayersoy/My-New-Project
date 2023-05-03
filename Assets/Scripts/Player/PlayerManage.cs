using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using System;
public class PlayerManage : MonoBehaviour
{
    PhotonView photonView;
    public GameObject controller;

    public void Die()
    {
        PhotonNetwork.Destroy(controller);
        CreateDiedController();
    }

    void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    void Start()
    {
        if (photonView.IsMine)
        {
            CreateController();
        }
    }
    void CreateController()
    {
        switch(PhotonNetwork.LocalPlayer.GetTeamID())
        {
            case 0:
                controller = PhotonNetwork.Instantiate("CapsuleBlue", GameManagerr.Instance.GeneratePlayers(PhotonNetwork.LocalPlayer.GetTeamID()), Quaternion.identity, 0, new object[] { photonView.ViewID });
                break;
            case 1:
                controller = PhotonNetwork.Instantiate("CapsuleRed", GameManagerr.Instance.GeneratePlayers(PhotonNetwork.LocalPlayer.GetTeamID()), Quaternion.identity, 0, new object[] { photonView.ViewID });
                break;
            case 2:
                controller = PhotonNetwork.Instantiate("CapsuleGreen", GameManagerr.Instance.GeneratePlayers(PhotonNetwork.LocalPlayer.GetTeamID()), Quaternion.identity, 0, new object[] { photonView.ViewID });
                break;
            case 3:
                controller = PhotonNetwork.Instantiate("CapsuleYellow", GameManagerr.Instance.GeneratePlayers(PhotonNetwork.LocalPlayer.GetTeamID()), Quaternion.identity, 0, new object[] { photonView.ViewID });
                break;

        }
        //controller = PhotonNetwork.Instantiate("Capsule", Vector3.up, Quaternion.identity, 0, new object[] { photonView.ViewID });
        //controller.transform.position = GameManagerr.Instance.GeneratePlayers(controller);
        //GameManagerr.Instance.GeneratePlayers(controller);
    }
    public void CreateDiedController()
    {
        switch (PhotonNetwork.LocalPlayer.GetTeamID())
        {
            case 0:
                controller = PhotonNetwork.Instantiate("CapsuleBlue", GameManagerr.Instance.GeneratePlayers(PhotonNetwork.LocalPlayer.GetTeamID()), Quaternion.identity, 0, new object[] { photonView.ViewID });
                break;
            case 1:
                controller = PhotonNetwork.Instantiate("CapsuleRed", GameManagerr.Instance.GeneratePlayers(PhotonNetwork.LocalPlayer.GetTeamID()), Quaternion.identity, 0, new object[] { photonView.ViewID });
                break;
            case 2:
                controller = PhotonNetwork.Instantiate("CapsuleGreen", GameManagerr.Instance.GeneratePlayers(PhotonNetwork.LocalPlayer.GetTeamID()), Quaternion.identity, 0, new object[] { photonView.ViewID });
                break;
            case 3:
                controller = PhotonNetwork.Instantiate("CapsuleYellow", GameManagerr.Instance.GeneratePlayers(PhotonNetwork.LocalPlayer.GetTeamID()), Quaternion.identity, 0, new object[] { photonView.ViewID });
                break;
        }
        //controller = PhotonNetwork.Instantiate("Capsule", GameManagerr.Instance.GeneratePlayers(controller), Quaternion.identity, 0, new object[] { photonView.ViewID });
        controller.GetComponent<PlayerAttribute>().isDead = true;
        controller.GetComponentInChildren<PersonalCanvas>().DeadSectionOn();
        StartCoroutine(Respawn(controller));
    }
    IEnumerator Respawn(GameObject controller)
    {
        yield return new WaitForSeconds(controller.GetComponentInChildren<PersonalCanvas>().respawnRemainingTime);
        controller.GetComponent<PlayerAttribute>().isDead = false;
        controller.GetComponentInChildren<PersonalCanvas>().DeadSectionOff(); // Deactivate player dead canvas.
    }
}
