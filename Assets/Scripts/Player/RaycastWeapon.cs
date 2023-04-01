using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RaycastWeapon : MonoBehaviour
{
    [Header("VFX Variables")]

    public ParticleSystem[] muzzleFlash;
    public ParticleSystem hitEffect;
    public TrailRenderer tracerEffect;
    public bool isFiring = false;

    [Space]
    
    [Header("Tranform Variables")]
    public Transform raycastOrigin;
    public Transform raycastDestination;


    public int teamNumber;
    Ray ray;
    RaycastHit hitInfo;

    private void Start()
    {
        teamNumber = GetComponentInParent<Player>().teamID;
    }
    public void StartFiring()
    {
        isFiring = true;
        foreach (var particle in muzzleFlash)
        {
            particle.Emit(1);
        }

        ray.origin = raycastOrigin.position;
        ray.direction = raycastDestination.position - raycastOrigin.position;


        var tracer = Instantiate(tracerEffect, ray.origin, Quaternion.identity);
        tracer.AddPosition(ray.origin);

        if (Physics.Raycast(ray, out hitInfo))
        {
            //Debug.DrawLine(ray.origin, hitInfo.point, Color.green, 2.0f);
            hitEffect.transform.position = hitInfo.point;
            hitEffect.transform.forward = hitInfo.normal;
            hitEffect.Emit(1);
            tracer.transform.position = hitInfo.point;

            if (hitInfo.transform.CompareTag("Ball"))
            {
                hitInfo.transform.GetComponent<Ball>().Split(this, ray.origin);
               // Debug.Log(hitInfo.distance);
            }
        }
   
    }
    public void StopFiring()
    {
        isFiring = false;
    }
}
