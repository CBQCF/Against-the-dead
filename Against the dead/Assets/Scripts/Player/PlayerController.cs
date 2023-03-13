using Mirror;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : NetworkBehaviour
{ 
    private float speed = 3f; 
    private float jumspeed = 8f;
    private float mouseSensitivityX = 3f; 
    private float mouseSensitivityY = 3f;
    private float maxHeadTurn = 60f;
    private float gravity = 20f;
    
    private Vector3 moveD = Vector3.zero;
    
    private CharacterController characterController;
    private Camera cam;

    private void HandleMovement()
    {
        if (characterController.isGrounded)
        {
            moveD = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveD = transform.TransformDirection(moveD);
            moveD *= speed;

            if (Input.GetButton("Jump"))
            {
                moveD.y = jumspeed;
            }
        }

        moveD.y -= gravity * Time.deltaTime;

        characterController.Move(moveD * Time.deltaTime);
    }

    private void HandleRotation()
    {
        // Get rotations
        float yRot = Input.GetAxisRaw("Mouse X");
        float xRot = Input.GetAxisRaw("Mouse Y");
        
        Vector3 rotation = new Vector3(0, yRot, 0) * mouseSensitivityX;
        Vector3 cameraRotation = new Vector3(xRot, 0, 0) * mouseSensitivityY;
        
        // Applying the values
        float x = (cam.transform.eulerAngles - cameraRotation).x;
        if ( x > 180f ? x-360 > -maxHeadTurn : x < maxHeadTurn)
            cam.transform.Rotate(-cameraRotation);
        transform.Rotate(rotation);
    }
    
    
    // Event function
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        characterController = GetComponent<CharacterController>();
        cam = Camera.main;
    }

    private void Update()
    {
        HandleMovement();
        HandleRotation();
    }
}