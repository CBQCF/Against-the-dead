using System;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Telepathy;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class Stats : NetworkBehaviour
{
    [SyncVar(hook = nameof(UpdateHealthBar))]
    public int health;
    [SyncVar(hook = nameof(UpdateFoodBar))]
    public int food;
    
    public int playerKill;
    public int normalKill; 
    public int crawlerKill;

    public Bar healthBar;
    public BarFood foodBar;
    
    [Server]
    private void Awake()
    {
        health = 100;
        food = 50;
    }

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

    [Server]
    public bool DealDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            OnKill();
            return true;
        }
        
        return false;
    }

    [Server]
    public void OnKill()
    {
        if (gameObject.CompareTag("Player"))
        {
            gameObject.GetComponent<Player>().DisconnectRPC();
        }
        else
        {
            NetworkServer.Destroy(this.gameObject);
        }
    }
    
    [Client]
    public void UpdateHealthBar(int _old, int _new)
    {
        if (healthBar is not null)
        {
            healthBar.SetValue(_new);
        }
    }

    [Server]
    public bool SubFood()
    {
        food -= 2;
        return food <= 0;
    }

    void UpdateFoodBar(int _old, int _new)
    {
        if (foodBar is not null)
        {
            foodBar.SetValue(_new);
        }
    }
    
    [Server]
    public void AddFood()
    {
        food += 5;
        if (food > 50)
        {
            food = 50;
        }
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