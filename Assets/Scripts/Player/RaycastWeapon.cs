using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RaycastWeapon : MonoBehaviourPunCallbacks
{
    [Header("VFX Variables")]
    public ParticleSystem[] muzzleFlash;
    public ParticleSystem hitEffect;
    public TrailRenderer tracerEffect;
    public bool isFiring = false;

    [Space]
    [Header("Transform Variables")]
    public Transform raycastOrigin;
    public Transform raycastDestination;
    public int teamNumber;
    Ray ray;
    RaycastHit hitInfo;
    public PhotonView pw;

    private void Start()
    {
        teamNumber = PhotonNetwork.LocalPlayer.GetTeamID();
        pw = GetComponent<PhotonView>();

        foreach (var effect in muzzleFlash)
        {
            ParticleSystem.MainModule mainModule = effect.main;
            mainModule.startColor = TeamColor.GetTeamColor((TeamID)teamNumber);
        }

        tracerEffect.startColor = TeamColor.GetTeamColor((TeamID)teamNumber);
    }

    [PunRPC]
    public void StartFiring()
    {
        isFiring = true;
        pw.RPC("EmitMuzzleFlash", RpcTarget.All);

        ray.origin = raycastOrigin.position;
        ray.direction = raycastDestination.position - raycastOrigin.position;

        var tracer = PhotonNetwork.Instantiate("BulletTracer", ray.origin, Quaternion.identity);
        tracer.GetComponent<TrailRenderer>().startColor = TeamColor.GetTeamColor((TeamID)teamNumber);
        tracer.GetComponent<TrailRenderer>().AddPosition(ray.origin);

        Vector3 RayOrigin = ray.origin;
        if (Physics.Raycast(ray, out hitInfo))
        {
            pw.RPC("EmitHitEffect", RpcTarget.All, hitInfo.point, hitInfo.normal);

            tracer.transform.position = hitInfo.point;

            if (hitInfo.transform.CompareTag("Ball"))
            {
                PhotonView ballPhotonView = hitInfo.collider.GetComponent<PhotonView>();
                // Call the Split() method on all clients using Photon RPC
                ballPhotonView.RPC("Split", RpcTarget.AllViaServer, teamNumber, transform.forward);
                GameManagerr.Instance.pw.RPC("GenerateAbility", RpcTarget.All, hitInfo.point);
                //Debug.Log("raycastweapon: " + PhotonNetwork.LocalPlayer.GetTeamID());
                //hitInfo.transform.GetComponent<Ball>().pw.RPC("Split", RpcTarget.All, PhotonNetwork.LocalPlayer.GetTeamID(), RayOrigin);
                //hitInfo.transform.GetComponent<Ball>().Split(teamNumber, RayOrigin);
            }
        }
    }

    [PunRPC]
    public void StopFiring()
    {
        isFiring = false;
    }

    [PunRPC]
    void EmitMuzzleFlash()
    {
        foreach (var particle in muzzleFlash)
        {
            particle.Emit(1);
        }
    }

    [PunRPC]
    void EmitHitEffect(Vector3 hitPosition, Vector3 hitNormal)
    {
        hitEffect.transform.position = hitPosition;
        hitEffect.transform.forward = hitNormal;
        hitEffect.Emit(1);
    }
}
