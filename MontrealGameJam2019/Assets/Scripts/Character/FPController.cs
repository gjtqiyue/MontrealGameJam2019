using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPController : MonoBehaviour
{
    public float speed = 1.0f;
    public float sensitivity = 1.0f;
    public GameObject cam;
    public Animator anim;
    public Animator camAnim;

    float moveHorizontal;
    float moveVertical;
    float rotX;
    float rotY;
    bool turnLock;
    bool cameraSwitch = false;
    Quaternion curRotation;

    public bool PlayerMovementEnabled = false;         // the player is only able to move when the attribute is set to true

    // Use this for initialization
    void Start()
    {
        anim.SetBool("PlayerControlling", false);
    }
    

    // Update is called once per frame
    void Update()
    {
        if (PlayerMovementEnabled)
        {
            //set the animator
            anim.SetBool("PlayerControlling", true);
            moveHorizontal = Input.GetAxis("Horizontal");
            moveVertical = Input.GetAxis("Vertical");
            anim.SetFloat("MovementSpeed", moveVertical);

            moveHorizontal *= speed / 2 * Time.deltaTime;
            moveVertical *= speed * Time.deltaTime;

            rotX = Input.GetAxis("HorizontalTurn");
            rotY = Input.GetAxis("VerticalTurn");
            if (rotX < 0.01 && rotX > -0.01)
            {
                rotX = 0;
            }

            float angle = rotY * sensitivity * Time.deltaTime;
            float preditAngle = cam.transform.rotation.eulerAngles.x + angle;
            if (preditAngle > 180) preditAngle -= 360;
            //Debug.Log(preditAngle);
            if (Mathf.Abs(preditAngle) < 50)
                cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(preditAngle, 0f, 0f)), Mathf.Abs(rotY) * sensitivity * Time.fixedDeltaTime);

            //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, h, 0f)), Mathf.Abs(h) * smoothing * Time.fixedDeltaTime);
            transform.Rotate(0, rotX * sensitivity * Time.fixedDeltaTime, 0);

            transform.Translate(moveHorizontal, 0, moveVertical);

            //if (Input.GetButtonDown("XboxR1"))
            //{
            //    // fast turning
            //    StartCoroutine(TurnAnimation());
            //}
        }
    }

    //IEnumerator TurnAnimation()
    //{
    //    turnLock = true;
    //    Quaternion target = Quaternion.LookRotation(-transform.forward);
    //    Debug.Log(Mathf.Abs(Quaternion.Angle(transform.rotation, target)));
    //    while (Mathf.Abs(Quaternion.Angle(transform.rotation, target)) > 1)
    //    {
    //        transform.rotation = Quaternion.Lerp(transform.rotation, target, 0.2f);
    //        yield return null;
    //    }
    //    turnLock = false;
    //}

    public void PickUp()
    {
        SwitchCamera();

        anim.SetTrigger("PickUpTrigger");
        camAnim.SetTrigger("PickUpTrigger");
    }

    public void SwitchCamera()
    {
        // switch main camera to the other one
        transform.GetChild(0).GetComponent<Camera>().enabled = cameraSwitch;
        transform.GetChild(1).GetComponent<Camera>().enabled = !cameraSwitch;
        cameraSwitch = !cameraSwitch;
    }
}