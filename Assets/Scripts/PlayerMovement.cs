using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    public float speed = 1f;

    //camera controller
    private float xRotation = 0f;
    public float mouseSensivity = 100f;

    //jump and gravity 
    private Vector3 velocity;
    private float gravity = -9.81f;

    public Transform groundChecker;
    public float groundCheckerRadius;
    public LayerMask obstacleLayer;
    private bool isGround;

    public float jumpHeight = 0.1f;
    public float gravityDivide = 100f;
    public float jumpSpeed = 100;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        //Cursor
        Cursor.visible = false; //mouseu Unity icerisinde gizleyecek 
        Cursor.lockState = CursorLockMode.Locked; //mouseu ne kadar hareket ettirirsen ettir daima orta noktada kalacak
    }

    private void Update()
    {
        //Check character is grounded
        isGround = Physics.CheckSphere(groundChecker.position, groundCheckerRadius, obstacleLayer);


        //movement
        Vector3 moveInputs = Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward; //transform yerine Vector3. komutu da yazilabilir fakat unity icerisinde bazi komplikasyonlara sebebiyet verdiginden transform cagirmak daha saglikli
        Vector3 moveVelocity = moveInputs * Time.deltaTime * speed;

        controller.Move(moveVelocity);

        //camera controller
        transform.Rotate(0, Input.GetAxis("Mouse X") * Time.deltaTime * mouseSensivity, 0);
        xRotation -= Input.GetAxis("Mouse Y") * Time.deltaTime * mouseSensivity;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); //kamera acisinin maks ve min degerlerini kapsar

        Camera.main.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        //jump and gravity
        if (!isGround)
        {
            velocity.y += gravity * Time.deltaTime / gravityDivide;
            speed = jumpSpeed;
        }
        else
        {
            velocity.y = -0.05f;
            speed = 10;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity / gravityDivide);
        }
        controller.Move(velocity);
    }
}