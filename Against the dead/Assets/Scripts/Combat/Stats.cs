using System;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Telepathy;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class Stats : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnDamageTaken))]
    public int health;
    [SyncVar]
    public int food;
    public int playerKill;
    public int normalKill; 
    public int crawlerKill;
    
    
    public Bar healthBar;
    public BarFood foodBar;

    public int GetPlayerCount()
    {
        if (NetworkServer.active)
        {
            // Si vous êtes l'hôte du serveur
            return NetManager.Instance.numPlayers;
        }
        else if (NetworkClient.isConnected)
        {
            NetworkManager.singleton.StopServer();
            NetworkManager.Destroy(this.gameObject);
            return 1; // Un seul joueur (vous-même)
        }

        // Si vous n'êtes ni l'hôte ni un client, le nombre de joueurs est inconnu
        return -1;
    }

    private void OnDamageTaken(int oldValue, int newValue)
    {
        if (healthBar is not null) healthBar.SetValue(newValue);
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
                NetworkServer.Destroy(gameObject);
                Debug.Log("faire spawn une arme");
            }
        }
    }
    
    public void AddHealth(int damage)
    {
        health -= damage;
        if (healthBar is not null)
        {
            healthBar.SetValue(health);
        }

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

    [Server]
    public void SubFood()
    {
        food -= 2;
        foodBar.SetValue(food);
        if (food <= 0)
        {
            SceneManager.LoadScene("GameOver");
            Cursor.lockState = CursorLockMode.None;
            NetworkManager.singleton.StopClient();
            NetworkServer.Destroy(this.gameObject);
        }
    }
    
    public void AddFood()
    {
        food += 5;
        if (food > 50)
        {
            food = 50;
        }
        foodBar.SetValue(food);
        
    }

    public override string ToString()
    {
        StatsWrapper stats = new StatsWrapper();
        
        stats.stats = new Dictionary<string, int>();
        
        stats.stats.Add("health", health);
        stats.stats.Add("hunger", food);
        
        stats.stats.Add("player", playerKill);
        stats.stats.Add("normal", normalKill);
        stats.stats.Add("crawler", crawlerKill);

        return JsonUtility.ToJson(stats);

    }
}