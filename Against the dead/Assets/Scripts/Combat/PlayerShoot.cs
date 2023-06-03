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
        Item selected = player.inventoryManager.GetSelectedItem(false);
        if (selected is not null)
        {
            if (selected.UID == 1 || selected.UID == 2)
                FindObjectOfType<AudioManager>().Play("mitraillette");
            else if (selected.UID == 3)
                FindObjectOfType<AudioManager>().Play("hache");
            else if (selected.UID == 4)
                FindObjectOfType<AudioManager>().Play("gun");
            else if (selected.UID == 5)
                FindObjectOfType<AudioManager>().Play("shotgun");
        }

        if (Physics.Raycast(_fpsCam.transform.position, _fpsCam.transform.forward, out var hit, range))
        {
            Stats target = hit.transform.GetComponent<Stats>();
            if (target is not null)
            {
                uint nId = 0;
                if (selected is not null) nId = selected.netId; 
                player.ShootCommand(target.gameObject.GetComponent<NetworkIdentity>().netId, player.netId, nId);
            }
        }
    }
}