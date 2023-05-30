using System;
using System.Collections.Generic;
using kcp2k;
using UnityEngine;
using Mirror;
using Mirror.Authenticators;
using Unity.VisualScripting;
using Random = UnityEngine.Random;

public class Player : NetworkBehaviour
{
    private Chat _chat;


    [SyncVar(hook = nameof(OnNameChange))] 
    public string playerName;

    public PlayerController playerController;
    public PlayerShoot playerShoot;
    public InventoryManager inventoryManager;
    public Stats stats;
    public PlayerWeapon playerWeapon;
    public PauseMenu pauseMenu;
    public MiniMapScript miniMap;
    
    // Overrides
    public override void OnStartLocalPlayer()
    {
    
        Camera.main.transform.SetParent(transform); 
        Camera.main.transform.localPosition = new Vector3(0, 0, 0);
        
        string playername = NetManager.singleton.GetComponent<Auth>().username;
        SetupPlayer(playername);


        inventoryManager = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
        playerController = this.AddComponent<PlayerController>();
        playerShoot = this.AddComponent<PlayerShoot>();
        playerShoot.player = this;
        stats = this.AddComponent<Stats>();
        pauseMenu = GameObject.Find("PauseMenu").GetComponent<PauseMenu>();
        miniMap = GameObject.Find("MiniMapCamera").GetComponent<MiniMapScript>();

        stats.healthBar = GameObject.FindWithTag("Main UI").transform.GetChild(2).GetComponent<Bar>();
        stats.foodBar = GameObject.FindWithTag("Main UI").transform.GetChild(3).GetComponent<BarFood>();
        
        
        stats.health = 100;  //*
        stats.healthBar.SetMax(stats.health);
        stats.healthBar.SetValue(stats.health);

        stats.food = 50;
        stats.foodBar.SetMax(stats.food);
        stats.foodBar.SetValue(stats.food);

        playerWeapon.SyncWeapon();
        
        inventoryManager.player = this; //*
        pauseMenu.player = this;
    }
    
    
    // Functions

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
    public void CmdSpawnItem(int item, int amount, Vector3 position)
    {
        GameObject obj = Instantiate(NetworkManager.singleton.spawnPrefabs[item], position, Quaternion.identity);
        obj.GetComponent<ItemData>().amount = amount;
        NetworkServer.Spawn(obj);
    }

    [Command]
    public void Destroy(uint id)
    {
        
        NetworkServer.Destroy(NetworkServer.spawned[id].gameObject);
    }

    [Command]
    public void SpawnInHands(int inHands)
    {
        GameObject item = Instantiate(NetworkManager.singleton.spawnPrefabs[inHands], transform);
        NetworkServer.Spawn(item, new LocalConnectionToClient());
    }
    
    [Command]
    public void CmdInflictDamage(int damage)
    {
        Stats stats = GetComponent<Stats>();
        Bar healthBar = GetComponent<Bar>();
        if (stats != null)
        {
            stats.AddHealth(damage);
        }
    }
    
    [Command]
    public void ExportInventory(int id, string data)
    {
        SqLiteHandler.Instance.UpdateUser(id,"inventory", data);
    }
    
    [Command]
    public void ExportStats(int id, string data)
    {
        SqLiteHandler.Instance.UpdateUser(id,"stats", data);
    }
}
