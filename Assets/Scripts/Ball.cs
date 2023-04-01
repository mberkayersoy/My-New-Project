using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public GameObject nextBall;
    public TeamID ballTeamID;
    private Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        GetComponent<MeshRenderer>().material.color = TeamColor.GetTeamColor(ballTeamID);
    }

    public void MoveTheBall()
    {
        gameObject.SetActive(true);
        rb.isKinematic = false;
        var randomVector = Random.insideUnitSphere * 25f;
        rb.AddForce(randomVector, ForceMode.Impulse);
    }
    public void Split(RaycastWeapon raycastWeapon, Vector3 rayOrigin)
    {
        if(nextBall == null)
        {
            GameManager.Instance.RemoveBallList(gameObject.GetComponent<Ball>());
            ScoreBoard.Instance.SetScore((TeamID)raycastWeapon.teamNumber, transform.localScale.x);
            Destroy(gameObject);
            return;
        }

        Vector3 forces = rayOrigin.normalized * 30;
        rb.AddForce(-forces, ForceMode.Impulse);
        //Debug.DrawLine(rb.position, rb.velocity + transform.position, Color.black, 2.0f);
        //Debug.DrawLine(transform.position, Quaternion.Euler(45, 45, 45) * rb.velocity + transform.position, Color.red, 2.0f);
        //Debug.DrawLine(transform.position, Quaternion.Euler(-45, -45, -45) * rb.velocity + transform.position, Color.blue, 2.0f);
        GenerateBallChilds((TeamID)raycastWeapon.teamNumber);
        GameManager.Instance.RemoveBallList(gameObject.GetComponent<Ball>());
        Destroy(gameObject);
    }

    public void GenerateBallChilds(TeamID teamID)
    {
        GameObject ball1;
        GameObject ball2;
        ScoreBoard.Instance.SetScore(teamID, transform.localScale.x);

        ball1 = Instantiate(nextBall, rb.position + new Vector3((transform.localScale.x / 4), 0, 0), Quaternion.identity);
        //ball1.GetComponent<MeshRenderer>().material.SetColor("_Color", TeamColor.GetTeamColor(teamID));
        ball1.GetComponent<Ball>().ballTeamID = teamID;

        ball2 = Instantiate(nextBall, rb.position + new Vector3(0, 0, (transform.localScale.x / 4)), Quaternion.identity);
        //ball2.GetComponent<MeshRenderer>().material.SetColor("_Color", TeamColor.GetTeamColor(teamID));
        ball2.GetComponent<Ball>().ballTeamID = teamID;

        ball1.GetComponent<Ball>().GetComponent<Rigidbody>().velocity = Quaternion.Euler(30, 30, 30) * rb.velocity;
        ball2.GetComponent<Ball>().GetComponent<Rigidbody>().velocity = Quaternion.Euler(-30, -30, -30) * rb.velocity;

        GameManager.Instance.AddBallList(ball1.GetComponent<Ball>(), ball2.GetComponent<Ball>());
    }
    //private void Update()
    //{
    //    Debug.DrawLine(rb.position, rb.velocity + transform.position, Color.green, 0.1f);
    //}
}
