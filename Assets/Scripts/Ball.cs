using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public GameObject nextBall;
    public int nextBallID;
    [SerializeField] private Rigidbody rb;
    void Start()
    {
        var randomVector = Random.insideUnitSphere * 35f;
        rb = GetComponent<Rigidbody>();
        //rb.AddForce(randomVector, ForceMode.Impulse);
    }
    public void Split(int team)
    {
        var randomVector1 = Random.insideUnitSphere * 8f;
        var randomVector2 = Random.insideUnitSphere * 8f;
        GameObject ball1;
        GameObject ball2;

        string ballName = "Ball" + nextBallID;

        if (nextBall != null)
        {
            switch (team)
            {
                case 0:
                    ScoreBoard.Instance.SetScore(team, transform.localScale.x);
                    ball1 = Instantiate(nextBall, rb.position + Vector3.right / 4f, Quaternion.identity);
                    ball1.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.blue);
                    ball2 = Instantiate(nextBall, rb.position + Vector3.left / 4f, Quaternion.identity);
                    ball2.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.blue);
                    ball1.GetComponent<Ball>().GetComponent<Rigidbody>().AddForce(randomVector1, ForceMode.Impulse);
                    ball2.GetComponent<Ball>().GetComponent<Rigidbody>().AddForce(randomVector2, ForceMode.Impulse);
                    nextBallID++;
                    break;

                case 1:
                    ScoreBoard.Instance.SetScore(team, transform.localScale.x);
                    ball1 = Instantiate(nextBall, rb.position + Vector3.right / 4f, Quaternion.identity);
                    ball1.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.red);
                    ball2 = Instantiate(nextBall, rb.position + Vector3.left / 4f, Quaternion.identity);
                    ball2.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.red);
                    ball1.GetComponent<Ball>().GetComponent<Rigidbody>().AddForce(randomVector1, ForceMode.Impulse);
                    ball2.GetComponent<Ball>().GetComponent<Rigidbody>().AddForce(randomVector2, ForceMode.Impulse);
                    nextBallID++;
                    break;

                case 2:
                    ScoreBoard.Instance.SetScore(team, transform.localScale.x);
                    ball1 = Instantiate(nextBall, rb.position + Vector3.right / 4f, Quaternion.identity);
                    ball1.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.yellow);
                    ball2 = Instantiate(nextBall, rb.position + Vector3.left / 4f, Quaternion.identity);
                    ball2.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.yellow);
                    ball1.GetComponent<Ball>().GetComponent<Rigidbody>().AddForce(randomVector1, ForceMode.Impulse);
                    ball2.GetComponent<Ball>().GetComponent<Rigidbody>().AddForce(randomVector2, ForceMode.Impulse);
                    nextBallID++;
                    break;

                case 3:
                    ScoreBoard.Instance.SetScore(team, transform.localScale.x);
                    ball1 = Instantiate(nextBall, rb.position + Vector3.right / 4f, Quaternion.identity);
                    ball1.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.green);
                    ball2 = Instantiate(nextBall, rb.position + Vector3.left / 4f, Quaternion.identity);
                    ball2.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.green);
                    ball1.GetComponent<Ball>().GetComponent<Rigidbody>().AddForce(randomVector1, ForceMode.Impulse);
                    ball2.GetComponent<Ball>().GetComponent<Rigidbody>().AddForce(randomVector2, ForceMode.Impulse);
                    nextBallID++;
                    break;
            }
        }
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.gameObject.name);
    }

}
