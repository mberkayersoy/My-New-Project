using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PickUpStop : MonoBehaviour
{
    Rigidbody rb;
    private string rotationID;
    private string yoyoID;
    void Start()
    {
        DOTween.Init();
        rb = GetComponentInParent<Rigidbody>();

        // I give a unique ID to each dotween object. In this way, DOTween.Kill does not affect other objects when it runs.
        rotationID = "Rotation" + GetInstanceID().ToString();
        yoyoID = "Yoyo" + GetInstanceID().ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Platform"))
        {
            //rb.velocity = Vector3.zero;
            rb.isKinematic = true;
            gameObject.GetComponent<Collider>().enabled = false;
            RotateAbility();
        }
    }

    void RotateAbility()
    {

        if (gameObject.activeInHierarchy)
        {

            transform.parent.DORotate(new Vector3(0f, 360f, 0f), 2f, RotateMode.LocalAxisAdd)
              .SetEase(Ease.Linear)
              .SetLoops(-1, LoopType.Restart)
              .SetId(rotationID);

            transform.parent.DOMoveY(1.5f,1)
                .SetEase(Ease.InOutQuad)
                .SetLoops(-1, LoopType.Yoyo)
                .SetId(yoyoID);
        }
    }


    // This method execute when gameobject will destroy. Prevent the dotween bugs/errors.
    private void OnDestroy()
    {
        DOTween.Kill(rotationID);
        DOTween.Kill(yoyoID);
    }
}
