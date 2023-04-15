using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayerWeapon : NetworkBehaviour
{
    public GameObject[] weapons;

    public void ChangeVisible(GameObject weapon)
    {
        foreach (GameObject wp in weapons)
        {
            wp.SetActive(wp.GetComponent<ItemData>().item == weapon.GetComponent<ItemData>().item);
        }
    }

    public void HideWeapon()
    {
        foreach (GameObject wp in weapons)
        {
            wp.SetActive(false);
        }
    }

    public void SyncWeapon()
    {
        foreach (GameObject weapon in weapons)
        {
            weapon.transform.parent = Camera.main.transform;
        }
    }
}
