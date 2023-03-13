using System;
using UnityEngine;
using Mirror;
using Unity.VisualScripting;
using Random = UnityEngine.Random;

public class Player : NetworkBehaviour
{
    private Chat _chat;

    [SyncVar(hook = nameof(OnNameChange))] 
    public string playerName;
    public int health { get; set; }

    // Overrides
    public override void OnStartLocalPlayer()
    {
        Camera.main.transform.SetParent(transform);
        Camera.main.transform.localPosition = new Vector3(0, 0, 0);

        string playername = "User" + Random.Range(100, 999);
        SetupPlayer(playername);

        this.AddComponent<PlayerController>();
        

    }
    
    // Events
    private void Awake()
    {
        _chat = FindObjectOfType<Chat>();
    }
    void Update() { }
    
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
    public void SendChatMessage(string text)
    {
        if (_chat)
        {
            Debug.Log("Sent a message" + text);
        }
    }
}
