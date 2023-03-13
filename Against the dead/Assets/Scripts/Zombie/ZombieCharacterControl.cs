using UnityEngine;
using System.Collections.Generic;
using Mirror;

public class ZombieCharacterControl : NetworkBehaviour
{
    private enum ControlMode
    {
        Tank,
        Direct
    }
    
    [SerializeField] private float m_moveSpeed = 2;
    [SerializeField] private float m_turnSpeed = 200;
    [SerializeField] private float m_detectionDistance = 10;
    [SerializeField] private float m_attackDistance = 2;
    [SerializeField] private Animator m_animator = null;
    [SerializeField] private Rigidbody m_rigidBody = null;
    [SerializeField] private ControlMode m_controlMode = ControlMode.Tank;
    private bool should_attack = false;

    private readonly float m_interpolation = 10;
    private Vector3 m_currentDirection = Vector3.zero;
    private List<GameObject> m_players = new List<GameObject>();

    private void Awake()
    {
        // Find all players in the scene
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            // Ignore the zombie's own player object
            if (player != gameObject)
            {
                m_players.Add(player);
            }
        }
    }

    private void FixedUpdate()
    {
        float closestDistanceToPlayer = float.MaxValue;
        Vector3 closestPlayerDirection = Vector3.zero;

        foreach (GameObject player in m_players)
        {
            Vector3 playerDirection = player.transform.position - transform.position;
            float distanceToPlayer = playerDirection.magnitude;

            if (distanceToPlayer < m_detectionDistance && distanceToPlayer < closestDistanceToPlayer)
            {
                closestDistanceToPlayer = distanceToPlayer;
                closestPlayerDirection = playerDirection;
            }
        }

        if (closestDistanceToPlayer == float.MaxValue)
        {
            // No players within detection distance
            m_controlMode = ControlMode.Tank;
            m_animator.SetBool("should_attack", false);
            m_animator.SetFloat("distanceDetection", 0);
        }
        else if (closestDistanceToPlayer > m_attackDistance)
        {   
            // Player within detection distance but not attack distance
            m_animator.SetFloat("distanceDetection", closestDistanceToPlayer);
            m_animator.SetBool("should_attack", false);
            m_controlMode = ControlMode.Direct;
        }
        else
        {
            // Player within attack distance
            m_animator.SetBool("should_attack", true);
            m_controlMode = ControlMode.Tank;
        }
        
        switch (m_controlMode)
        {
            case ControlMode.Direct:
                DirectUpdate(closestPlayerDirection);
                break;

            case ControlMode.Tank:
                TankUpdate();
                break;

            default:
                Debug.LogError("Unsupported state");
                break;
        }
        
    }

    private void TankUpdate()
    {
        float v = 1;
        float h = 0;

        m_animator.SetFloat("MoveSpeed", v);

        m_currentDirection = Vector3.Slerp(m_currentDirection, transform.forward, Time.deltaTime * m_interpolation);
        transform.rotation = Quaternion.LookRotation(m_currentDirection);
        transform.position += transform.forward * m_moveSpeed * Time.deltaTime;
    }

    private void DirectUpdate(Vector3 playerDirection)
    {
        playerDirection.y = 0;
        m_currentDirection = Vector3.Slerp(m_currentDirection, playerDirection, Time.deltaTime * m_interpolation);
        transform.rotation = Quaternion.LookRotation(m_currentDirection);
        transform.position += transform.forward * m_moveSpeed * Time.deltaTime;
    }
}