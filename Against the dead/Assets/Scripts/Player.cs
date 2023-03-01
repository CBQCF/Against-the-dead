using System;
using UnityEngine;
using Mirror;
using Random = UnityEngine.Random;

public class Player : NetworkBehaviour
{
    private Chat _chat;
    
    [SyncVar(hook = nameof(OnNameChange))] 
    public string playerName;
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
    
    // Overrides
    public override void OnStartLocalPlayer()
    {
        Camera.main.transform.SetParent(transform);
        Camera.main.transform.localPosition = new Vector3(0, 0, 0);

        string playername = "User" + Random.Range(100, 999);
        SetupPlayer(playername);

    }
    
    // Events
    private void Awake()
    {
        _chat = FindObjectOfType<Chat>();
    }
    void Update()
    {
        HandleMovement();
    }
    
    // Hooks
    public void OnNameChange(string _Old, string _New)
    {
        name = playerName;
    }
    
    // Commands
    [Command]
    void SetupPlayer(string playername)
    {
        playerName = playername;
    }

    [Command]
    public void SendChatMessage()
    {
        if (_chat)
        {
        }
    }
}
