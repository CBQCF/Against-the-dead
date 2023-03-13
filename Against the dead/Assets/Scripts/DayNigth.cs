using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

public class DayNigth : NetworkBehaviour
{
    private Light sun;
    public float speed = 1f;

    void Start()
    {
        sun = GetComponent<Light>();
    }
    
    void Update()
    {
        sun.transform.Rotate(UnityEngine.Vector3.right * speed * Time.deltaTime);
    }
}
