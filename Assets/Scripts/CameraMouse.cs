using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMouse : MonoBehaviour
{
    public Vector2 turn;
    public float sens = .5f;
    public Vector3 deltaMove;
    public float speed = 1;
    public GameObject mover;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {

        turn.x += Input.GetAxis("Mouse X") * sens;
        turn.y += Input.GetAxis("Mouse Y") * sens;
        mover.transform.localRotation = Quaternion.Euler(0, turn.x, 0);
        transform.localRotation = Quaternion.Euler(-turn.y, 0, 0);
    }
}
