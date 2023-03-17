using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using Photon.Pun;

public class CharacterAiming : MonoBehaviour
{
    public float turnSpeed = 15f;
    public float aimDuration = 0.2f;
    public Rig aimLayer;

    public Camera mainCamera;
    RaycastWeapon weapon;
    PhotonView pw;


    // Start is called before the first frame update
    void Start()
    {
        pw = GetComponent<PhotonView>();
        Cursor.visible = false;
        //mainCamera = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
        weapon = GetComponentInChildren<RaycastWeapon>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (pw.IsMine)
        {
            //float yawCamera = mainCamera.transform.rotation.eulerAngles.y;
            //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, yawCamera, 0), turnSpeed * Time.fixedDeltaTime);
        }

    }

    private void LateUpdate()
    {
        if (pw.IsMine)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                weapon.StartFiring();
            }
            if (Input.GetButtonUp("Fire1"))
            {
                weapon.StopFiring();
            }
        }
    }
}
