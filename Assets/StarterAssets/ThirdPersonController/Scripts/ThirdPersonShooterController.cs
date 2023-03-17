using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using Photon.Pun;
public class ThirdPersonShooterController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
    [SerializeField] private float normalSensitivity;
    [SerializeField] private float aimSensitivity;
    [SerializeField] private LayerMask aimColliderLayerMask;
    [SerializeField] private Transform pfBulletProjectile;
    [SerializeField] private Transform spawnBulletPosition;
    [SerializeField] private Transform vfxHitGreen;
    [SerializeField] private Transform vfxHitRed;
    [SerializeField] public int teamNumber;

    private ThirdPersonController thirdPersonController;
    private StarterAssetsInputs starterAssetsInputs;
    private Animator animator;
    PhotonView pw;

    private void Awake()
    {
        Cursor.visible = false;
        thirdPersonController = GetComponent<ThirdPersonController>();
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        animator = GetComponent<Animator>();
        pw = GetComponent<PhotonView>();

    }
    private void Update()
    {
        if (pw.IsMine)
        {
            Vector3 mouseWorldPosition = Vector3.zero;

            Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
            Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
            Transform hitTransform = null;

            if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
            {
                mouseWorldPosition = raycastHit.point;
                hitTransform = raycastHit.transform;
            }

            if (starterAssetsInputs.aim)
            {
                aimVirtualCamera.gameObject.SetActive(true);
                thirdPersonController.SetSensitivity(aimSensitivity);
                thirdPersonController.SetRotateOnMove(false);

                Vector3 worldAimTarget = mouseWorldPosition;
                worldAimTarget.y = transform.position.y;
                Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

                transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
            }
            else
            {
                aimVirtualCamera.gameObject.SetActive(false);
                thirdPersonController.SetSensitivity(normalSensitivity);
                thirdPersonController.SetRotateOnMove(true);
            }

            if (Input.GetButtonDown("Fire1"))
            {
                if (starterAssetsInputs.shoot)
                {
                    if (hitTransform != null)
                    {
                        //hit someting
                        if (hitTransform.gameObject.tag == "Ball")
                        {
                            Instantiate(vfxHitGreen, mouseWorldPosition, Quaternion.identity);
                            hitTransform.gameObject.GetComponent<Ball>().Split(teamNumber);
                        }
                        else
                        {
                            //hit someting else
                            //Debug.Log(hitTransform.gameObject.name);
                            Instantiate(vfxHitRed, mouseWorldPosition, Quaternion.identity);
                        }
                    }
                    //Vector3 aimDir = (mouseWorldPosition - spawnBulletPosition.position).normalized;
                    //Instantiate(pfBulletProjectile, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
                    //starterAssetsInputs.shoot = false;
                }
            }
        }
        
        
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (pw.IsMine)
        {
            if (hit.gameObject.tag == "Ball")
            {
                Debug.Log("false");
                gameObject.SetActive(false);
                Invoke("Generate", 2);
            }
        }

    }

    public void Generate()
    {
        transform.position = SpawnManager.Instance.SpawnPlayer(teamNumber);
        gameObject.SetActive(true);
        gameObject.GetComponent<CharacterController>().SimpleMove(Vector3.zero);
    }   
}
