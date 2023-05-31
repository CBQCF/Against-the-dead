using System;
using kcp2k;
using Mirror;
using Mirror.Authenticators;
using TMPro;
using UnityEngine;
using System.Drawing;
using Telepathy;
using UnityEngine.SceneManagement;
using Color = UnityEngine.Color;

public class ConnectionHandler : MonoBehaviour
{
    public TMP_InputField userName;
    public TMP_InputField userPassword;
    public TMP_InputField serverIP;
    public TMP_InputField serverPort;
    public TMP_InputField serverPassword;

    private NetManager _manager;
    private KcpTransport _transport;
    private Auth _authenticator;


    private readonly ushort _defaultPort = 52345;

    private void Awake()
    {
        _manager = NetManager.Instance;
        _transport = _manager.transport.GetComponent<KcpTransport>();
        _authenticator = _manager.transport.GetComponent<Auth>();
    }

    public bool checkUser()
    {
        bool valid = true;
        if (userName.text.Length < 2)
        {
            NotificationManager.Instance.Error("Invalid username");
            valid = false;
        }

        if (userPassword.text.Length < 8 && userPassword.text.Length > 100)
        {
            NotificationManager.Instance.Error("Invalid password (must be 8+ characters)");
            valid = false;
        }

        return valid;
    }
    
    public void Connect()
    {
        if (checkUser() && !NetworkClient.isConnected)
        {
            _manager.networkAddress = serverIP.text;

            ushort port;
            if (ushort.TryParse(serverPort.text, out port) && port > 0 && port < 65535)
            {
                _transport.Port = port;
            }
            else
            {
                _transport.Port = _defaultPort;
            }

            _authenticator.serverPassword = serverPassword.text;
            _authenticator.password = userPassword.text;
            _authenticator.username = userName.text;

            NotificationManager.Instance.SuccessNetwork($"Connecting to :\n{serverIP.text}:{port.ToString()}");

            _manager.StartClient();
        }
    }

    public void Host()
    {
        if (checkUser() && !NetworkClient.isConnected)
        {
            ushort port;
            if (ushort.TryParse(serverPort.text, out port) && port > 0 && port < 65535)
            {
                _transport.Port = port;
            }
            else
            {
                _transport.Port = _defaultPort;
            }
            
            _authenticator.serverPassword = serverPassword.text;
            _authenticator.password = userPassword.text;
            _authenticator.username = userName.text;

            NotificationManager.Instance.SuccessNetwork($"Starting server on port :\n{_transport.Port.ToString()}");

            _manager.StartHost();
        }
    }

    private void Update()
    {
        if (NetworkClient.isConnected && NetworkClient.connection.isAuthenticated)
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("Main"));
            if (!NetworkClient.ready)
            {
                NetworkClient.Ready();
                if (NetworkClient.localPlayer == null)
                {
                    NetworkClient.AddPlayer();
                }
                
                if (NetworkClient.activeHost)
                {
                    GameObject.Find("ZombieSpawn").GetComponent<TestHorde>().HordeInit();
                }
            }
            
        }
        else
        { }
    }
}
