using UnityEngine;
using Mirror;

public class NetManager : NetworkManager
{
    public override void OnStartServer()
    {
        Debug.Log("Server started!");
    }

    public override void OnStopServer()
    {
        Debug.Log("Server stopped!");
    }

    public override void OnClientConnect()
    {
        Debug.Log("Connected to server!");
    }

    public override void OnClientDisconnect()
    {
        Debug.Log("Disconnected from Server!");
    }
}
