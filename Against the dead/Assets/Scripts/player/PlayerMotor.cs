using System;
using Mirror;
using UnityEngine;
public class PlayerMotor : NetworkBehaviour
{
    private Camera cam;
    private Vector3 velocity;
    private Vector3 rotation;
    private Vector3 cameraRotation;
    private float heightAboveGround = 0f;
    private float jumpForce = 40f;
    private bool isJumping = false;
    private float maxHeadTurn = 60f;

    private void Start()
    {
        cam = Camera.main;
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

    public void Jump()
    {
        if (!isJumping)
        {
            isJumping = true;
            velocity.y = jumpForce;
        }
    }
    private void Update()
    { 
        PerfomMovement();
        PerformRotation();
    }
    
    private void PerfomMovement()
    {
        if (velocity != Vector3.zero)
        {
            heightAboveGround = transform.position.y / 2f;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, -Vector3.up, out hit, heightAboveGround + 0.1f))
            {
                velocity.y -= 50f * Time.deltaTime;
                transform.position += (velocity * Time.deltaTime) + new Vector3(0, hit.point.y + heightAboveGround, 0);;
                if (hit.distance <= heightAboveGround + 0.1f)
                {
                    isJumping = false;
                }
            }
            else
            {
                velocity.y -= 19.6f * Time.deltaTime;
                transform.position += (velocity * Time.deltaTime);
            }
        }
    }

    private void PerformRotation()
    {
        float x = (cam.transform.eulerAngles - cameraRotation).x;
        if ( x > 180f ? x-360 > -maxHeadTurn : x < maxHeadTurn)
            cam.transform.Rotate(-cameraRotation);
        transform.Rotate(rotation);
    }

}