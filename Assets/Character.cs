using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Character : MonoBehaviour
{
    //vars
    private CharacterController controller;
    private Vector2 moveInput;
    public float speed;

    //player physical speed
    private Vector3 playerVel;
    private bool grounded;
    public float gravity = -9.8f;
    public float jumpForce = 2f;

    //player camera speed
    public Camera cam;
    private Vector2 lookPos;
    private float xRotation = 0f;
    public float xSens = 30f;
    public float ySens = 30f;

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        jump();
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        lookPos = context.ReadValue<Vector2>();
    }
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        // transform.position = player.transform.position + new Vector3(0, 1, -5);
        grounded = controller.isGrounded;
        movePlayer();
        playerLook();
    }

    public void movePlayer()
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = moveInput.x;
        moveDirection.z = moveInput.y;
        controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);

        playerVel.y += gravity * Time.deltaTime;
        if(grounded && playerVel.y < 0)
        {
            playerVel.y = -2f;
        }
        controller.Move(playerVel * Time.deltaTime);
    }

    public void jump()
    {
        if (grounded)
        {
            playerVel.y = Mathf.Sqrt(jumpForce * -3f * gravity);
        }
    }

    public void playerLook()
    {
        xRotation = (lookPos.y * Time.deltaTime) * ySens;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        transform.Rotate(Vector3.up * (lookPos.x * Time.deltaTime) * xSens);
    }
}
