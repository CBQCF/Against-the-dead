using System;
using UnityEngine;
using Mirror;
using Unity.VisualScripting;
using Random = UnityEngine.Random;

public class Player : NetworkBehaviour
{
    private Chat _chat;
    private GameObject _mainInventory;
    private PlayerController _pc;
    
    
    [SyncVar(hook = nameof(OnNameChange))] 
    public string playerName;

    // Overrides
    public override void OnStartLocalPlayer()
    {
    
        Camera.main.transform.SetParent(transform); 
        Camera.main.transform.localPosition = new Vector3(0, 0, 0);
        
        string playername = "User" + Random.Range(100, 999);
        SetupPlayer(playername);
        
        _pc = this.AddComponent<PlayerController>();
        this.AddComponent<PlayerCombat>();
        PlayerShoot ps = this.AddComponent<PlayerShoot>();
        ps.damage = 20;
        ps.range = 100;
    }
    
    // Functions
    
    public void SwitchInventory()
    {
        _mainInventory.SetActive(!_mainInventory.activeInHierarchy);
        _pc.InInterface = _mainInventory.activeInHierarchy;
        
        if (_mainInventory.activeInHierarchy)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    // Events
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SwitchInventory();
        }
    }
    
    private void Start()
    {
        name = playerName;
        this.AddComponent<Stats>().health = 100;
        _mainInventory = GameObject.FindGameObjectWithTag("Main UI").transform.GetChild(0).gameObject;
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
