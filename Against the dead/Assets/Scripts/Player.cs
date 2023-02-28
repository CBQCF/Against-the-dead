using UnityEngine;
using Mirror;
public class Player : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnNameChange))] 
    public string playerName;
    public override void OnStartLocalPlayer()
    {
        Camera.main.transform.SetParent(transform);
        Camera.main.transform.localPosition = new Vector3(0, 0, 0);

        string playername = "User" + Random.Range(100, 999);
        SetupPlayer(playername);

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

    public void OnNameChange(string _Old, string _New)
    {
        name = playerName;
    }
    
    [Command]
    public void SetupPlayer(string playername)
    {
        playerName = playername;
    }
    void Update()
    {
        HandleMovement();
    }
}
