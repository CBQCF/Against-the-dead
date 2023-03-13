using Mirror;
using UnityEngine;

public class ServerInfo : NetworkBehaviour
{
    public GameObject[] playerList { get; private set; }
    
    [Server]
    private void Update()
    {
        playerList = GameObject.FindGameObjectsWithTag("Player");
    }
}