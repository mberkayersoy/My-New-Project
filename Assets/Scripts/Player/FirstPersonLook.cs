using UnityEngine;
using Photon.Pun;
public class FirstPersonLook : MonoBehaviour
{
    [SerializeField]
    public Transform character;
    public Transform camPos;
    public float sensitivity = 2;
    public float smoothing = 1.5f;
    public float camMinRot = -25f;
    public float camMinRotOnAir = -40f;
    public float camMaxRot = 50f;
    Vector2 velocity;
    Vector2 frameVelocity;

    private Vector3 velocityy = Vector3.zero;


    void Start()
    {
        // Lock the mouse cursor to the game screen.
        Cursor.lockState = CursorLockMode.Locked;
        if (!character.gameObject.GetComponent<PhotonView>().IsMine)
        {
            GetComponent<Camera>().enabled = false;
            GetComponent<FirstPersonLook>().enabled = false;
            GetComponent<AudioListener>().enabled = false;
        }
            
    }

    void Update()
    {
        CamRotation();
    }
    private void CamRotation()
    {
        float targetCamMinRot;
        if (!character.GetComponent<FirstPersonMovement>().Grounded)
        {
            targetCamMinRot = camMinRotOnAir;
        }
        else
        {
            targetCamMinRot = -25f;
        }

        camMinRot = Mathf.Lerp(camMinRot, targetCamMinRot, 0.1f);
        // Get smooth velocity.
        Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        Vector2 rawFrameVelocity = Vector2.Scale(mouseDelta, Vector2.one * sensitivity);
        frameVelocity = Vector2.Lerp(frameVelocity, rawFrameVelocity, 1 / smoothing);
        velocity += frameVelocity;
        velocity.y = Mathf.Clamp(velocity.y, camMinRot, camMaxRot);
        character.localRotation = Quaternion.AngleAxis(velocity.x, Vector3.up);
        transform.rotation = Quaternion.Euler(-velocity.y, character.eulerAngles.y, 0);
    }

    private void LateUpdate()
    {
        CamFollow();
    }
    void CamFollow()
    {
        Vector3 targetPosition = camPos.position;
        //targetPosition.y = transform.position.y;

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocityy, 0);

        //Quaternion targetRotation = Quaternion.LookRotation(camPos.position - transform.position);
        //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0 * Time.deltaTime);
    }

}
