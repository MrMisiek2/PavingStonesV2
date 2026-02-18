using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FPSPlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 8f;
    public float crouchSpeed = 2.5f;

    [Header("Mouse Look")]
    public float mouseSensitivity = 150f;
    public Transform cameraHolder;

    [Header("Jump & Gravity")]
    public float jumpHeight = 1.6f;
    public float gravity = -20f;

    [Header("Crouch")]
    public float crouchHeight = 1f;

    private CharacterController controller;
    private Vector3 velocity;
    private float xRotation;
    private bool isCrouching;
    private float standHeight;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        standHeight = controller.height;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Look();
        HandleCrouch();
        HandleMovement();
    }

    void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        cameraHolder.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        bool grounded = controller.isGrounded;

        if (grounded && velocity.y < 0)
            velocity.y = -2f;

        float speed = walkSpeed;

        if (Input.GetKey(KeyCode.LeftShift))
            speed = sprintSpeed;

        if (isCrouching)
            speed = crouchSpeed;

        Vector3 move = transform.right * x + transform.forward * z;

        if (Input.GetButtonDown("Jump") && grounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        Vector3 finalMove = move * speed + Vector3.up * velocity.y;
        controller.Move(finalMove * Time.deltaTime);
    }

    void HandleCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
            isCrouching = true;

        if (Input.GetKeyUp(KeyCode.LeftControl))
            isCrouching = false;

        controller.height = isCrouching ? crouchHeight : standHeight;
    }
}
