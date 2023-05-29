using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    /// <summary>
    /// Camera for lookat target
    /// </summary>
    Transform cam;


    private void Start()
    {
        cam = Camera.main.transform;
    }
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled
    /// </summary>
    void LateUpdate()
    {
        transform.LookAt(transform.position + cam.rotation * Vector3.forward,
            cam.rotation * Vector3.up);
        //// Try find camera in scene
        //if (cam == null) cam = FindObjectOfType<Camera>();
        //if (cam == null) return;

        //transform.LookAt(cam.transform);
        //transform.Rotate(Vector3.up * 180);
    }
}
