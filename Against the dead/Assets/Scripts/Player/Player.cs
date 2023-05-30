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
    [SyncVar(hook = nameof(OnNameChange))] 
    public string playerName;

    [SyncVar] public int id;

    public readonly int invSize = 32;

    public PlayerController playerController;
    public PlayerShoot playerShoot;
    public InventoryManager inventoryManager;
    public Stats stats;
    public PlayerWeapon playerWeapon;
    public PauseMenu pauseMenu;
    public MiniMapScript miniMap;
    public List<Item> inventory;


    // Overrides
    public override void OnStartLocalPlayer()
    {
    
        Camera.main.transform.SetParent(transform); 
        Camera.main.transform.localPosition = new Vector3(0, 0, 0);
        
        string playername = NetManager.Instance.GetComponent<Auth>().username;
        SetupPlayer(playername);
        
        SetupId(((Auth)NetManager.Instance.authenticator).id);

        UIReference uiReference = GameObject.FindGameObjectWithTag("Main UI").GetComponent<UIReference>();
        
        inventoryManager = gameObject.GetComponent<InventoryManager>();
        
        playerController = this.AddComponent<PlayerController>();
        playerShoot = this.AddComponent<PlayerShoot>();
        stats = this.AddComponent<Stats>();
        pauseMenu = GameObject.Find("PauseMenu").GetComponent<PauseMenu>();
        miniMap = GameObject.Find("MiniMapCamera").GetComponent<MiniMapScript>();

        stats.healthBar = uiReference.healthBar.GetComponent<Bar>();
        stats.foodBar = uiReference.foodBar.GetComponent<Bar>();
        
        
        stats.health = 100;
        stats.healthBar.SetMax(stats.health);
        stats.healthBar.SetValue(stats.health);

        playerController.player = this;

        playerWeapon.SyncWeapon();
        
        uiReference.darkBackground.GetComponent<BackgroundDrop>().inventoryManager = inventoryManager;
        
        uiReference.debug.GetComponent<DebugItem>().inventoryManager = inventoryManager;
        NetworkClient.RegisterHandler<InventoryManager.ResponseInventoryAction>(inventoryManager.OnInventoryResponse);
        
        inventoryManager.player = this;
        inventoryManager.Enable();
        pauseMenu.player = this;
    }
    
    
    // Functions

    private void Start()
    {
        name = playerName;

        inventoryManager = gameObject.GetComponent<InventoryManager>();
        inventoryManager.player = this;
        if (isServer)
        {
            inventory = new List<Item>();
        }
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
    void SetupId(int identifier)
    {
        id = identifier;
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
}
