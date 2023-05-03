using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using Photon.Pun;

public class CharacterAiming : MonoBehaviourPunCallbacks
{
    public float turnSpeed = 15f;
    public float aimDuration = 0.2f;
    public Rig aimLayer;
    RaycastWeapon weapon;
    public PhotonView pw;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        weapon = GetComponentInChildren<RaycastWeapon>();
        pw = GetComponent<PhotonView>();
    }

    private void LateUpdate()
    {
        if(GetComponent<PhotonView>().IsMine) 
        {
            if (!GetComponent<PlayerAttribute>().isDead)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    //weapon.pw.RPC("StartFiring", RpcTarget.All);
                    weapon.StartFiring();
                }
                if (Input.GetButtonUp("Fire1"))
                {
                    //weapon.pw.RPC("StartFiring", RpcTarget.All);
                    weapon.StopFiring();
                }
            }
        }
    }
}
