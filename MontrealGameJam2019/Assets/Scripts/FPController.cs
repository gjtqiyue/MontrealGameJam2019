using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPController : MonoBehaviour
{

    public float speed = 1.0f;
    public float height = 1.0f;
    public float sensitivity = 1.0f;
    public GameObject cam;

    float moveHorizontal;
    float moveVertical;
    float jump;
    float rotX;
    float rotY;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        moveHorizontal = Input.GetAxis("Horizontal") * speed;
        moveVertical = Input.GetAxis("Vertical") * speed;
        jump = Input.GetAxis("Jump") * height;
        moveHorizontal *= Time.deltaTime;
        moveVertical *= Time.deltaTime;
        jump *= Time.deltaTime;

        rotX = Input.GetAxis("Mouse X") * sensitivity;
        rotY = Input.GetAxis("Mouse Y") * sensitivity;
        rotX *= Time.deltaTime;
        rotY *= Time.deltaTime;

        transform.Translate(moveHorizontal, jump, moveVertical);
        transform.Rotate(0, rotX, 0);
        cam.transform.Rotate(-rotY, 0, 0);
    }
}