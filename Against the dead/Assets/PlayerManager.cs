using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayerManager : NetworkBehaviour
{
    public ServerInfo serverInfo;
    private float timer = 5f;

    [Server]
    void Update()
    {
        if (serverInfo.playerList != null)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                foreach (var player in serverInfo.playerList)
                {
                    player.GetComponent<Stats>().SubFood();
                }
                timer = 5f;
            }
        }
    }
}
