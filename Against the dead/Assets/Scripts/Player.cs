using UnityEngine;
using Mirror;
public class Player : NetworkBehaviour
{
    [SyncVar] 
    public string name;
    public override void OnStartLocalPlayer()
    {
        Camera.main.transform.SetParent(transform);
        Camera.main.transform.localPosition = new Vector3(0, 0, 0);
        
    }
    void HandleMovement()
    {
        if (isLocalPlayer)
        {
            float moveX = Input.GetAxis("Horizontal") * Time.deltaTime * 110.0f;
            float moveZ = Input.GetAxis("Vertical") * Time.deltaTime * 4f;

            transform.Rotate(0, moveX, 0);
            transform.Translate(0, 0, moveZ);
        }
    }
    
    void Update()
    {
        HandleMovement();
    }
}
