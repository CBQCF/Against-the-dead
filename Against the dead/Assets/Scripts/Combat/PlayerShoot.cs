using Mirror;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public Player player;

    public int damage;
    public int range;

    private Camera _fpsCam;
    //public ParticleSystem muzzleFlash;

    private void Start()
    {
        _fpsCam = Camera.main;
    }
    
    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }
    
    void Shoot()
    {
        // mettre son sam
        if (Physics.Raycast(_fpsCam.transform.position, _fpsCam.transform.forward, out var hit, range))
        {
            Stats target = hit.transform.GetComponent<Stats>();
            if (target is not null)
            {
                player.ShootCommand(target.gameObject.GetComponent<NetworkIdentity>().netId, player.netId, player.inventoryManager.GetSelectedItem(false).netId);
            }
        }
    }
}