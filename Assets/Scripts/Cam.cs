using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    public Transform playerTransform;

    [Range(1, 10)]

    [SerializeField] private float smootSpeed = 5f;
    [SerializeField] private Vector3 offSet;
    [SerializeField] private Vector3 minValue, maxValue;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
        updateCameraPos();
    }

    private void updateCameraPos()
    {
        Vector3 desiredPos = playerTransform.TransformPoint(offSet);

        Vector3 clampPosition = new Vector3(
            Mathf.Clamp(desiredPos.x, minValue.x, maxValue.x),
            Mathf.Clamp(desiredPos.y, minValue.y, maxValue.y),
            Mathf.Clamp(desiredPos.z, minValue.z, maxValue.z));

        Vector3 smoothPos = Vector3.Lerp(
            transform.position,
            clampPosition,
            smootSpeed * Time.deltaTime);

        transform.position = smoothPos;
        transform.LookAt(playerTransform);
    }
}
