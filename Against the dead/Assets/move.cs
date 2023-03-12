using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    public float speed = 6f;

    public float jumspeed = 8f;

    public float RotateSpeedX = 20f;
    
    public float RotateSpeedY = 20f;

    public float gravity = 20f;

    private Vector3 moveD = Vector3.zero;

    private CharacterController Cac;

    private Vector2 turn;
    
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cac = GetComponent<CharacterController>();

    }

    // Update is called once per frame
    void Update()
    {

        if (Cac.isGrounded)
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

        turn.x += Input.GetAxis("Mouse X") * RotateSpeedX;
        turn.y += Input.GetAxis("Mouse Y") * RotateSpeedY;

        transform.localRotation = Quaternion.Euler(- turn.y, turn.x,0);

        Cac.Move(moveD * Time.deltaTime);
    }
}
