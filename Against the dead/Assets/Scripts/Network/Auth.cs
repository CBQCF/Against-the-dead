using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Mirror;
using UnityEngine;

/*
    Documentation: https://mirror-networking.gitbook.io/docs/components/network-authenticators
    API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkAuthenticator.html
*/

public class Auth : NetworkAuthenticator
{
    [Header("Server Credentials")]
    public string serverPassword;

    [Header("Client Credentials")]
    public string username;
    public string password;

    readonly HashSet<NetworkConnection> connectionsPendingDisconnect = new HashSet<NetworkConnection>();
    
    #region Messages

    public struct AuthRequestMessage : NetworkMessage
    {
        public string userName;
        public string userPassword;
        public string serverPassword;
    }

    public struct AuthResponseMessage : NetworkMessage
    {
        public byte code;
        public string message;
    }

    #endregion

    #region Server

    /// <summary>
    /// Called on server from StartServer to initialize the Authenticator
    /// <para>Server message handlers should be registered in this method.</para>
    /// </summary>
    public override void OnStartServer()
    {
        // register a handler for the authentication request we expect from client
        NetworkServer.RegisterHandler<AuthRequestMessage>(OnAuthRequestMessage, false);
    }

    /// <summary>
    /// Called on server from OnServerConnectInternal when a client needs to authenticate
    /// </summary>
    /// <param name="conn">Connection to client.</param>
    public override void OnServerAuthenticate(NetworkConnectionToClient conn) { }

    /// <summary>
    /// Called on server when the client's AuthRequestMessage arrives
    /// </summary>
    /// <param name="conn">Connection to client.</param>
    /// <param name="msg">The message payload</param>
    public void OnAuthRequestMessage(NetworkConnectionToClient conn, AuthRequestMessage msg)
    {
        bool valid;
        AuthResponseMessage authResponseMessage;
        if (serverPassword == msg.serverPassword)
        {
            if (SqLiteHandler.Instance.GetUser(msg.userName).Read()) // User already exists
            {
                valid = SqLiteHandler.Instance.CheckPassword(msg.userName, msg.userPassword); // Check password
            }
            else
            {
                SqLiteHandler.Instance.RegisterUser(msg.userName, msg.userPassword); // Register new user
                valid = true;
            }
        }
        else
        {
            valid = false;
        }

        if (valid)
        {
            authResponseMessage = new AuthResponseMessage()
            {
                code = 100,
                message = "Valid credentials"
            };
            conn.Send(authResponseMessage);
            ServerAccept(conn);
            conn.isAuthenticated = true;
        }
        else
        {
            authResponseMessage = new AuthResponseMessage()
            {
                code = 200,
                message = "Incorrect Credentials"
            };
            conn.Send(authResponseMessage);
            conn.isAuthenticated = false;
            connectionsPendingDisconnect.Add(conn);
            StartCoroutine(DelayedDisconnect(conn, 1f));
        }
        
    }
    
    IEnumerator DelayedDisconnect(NetworkConnectionToClient conn, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        // Reject the unsuccessful authentication
        ServerReject(conn);

        yield return null;

        // remove conn from pending connections
        connectionsPendingDisconnect.Remove(conn);
    }

    /// <summary>
    /// Called when server stops, used to unregister message handlers if needed.
    /// </summary>
    public override void OnStopServer()
    {
        // Unregister the handler for the authentication request
        NetworkServer.UnregisterHandler<AuthRequestMessage>();
    }

    #endregion

    #region Client

    /// <summary>
    /// Called on client from StartClient to initialize the Authenticator
    /// <para>Client message handlers should be registered in this method.</para>
    /// </summary>
    public override void OnStartClient()
    {
        // register a handler for the authentication response we expect from server
        NetworkClient.RegisterHandler<AuthResponseMessage>(OnAuthResponseMessage, false);
    }

    /// <summary>
    /// Called on client from OnClientConnectInternal when a client needs to authenticate
    /// </summary>
    public override void OnClientAuthenticate()
    {
        AuthRequestMessage authRequestMessage = new AuthRequestMessage()
        {
            serverPassword = serverPassword,
            userName = username,
            userPassword = password
        };
        
        NetworkClient.Send(authRequestMessage);
    }

    /// <summary>
    /// Called on client when the server's AuthResponseMessage arrives
    /// </summary>
    /// <param name="msg">The message payload</param>
    public void OnAuthResponseMessage(AuthResponseMessage msg)
    {
        // Authentication has been accepted
        if (msg.code == 100)
        {
            NotificationManager.Instance.SuccessNetwork("Authentication successful !");

            // Authentication has been accepted
            ClientAccept();
        }
        else
        {
            NotificationManager.Instance.Error("Authentication failed !");
            // Authentication has been rejected
            ClientReject();
        }
    }

    /// <summary>
    /// Called when client stops, used to unregister message handlers if needed.
    /// </summary>
    public override void OnStopClient()
    {
        // Unregister the handler for the authentication response
        NetworkClient.UnregisterHandler<AuthResponseMessage>();
    }

    #endregion
}