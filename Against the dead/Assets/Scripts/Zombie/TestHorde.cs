using System;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class TestHorde : NetworkBehaviour
{
    [SerializeField] private GameObject hordePrefab;

    [SerializeField] private Vector3 zoneSize;

    public ServerInfo serverInfo;
    private Vector3 randomSpawn()
    {
        return new Vector3(
            Random.Range(transform.position.x - zoneSize.x / 2, transform.position.x + zoneSize.x / 2),
            Random.Range(transform.position.y - zoneSize.y / 2, transform.position.y + zoneSize.y / 2),
            Random.Range(transform.position.z - zoneSize.z / 2, transform.position.z + zoneSize.z / 2)
        );
    }

    [Command(channel = 0, requiresAuthority = false)]
    public void HordeInit()
    {
        serverInfo = FindObjectOfType<ServerInfo>();
        
        for (int i = 0; i < 10; i++)
        {
            SpawnHorde();
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, zoneSize);
    }

    [Server]
    public void SpawnHorde()
    {
        var position = randomSpawn();
        GameObject hordeGameObject = Instantiate(hordePrefab, position, Quaternion.identity);
        horde hodreComponment = hordeGameObject.GetComponent<horde>();
        hodreComponment.spawner = this;
        NetworkServer.Spawn(hordeGameObject);
        hodreComponment.SyncParent();
    }
}
