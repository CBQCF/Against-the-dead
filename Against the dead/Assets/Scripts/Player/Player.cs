using System;
using UnityEngine;
using Mirror;
using Unity.VisualScripting;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Player : NetworkBehaviour
{
    private Chat _chat;


    [SyncVar(hook = nameof(OnNameChange))] 
    public string playerName;

    public PlayerController playerController;
    public PlayerShoot playerShoot;
    public InventoryManager inventoryManager;
    public PauseMenu pauseMenu;
    public HealthBar healthBar;

    // Overrides
    public override void OnStartLocalPlayer()
    {
    
        Camera.main.transform.SetParent(transform); 
        Camera.main.transform.localPosition = new Vector3(0, 0, 0);
        
        string playername = "User" + Random.Range(100, 999);
        SetupPlayer(playername);


        inventoryManager = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
        pauseMenu = GameObject.Find("PauseMenu").GetComponent<PauseMenu>();
        playerController = this.AddComponent<PlayerController>();
        playerShoot = this.AddComponent<PlayerShoot>();
        healthBar = GameObject.FindWithTag("Main UI").transform.GetChild(2).GetComponent<HealthBar>();

        inventoryManager.player = this;
        pauseMenu.player = this;

        playerShoot.damage = 20;
        playerShoot.range = 100;
    }
    
    // Functions

    private void Start()
    {
        name = playerName;
        this.AddComponent<Stats>().health = 100;
        healthBar.SetMaxHealth(GetComponent<Stats>().health);
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
    public void CmdSpawnItem(int item, Vector3 position)
    {
        GameObject obj = Instantiate(NetworkManager.singleton.spawnPrefabs[item], position, Quaternion.identity);
        NetworkServer.Spawn(obj);
    }

}
