using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    public Transform camPos;
    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 targetPosition = target.TransformPoint(new Vector3(0, 0, 0));
        //transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        //transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, .01f);
    }

    void LateUpdate()
    {
        Vector3 targetPosition = camPos.position;
        targetPosition.y = transform.position.y;

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

        Quaternion targetRotation = Quaternion.LookRotation(camPos.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smoothTime * Time.deltaTime);
    }
}
