using System;
using System.Collections.Generic;
using System.Data.SQLite;
using UnityEngine;
using Mirror;
using UnityEngine.Serialization;

public class NetManager : NetworkManager
{
    public struct InventoryAction : NetworkMessage
    {
        public string action;
        public NetworkIdentity identity;
    }

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


    public override void OnStartServer()
    {
        base.OnStartServer();
        NetworkServer.RegisterHandler<InventoryAction>(OnInventoryAction);
    }

    public void OnInventoryAction(NetworkConnection conn, InventoryAction msg)
    {
        throw new NotImplementedException();
    }
    public override void OnStopServer() { }

    public override void OnClientConnect() { }

    public override void OnClientDisconnect()
    {
        Player player = NetworkClient.localPlayer.gameObject.GetComponent<Player>();
        int id = ((Auth)authenticator).id;
        
        player.ExportInventory(id, player.inventoryManager.ToString());
        player.ExportStats(id, player.stats.ToString());
        
        if (NetworkClient.activeHost)
        {
            StopServer();
        }
    }

}
