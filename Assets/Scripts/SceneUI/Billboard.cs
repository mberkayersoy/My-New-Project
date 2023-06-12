using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    /// <summary>
    /// Camera for lookat target
    /// </summary>
    public Transform cam;

    void LateUpdate()
    {
        if (cam != null)
        {
            transform.LookAt(transform.position + cam.rotation * Vector3.forward,
          cam.rotation * Vector3.up);
        } else
        {
            cam = Camera.main.transform;
        }

        //// Try find camera in scene
        //if (cam == null) cam = FindObjectOfType<Camera>();
        //if (cam == null) return;

        //transform.LookAt(cam.transform);
        //transform.Rotate(Vector3.up * 180);
    }
}
