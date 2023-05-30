using Mirror;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : NetworkBehaviour
{
    private const float Speed = 6f; 
    private const float JumpSpeed = 8f;
    public static float MouseSensitivity = 3f;
    private const float MaxHeadTurn = 60f;
    private const float Gravity = 20f;

    private Vector3 _moveD = Vector3.zero;
    
    private CharacterController _characterController;
    public Player player;
    private Camera _cam;

    public bool inInterface;

    public void SetSensitivity(float sensi)
    {
        MouseSensitivity = sensi;
    }

    private void HandleMovement()
    {
        if (_characterController.isGrounded)
        {
            _moveD = Vector3.zero;
            if (!inInterface) _moveD = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            _moveD = transform.TransformDirection(_moveD);
            _moveD *= Speed;

            if (Input.GetButton("Jump"))
            {
                _moveD.y = JumpSpeed;
            }
        }

        _moveD.y -= Gravity * Time.deltaTime;

        _characterController.Move(_moveD * Time.deltaTime);
    }

    private void HandleRotation()
    {
        // Get rotations
        float yRot = Input.GetAxisRaw("Mouse X");
        float xRot = Input.GetAxisRaw("Mouse Y");

        Vector3 rotation = new Vector3(0, yRot, 0) * MouseSensitivity;
        Vector3 cameraRotation = new Vector3(xRot, 0, 0) * MouseSensitivity;

            // Applying the values
        float x = (_cam.transform.eulerAngles - cameraRotation).x;
        if ( x > 180f ? x-360 > -MaxHeadTurn : x < MaxHeadTurn)
            _cam.transform.Rotate(-cameraRotation);
        transform.Rotate(rotation);
    }
    
    
    // Event function
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _characterController = GetComponent<CharacterController>();
        _cam = Camera.main;
    }

    private void Update()
    {
        if (!inInterface)
        {
            HandleRotation();   
        }
        HandleMovement();
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            player.inventoryManager.SwitchInventory();
        }

        if (Input.inputString != null)
        {
            bool isNumber = int.TryParse(Input.inputString, out int number);
            if (isNumber && number > 0 && number < 9)
            {
                player.inventoryManager.ChangeSelectedSlot(number - 1);
            }
        }
        
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (player.inventoryManager.selectedSlot == 7)
                player.inventoryManager.ChangeSelectedSlot(0);
            else
                player.inventoryManager.ChangeSelectedSlot(player.inventoryManager.selectedSlot+1);
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (player.inventoryManager.selectedSlot == 0)
                player.inventoryManager.ChangeSelectedSlot(7);
            else
                player.inventoryManager.ChangeSelectedSlot(player.inventoryManager.selectedSlot-1);
        }

        RaycastHit lookat;
        if (Physics.Raycast(_cam.transform.position, _cam.transform.forward, out lookat, player.inventoryManager.pickupRange))
        {
            Item item = lookat.transform.gameObject.GetComponentInParent<Item>();
            if (item is not null && item.CanBePickedUp)
            {
                player.inventoryManager.interactionText.gameObject.SetActive(true);
                player.inventoryManager.interactionText.text = item.ToString();
                if (Input.GetKeyDown(KeyCode.F))
                {
                    player.inventoryManager.InventoryInteraction(item.netIdentity, InventoryManager.InvAction.Pickup);
                }
            }
            else
            {
                player.inventoryManager.interactionText.gameObject.SetActive(false);
            }
        }
        
        player.inventoryManager.WeaponInHands();
        
    }
}