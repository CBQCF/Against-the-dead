using UnityEngine;

public class PlayerShoot : MonoBehaviour
{

    public PlayerWeapon weapon; 

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private LayerMask mask;

    void Start()
    {
        if (cam == null)
        {
            Debug.LogError("pas de caméra renseignée sur le système de tir");
                this.enabled = false;
        }
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
            Debug.Log("Objet touché : " + hit.collider.name);
        }

    }
}
