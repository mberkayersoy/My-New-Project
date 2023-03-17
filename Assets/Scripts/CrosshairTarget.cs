using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CrosshairTarget : MonoBehaviour
{
    public Camera mainCamera;
    Ray ray;
    RaycastHit hitInfo;
    //public PhotonView pw;
    // Start is called before the first frame update
    void Start()
    {
        //mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        ray.origin = mainCamera.transform.position;
        ray.direction = mainCamera.transform.forward;
        Physics.Raycast(ray, out hitInfo);
        transform.position = hitInfo.point;

    }
}
