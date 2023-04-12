using Mirror;
using UnityEngine;

public class ServerInfo : NetworkBehaviour
{
    public GameObject[] playerList { get; private set; }
    
    private void Update()
    {
        if(isServer) UpDatePlayer();
    }

    [Server]
    private void UpDatePlayer()
    {
        playerList = GameObject.FindGameObjectsWithTag("Player");
    }
}