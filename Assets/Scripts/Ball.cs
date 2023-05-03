using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Ball : MonoBehaviourPunCallbacks
{
    public GameObject nextBall;
    public PhotonView pw;
    public TeamID ballTeamID = 0;
    private Rigidbody rb;
    public string nextBallID;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        pw = GetComponent<PhotonView>();
    }

    private void Start()
    {
       // Debug.Log("Ball start: " + ballTeamID);
        GetComponent<MeshRenderer>().material.color = TeamColor.GetTeamColor(ballTeamID);
    }

    [PunRPC]
    public void Split(int shooterTeamID, Vector3 rayOrigin)
    {
        //Debug.Log("SPLITTT");
        if (string.IsNullOrEmpty(nextBallID))
        {
            //GameManagerr.Instance.RemoveBallList(gameObject.GetComponent<Ball>());
            GameManagerr.Instance.pw.RPC("RemoveBallList", RpcTarget.All, gameObject.GetPhotonView().ViewID);
            ScoreBoard.Instance.SetScore((TeamID)shooterTeamID, transform.localScale.x);
            GameManagerr.Instance.pw.RPC("CheckGameEnd", RpcTarget.All);
            //ScoreBoard.Instance.pw.RPC("SetScore", RpcTarget.OthersBuffered, teamNumber, transform.localScale.x);

            if (PhotonNetwork.IsMasterClient)
                PhotonNetwork.Destroy(gameObject);
            
            return;
        }

        Vector3 forces = rayOrigin.normalized * 30;
        rb.AddForce(-forces, ForceMode.Impulse);

        //Debug.Log("Teamnumber: " + (TeamID)shooterTeamID);
        GenerateBallChilds((TeamID)shooterTeamID);
        //GameManagerr.Instance.RemoveBallList(gameObject.GetComponent<Ball>());
        GameManagerr.Instance.pw.RPC("RemoveBallList", RpcTarget.All, gameObject.GetPhotonView().ViewID);
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

    [PunRPC]
    void SetBallTeamID(TeamID teamID)
    {
        ballTeamID = teamID;
        GetComponent<MeshRenderer>().material.color = TeamColor.GetTeamColor(teamID);
    }

    [PunRPC]
    public void GenerateBallChilds(TeamID shooterTeamID)
    {
        //Debug.Log("GENERATE BALL CHILDS");
        GameObject ball1;
        GameObject ball2;

        ScoreBoard.Instance.SetScore(shooterTeamID, transform.localScale.x);
        //ScoreBoard.Instance.pw.RPC("SetScore", RpcTarget.AllBuffered, teamID, transform.localScale.x);
        if (!PhotonNetwork.IsMasterClient) return;
        //ball1 = Instantiate(nextBall, rb.position + new Vector3((transform.localScale.x / 4), 0, 0), Quaternion.identity);
        ball1 = PhotonNetwork.InstantiateRoomObject(nextBallID, rb.position + new Vector3(transform.localScale.x / 4, 0, 0), Quaternion.identity, 0);
        ball1.GetComponent<Ball>().ballTeamID = shooterTeamID;
        PhotonView p1 = ball1.GetComponent<PhotonView>();
        p1.RPC("SetBallTeamID", RpcTarget.AllViaServer, (int)shooterTeamID);
 
        //ball2 = Instantiate(nextBall, rb.position + new Vector3(0, 0, (transform.localScale.x / 4)), Quaternion.identity);
        ball2 = PhotonNetwork.InstantiateRoomObject(nextBallID, rb.position + new Vector3(0, 0, transform.localScale.x / 4), Quaternion.identity, 0);
        ball2.GetComponent<Ball>().ballTeamID = shooterTeamID;
        PhotonView p2 = ball2.GetComponent<PhotonView>();
        p2.RPC("SetBallTeamID", RpcTarget.AllViaServer, (int)shooterTeamID);

        ball1.GetComponent<Rigidbody>().velocity = Quaternion.Euler(30, 30, 30) * rb.velocity;
        ball2.GetComponent<Rigidbody>().velocity = Quaternion.Euler(-30, -30, -30) * rb.velocity;

        GameManagerr.Instance.pw.RPC("AddBallList", RpcTarget.All, ball1.GetPhotonView().ViewID);
        GameManagerr.Instance.pw.RPC("AddBallList", RpcTarget.All, ball2.GetPhotonView().ViewID);
       // GameManagerr.Instance.AddBallList(ball1.GetComponent<Ball>(), ball2.GetComponent<Ball>());
    }

    [PunRPC]
    public void DestroyBall(int ballViewID)
    {
        PhotonView ballPV = PhotonView.Find(ballViewID);
        if (ballPV != null && ballPV.IsMine)
        {
            PhotonNetwork.Destroy(ballPV.gameObject);
        }
    }
}


