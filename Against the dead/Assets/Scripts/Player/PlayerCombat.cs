using Mirror;
using UnityEngine;

public class PlayerCombat : NetworkBehaviour
{
    public Weapon weapon; 
    
    private Camera cam;

    [SerializeField]
    private LayerMask mask;

    void Start()
    {
        cam = Camera.main;
        weapon = null;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        
    }
}
