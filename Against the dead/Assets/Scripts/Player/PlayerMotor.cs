using System;
using Mirror;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMotor : NetworkBehaviour
{
    private Camera cam;
    private CharacterController characterController;
    private Vector3 velocity;
    private Vector3 rotation;
    private Vector3 cameraRotation;
    private float heightAboveGround = 0f;
    private float jumpForce = 40f;
    private float gravity = 20f;
    private bool isJumping = false;
    private float maxHeadTurn = 60f;
    

    public void Rotate(Vector3 _rotation)
    {
        rotation = _rotation;
    }

    public void RotateCamera(Vector3 _cameraRotation)
    {
        cameraRotation = _cameraRotation;
    }
    
    
    private void PerformRotation()
    {
        float x = (cam.transform.eulerAngles - cameraRotation).x;
        if ( x > 180f ? x-360 > -maxHeadTurn : x < maxHeadTurn)
            cam.transform.Rotate(-cameraRotation);
        transform.Rotate(rotation);
    }

}