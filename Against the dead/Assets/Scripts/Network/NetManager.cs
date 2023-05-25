using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.Serialization;

public class NetManager : NetworkManager
{
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
    
    
    public override void OnStartServer() { }

    public override void OnStopServer() { }

    public override void OnClientConnect() { }

    public override void OnClientDisconnect() { }
}
