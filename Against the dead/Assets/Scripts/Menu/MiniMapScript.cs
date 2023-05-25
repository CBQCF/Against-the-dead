using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using Unity.VisualScripting;
using UnityEngine;

public class MiniMapScript : NetworkBehaviour
{
    private Transform _transform;

    private void Awake()
    {
        _transform = Camera.main.transform;
    }

    private void LateUpdate()
    {
        if (_transform is not null)
        {
            Vector3 newposition = _transform.position;
            newposition.y = transform.position.y;
            transform.position = newposition;
            transform.rotation = Quaternion.Euler(90f, transform.eulerAngles.y, 0f);
        }
    }
}
