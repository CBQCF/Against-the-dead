using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEditor;
using UnityEngine;

public class horde : NetworkBehaviour
{
    public GameObject zombiePrefab;

    public GameObject crawlerPrefab;

    public TestHorde spawner;

    [SyncVar]
    public List<uint> zombies;

    [Server]
    void Start()
    {
        if (isServer)
        {
            zombies = new List<uint>();
            int sizeHorde = Random.Range(1, 10);
            for (int i = 0; i < sizeHorde; i++)
            {
                var spawnpos = new Vector3(
                    Random.Range(transform.position.x - 10, transform.position.x + 10),
                    transform.position.y,
                    Random.Range(transform.position.z - 10, transform.position.z + 10)
                );
                GameObject Zombie = Instantiate(zombiePrefab, spawnpos, Quaternion.identity);
                ZombieCharacterControl zcc = Zombie.AddComponent<ZombieCharacterControl>();
                zcc.serverInfo = spawner.serverInfo;
                NetworkServer.Spawn(Zombie);
                Zombie.transform.parent = gameObject.transform;
                zombies.Add(Zombie.GetComponent<NetworkIdentity>().netId);
            }

            GameObject Boss = Instantiate(crawlerPrefab, transform.position, Quaternion.identity);
            ZombieCharacterControl zccc = Boss.AddComponent<ZombieCharacterControl>();
            zccc.serverInfo = spawner.serverInfo;
            NetworkServer.Spawn(Boss);
            Boss.transform.parent = gameObject.transform;
            zombies.Add(Boss.GetComponent<NetworkIdentity>().netId);
        }
    }

    [ClientRpc]
    public void SyncParent()
    {
        foreach (var zombieID in zombies)
        {
            NetworkIdentity zombie = NetworkClient.spawned[zombieID];
            zombie.transform.parent = transform;
        }
    }
    

    [Server]
    void Update()
    {
        if (transform.childCount == 0)
        {
            spawner.SpawnHorde();
            NetworkServer.Destroy(this.gameObject);
        }
    }
}
