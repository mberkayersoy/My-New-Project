using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 defaultDistance = new Vector3(3, 1, -4);
    [SerializeField] float distanceDamp = 10f;
    [SerializeField] float rotationDamp = 10f;
    Vector3 velocity = Vector3.one;

    private void LateUpdate()
    {
        SmoothFollow();
    }

    private void SmoothFollow()
    {
        Vector3 toPos = target.position + (target.rotation * defaultDistance);
        Vector3 curPos = Vector3.SmoothDamp(transform.position, toPos, ref velocity, distanceDamp);
        transform.position = curPos;
        transform.LookAt(target, target.up);
    }
}
