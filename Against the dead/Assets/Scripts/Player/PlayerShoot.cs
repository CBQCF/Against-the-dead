using Mirror;
using UnityEngine;

public class PlayerShoot : NetworkBehaviour
{
    public PlayerWeapon weapon; 
    
    private Camera cam;

    [SerializeField]
    private LayerMask mask;

    void Start()
    {
        cam = GetComponent<Camera>();
        weapon = GetComponent<PlayerWeapon>();
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
        RaycastHit hit;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, weapon.range, mask))
        {
            Debug.Log("Hit : " + hit.collider.name);
        }

    }
}
