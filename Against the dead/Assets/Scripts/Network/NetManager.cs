using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using UnityEngine;
using Mirror;
using Unity.VisualScripting;
using UnityEngine.Serialization;

public class NetManager : NetworkManager
{
    private float time = 0.0f;
    public float interpolationPeriod = 5f;
    
    public InventoryManager localInventoryManager;
    
    public static NetManager Instance
    {
        get
        {
            if (instance != null)
            {
                return instance;
            }

            instance = FindObjectOfType<NetManager>();

            if (instance != null)
            {
                return instance;
            }

            CreateNewInstance();

            return instance;

        }
    }

    private static NetManager CreateNewInstance()
    {
        NetManager netManager = Resources.Load<NetManager>("NetworkManager");
        instance = Instantiate(netManager);

        return instance;
    }
    
    private static NetManager instance;


    [Server]
    public void DisconnectExport(NetworkIdentity identity)
    {
        PlayerExport(identity.connectionToClient);
    }
    
    [Server]
    void Update()
    {
        time += Time.deltaTime;
 
        if (time >= interpolationPeriod) {
            time = 0.0f;

            foreach (var connection in NetworkServer.connections)
            {
                PlayerExport(connection.Value);
            }
        }
    }

    public override void OnStartServer()
    {
        localInventoryManager = this.AddComponent<InventoryManager>();
        NetworkServer.RegisterHandler<InventoryManager.RequestInventoryAction>(localInventoryManager.OnInventoryAction);
    }

    public override void OnStopServer() { }

    public override void OnClientConnect()
    { }

    public override void OnServerDisconnect(NetworkConnectionToClient conn) { }

    private void PlayerExport(NetworkConnectionToClient conn)
    {
        Player player = conn.identity.GetComponent<Player>();
        localInventoryManager.ExportInventory(player.id, localInventoryManager.ConvertInventory(player.inventory)); // Export Inventory to DB
    }

    public override void OnClientDisconnect()
    {
        if (NetworkClient.activeHost)
        {
            StopServer();
        }
    }
}
