using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPController : MonoBehaviour
{
    public float speed = 1.0f;
    public float sensitivity = 1.0f;
    public GameObject cam;

    float moveHorizontal;
    float moveVertical;
    float rotX;
    float rotY;

    bool PlayerMovementEnabled;         // the player is only able to move when the attribute is set to true

    // Use this for initialization
    void Start()
    {
        PlayerMovementEnabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerMovementEnabled)
        {
            moveHorizontal = Input.GetAxis("Horizontal") * speed / 2;
            moveVertical = Input.GetAxis("Vertical") * speed;

            moveHorizontal *= Time.deltaTime;
            moveVertical *= Time.deltaTime;

            rotX = Input.GetAxis("HorizontalTurn");
            rotY = Input.GetAxis("VerticalTurn");
            if (rotX < 0.001 && rotX > -0.001) rotX = 0;

            float angle = rotY * sensitivity * Time.deltaTime;
            float preditAngle = cam.transform.rotation.eulerAngles.x + angle;
            if (preditAngle > 180) preditAngle -= 360;
            //Debug.Log(preditAngle);
            if (Mathf.Abs(preditAngle) < 50)
                cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(preditAngle, 0f, 0f)), Mathf.Abs(rotY) * sensitivity * Time.fixedDeltaTime);

            //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, h, 0f)), Mathf.Abs(h) * smoothing * Time.fixedDeltaTime);
            transform.Rotate(0, rotX * sensitivity * Time.fixedDeltaTime, 0);

            transform.Translate(moveHorizontal, 0, moveVertical);
        }
    }
}