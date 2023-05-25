using System;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class Stats : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnDamageTaken))]
    public int health;

    public HealthBar healthBar;
    
    public int GetPlayerCount()
    {
        if (NetworkServer.active)
        {
            // Si vous êtes l'hôte du serveur
            return NetworkManager.singleton.numPlayers;
        }
        else if (NetworkClient.isConnected)
        {
            // Si vous êtes un client
            return 1; // Un seul joueur (vous-même)
        }

        // Si vous n'êtes ni l'hôte ni un client, le nombre de joueurs est inconnu
        return -1;
    }

    private void OnDamageTaken(int oldValue, int newValue)
    {
        if (healthBar is not null) healthBar.SetHealth(newValue);
        if (newValue <= 0)
        {
            if (gameObject.CompareTag("Player"))
            {
                // Charger la scène GameOver
                SceneManager.LoadScene("GameOver");
                Cursor.lockState = CursorLockMode.None;
                NetworkManager.singleton.StopClient();
                NetworkServer.Destroy(this.gameObject);
                Debug.Log(GetPlayerCount());
                if (GetPlayerCount() == 1 || GetPlayerCount() == 0 || GetPlayerCount() == -1)
                {
                    NetworkManager.singleton.StopServer();
                    Debug.Log("arret serveur");
                }
            }
            else
            {
                NetworkServer.Destroy(this.gameObject);
            }
        }
    }
    
    public void AddHealth(int damage)
    {
        health -= damage;
        if (healthBar is not null) healthBar.SetHealth(health);
        if (health <= 0)
        {
            if (gameObject.CompareTag("Player"))
            {
                // Charger la scène GameOver
                SceneManager.LoadScene("GameOver");
                Cursor.lockState = CursorLockMode.None;
                NetworkManager.singleton.StopClient();
                NetworkServer.Destroy(this.gameObject);
            }
            else
            {
                NetworkServer.Destroy(this.gameObject);
            }
        }
    }
}