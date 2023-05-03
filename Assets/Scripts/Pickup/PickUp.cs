using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviourPunCallbacks
{
    public Ability ability;
    private int ownerID;

    private void Start()
    {
        GetComponent<Rigidbody>().AddForce(Vector3.down * 20f, ForceMode.Impulse);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ownerID = other.GetComponent<PhotonView>().Owner.ActorNumber;
            other.GetComponent<PlayerAttribute>().SetAbility(ability);
            if(PhotonNetwork.IsMasterClient)
            PhotonNetwork.Destroy(gameObject);
            //GetComponent<PhotonView>().RPC("DestroySphere", RpcTarget.AllBuffered);
        }
    }
}
