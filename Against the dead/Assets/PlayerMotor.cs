using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMotor : MonoBehaviour
{
    [SerializeField]
    private Camera cam;
    private Vector3 velocity;
    private Vector3 rotation;
    private Vector3 cameraRotation;

    private CharacterController characterController;

    private float heightAboveGround = 0f;
    private float jumpForce = 10f;
    private bool isJumping = false;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    public void Move(Vector3 _velocity)
    {
        velocity = _velocity;
    }

    public void Rotate(Vector3 _rotation)
    {
        rotation = _rotation;
    }

    public void RotateCamera(Vector3 _cameraRotation)
    {
        cameraRotation = _cameraRotation;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            isJumping = true;
            velocity.y = jumpForce;
        }
        PerfomMovement();
        PerformRotation();
    }

    private void PerfomMovement()
    {
        if (velocity != Vector3.zero)
        {
            heightAboveGround = characterController.height / 2f;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, -Vector3.up, out hit, heightAboveGround + 0.1f))
            {
                velocity.y -= 9.8f * Time.deltaTime;
                characterController.Move(velocity * Time.deltaTime);
                transform.position = new Vector3(transform.position.x, hit.point.y + heightAboveGround, transform.position.z);
                if (hit.distance <= heightAboveGround + 0.1f)
                {
                    isJumping = false;
                }
            }
            else
            {
                characterController.Move(velocity * Time.deltaTime);
            }
        }
    }

    private void PerformRotation()
    {
        transform.Rotate(rotation);
        cam.transform.Rotate(-cameraRotation);
    }
}