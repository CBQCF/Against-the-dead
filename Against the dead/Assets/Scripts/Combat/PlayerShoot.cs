using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayerShoot : NetworkBehaviour
{
    public Player player;

    public int damage;
    public int range;

    private Camera fpsCam;
    //public ParticleSystem muzzleFlash;

    private void Start()
    {
        fpsCam = Camera.main;
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
        
        //muzzleFlash.Play();
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range) && hit.transform.tag != "Terrain")
        {
            Stats target = hit.transform.GetComponent<Stats>();
            if (target is not null)
            {
                target.AddHealth(damage);
            }
        }
    }
}