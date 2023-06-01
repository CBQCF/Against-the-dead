using System;
using System.Collections.Generic;
using kcp2k;
using UnityEngine;
using Mirror;
using Mirror.Authenticators;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class Player : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnNameChange))] 
    public string playerName;
    [SyncVar]
    public int id;

    public PlayerController playerController;
    public PlayerShoot playerShoot;
    public InventoryManager inventoryManager;
    public Stats stats;
    public PlayerWeapon playerWeapon;
    public PauseMenu pauseMenu;
    public MiniMapScript miniMap;
    
    public readonly int invSize = 32;
    public List<Item> inventory;

    // Overrides
    public override void OnStartLocalPlayer()
    {
    
        Camera.main.transform.SetParent(transform); 
        Camera.main.transform.localPosition = new Vector3(0, 0, 0);
        
        string playername = NetManager.Instance.GetComponent<Auth>().username;
        SetupPlayer(playername);

        SetupID(((Auth)NetManager.Instance.authenticator).id);
        
        UIReference uiReference = GameObject.FindGameObjectWithTag("Main UI").GetComponent<UIReference>();

        playerController = this.AddComponent<PlayerController>();
        playerShoot = this.AddComponent<PlayerShoot>();
        playerShoot.player = this;
        pauseMenu = GameObject.Find("PauseMenu").GetComponent<PauseMenu>();
        miniMap = GameObject.Find("MiniMapCamera").GetComponent<MiniMapScript>();

        stats.healthBar = uiReference.healthBar.GetComponent<Bar>();
        stats.foodBar = uiReference.foodBar.GetComponent<BarFood>();
        
        stats.healthBar.SetMax(stats.health);
        stats.healthBar.SetValue(stats.health);

        stats.foodBar.SetMax(stats.food);
        stats.foodBar.SetValue(stats.food);

        playerController.player = this;
        
        playerWeapon.SyncWeapon();

        uiReference.darkBackground.GetComponent<BackgroundDrop>().inventoryManager = inventoryManager;

        uiReference.debug.GetComponent<DebugItem>().inventoryManager = inventoryManager;

        NetworkClient.RegisterHandler<InventoryManager.ResponseInventoryAction>(inventoryManager.OnInventoryResponse);
        
        inventoryManager.player = this; //*
        inventoryManager.Enable();
        pauseMenu.player = this;

        SyncZombies();
    }
    
    
    // Functions

    private void SyncZombies()
    {
        
        foreach (var objectIdentity in NetworkClient.spawned)
        {
            horde horde = objectIdentity.Value.GetComponent<horde>();
            if (horde is not null)
            {
                foreach (var zombieID in horde.zombies)
                {
                    if (NetworkClient.spawned.TryGetValue(zombieID, out var zombie))
                    {
                        zombie.transform.parent = horde.transform;
                    }
                }
            }
        }
    }
    
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
    void SetupID(int identifier)
    {
        id = identifier;
    }
    
    [Command]
    public void SpawnInHands(int inHands)
    {
        GameObject item = Instantiate(NetManager.Instance.spawnPrefabs[inHands], transform);
        NetworkServer.Spawn(item, new LocalConnectionToClient());
    }

    [TargetRpc]
    public void DisconnectRPC()
    {
        NetManager.Instance.DisconnectExport(netIdentity);
        NetManager.Destroy(gameObject);
        NetManager.Instance.StopClient();
        NetManager.Instance.Reset();
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName("Menu"));
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName("Main"));
        SceneManager.LoadScene("Menu");
        
    }
    
    [Client]
    public void Disconnect()
    {
        NetManager.Instance.DisconnectExport(netIdentity);
        NetManager.Destroy(gameObject);
        NetManager.Instance.StopClient();
        NetManager.Instance.Reset();
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName("Menu"));
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName("Main"));
        SceneManager.LoadScene("Menu");
    }
    
    [Command]
    public void ShootCommand(uint target, uint shooter, uint weapon)
    {
        Player shooterPlayer = NetworkServer.spawned[shooter].GetComponent<Player>();
        if (shooterPlayer.inventory.Exists(data => data.netId == weapon)) // Check if player has the weapon
        {
            Stats targetStats = NetworkServer.spawned[target].GetComponent<Stats>();
            WeaponData weaponData = NetworkServer.spawned[weapon].GetComponent<WeaponData>();
            int range = weaponData is not null ? weaponData.ammo.AmmoRange : 2;
            int damage = weaponData is not null ? weaponData.ammo.Damage : 10;
            if (targetStats is not null && Vector3.Distance(transform.position, targetStats.transform.position) <= range)
            {
                if (targetStats.DealDamage(damage))
                {
                    shooterPlayer.stats.AddFood();
                }
            }
        }
    }

    [Command]
    private void DisconnectExport(NetworkIdentity identity)
    {
        NetManager.Instance.DisconnectExport(identity);
    }
}
