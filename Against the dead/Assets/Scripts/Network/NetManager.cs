using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.Serialization;

public class NetManager : NetworkManager
{

    public override void OnStartServer() { }

    public override void OnStopServer() { }

    public override void OnClientConnect() { }

    public override void OnClientDisconnect() { }
}
