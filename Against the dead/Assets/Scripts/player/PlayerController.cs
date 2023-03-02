using Mirror;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : NetworkBehaviour
{
    [SerializeField] private float speed = 3f;

    [SerializeField] private float mouseSensitivityX = 3f;

    [SerializeField] private float mouseSensitivityY = 3f;

    private PlayerMotor motor;
    private Player player;

    private void Start()
    {
        motor = GetComponent<PlayerMotor>();
        player = GetComponent<Player>();
    }

    private void Update()
    {
        // Calculer la vitesse du mouvement de notre joueur
        float xMov = Input.GetAxisRaw("Horizontal");
        float zMov = Input.GetAxisRaw("Vertical");

        Vector3 moveHorizontal = transform.right * xMov;
        Vector3 moveVertical = transform.forward * zMov;

        Vector3 velocity = (moveHorizontal + moveVertical).normalized * speed;

        motor.Move(velocity);

        // On calcule la rotation du joueur en un Vector3
        float yRot = Input.GetAxisRaw("Mouse X");

        Vector3 rotation = new Vector3(0, yRot, 0) * mouseSensitivityX;

        motor.Rotate(rotation);

        // On calcule la rotation de la caméra en un Vector3
        float xRot = Input.GetAxisRaw("Mouse Y");

        Vector3 cameraRotation = new Vector3(xRot, 0, 0) * mouseSensitivityY;
        motor.RotateCamera(cameraRotation);

        //Jump!
        if (Input.GetKeyDown(KeyCode.Space))
        {
            motor.Jump();
        }
    }
}