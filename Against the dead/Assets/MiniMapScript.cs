using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using Unity.VisualScripting;
using UnityEngine;

public class MiniMapScript : NetworkBehaviour
{
    public Transform player;

    private void LateUpdate()
    {
        Vector3 newposition = player.position;
        newposition.y = transform.position.y;
        transform.position = newposition;
        
        transform.rotation = Quaternion.Euler(90f,player.eulerAngles.y,0f);
    }
}
