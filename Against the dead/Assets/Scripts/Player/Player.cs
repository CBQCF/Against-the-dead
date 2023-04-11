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

    [SyncVar(hook = nameof(OnDamageTaken))] 
    public float health;

    // Overrides
    public override void OnStartLocalPlayer()
    {
    
        Camera.main.transform.SetParent(transform); 
        Camera.main.transform.localPosition = new Vector3(0, 0, 0);
        
        string playername = "User" + Random.Range(100, 999);
        SetupPlayer(playername);
        
        this.AddComponent<PlayerController>();
        this.AddComponent<PlayerCombat>();
        this.AddComponent<Inventory>();
        this.AddComponent<ItemInteraction>();
    }
    
    // Functions
    
    private void OnDamageTaken(float _old, float _new)
    {
        if (_new <= 0)
        {
            NetworkServer.Destroy(this.gameObject);
        }
    }
    
    // Events
    void Update() { }
    
    private void Start()
    {
        name = playerName;
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
    void SetHealth(int newHealth)
    {
        health = newHealth;
    }

}
