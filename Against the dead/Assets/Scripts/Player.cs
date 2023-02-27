using UnityEngine;
using Mirror;
public class Player : NetworkBehaviour
{
    void HandleMovement()
    {
        if (isLocalPlayer)
        {
            float axisX = Input.GetAxis("Horizontal");
            float axisY = Input.GetAxis("Vertical");
            Vector3 movement = new Vector3(axisX, axisY, 0);
            transform.position += movement;
        }
    }
    
    void Update()
    {
        HandleMovement();
    }
}
