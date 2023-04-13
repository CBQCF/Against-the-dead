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

    // Overrides
    public override void OnStartLocalPlayer()
    {
    
        Camera.main.transform.SetParent(transform); 
        Camera.main.transform.localPosition = new Vector3(0, 0, 0);
        
        string playername = "User" + Random.Range(100, 999);
        SetupPlayer(playername);
        
        GameObject.Find("InventoryManager").GetComponent<InventoryManager>().pc = this.AddComponent<PlayerController>();
        this.AddComponent<PlayerCombat>();
        PlayerShoot ps = this.AddComponent<PlayerShoot>();
        ps.damage = 20;
        ps.range = 100;
    }
    
    // Functions

    private void Start()
    {
        name = playerName;
        this.AddComponent<Stats>().health = 100;
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

}
