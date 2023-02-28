using UnityEngine;
public class ZombieCharacterControl : MonoBehaviour
{
    private enum ControlMode
    {
        Tank,
        Direct
    }

    [SerializeField] private float m_moveSpeed = 2;
    [SerializeField] private float m_turnSpeed = 200;
    [SerializeField] private float m_detectionDistance = 10;
    [SerializeField] private float m_attackDistance = 1;
    [SerializeField] private Animator m_animator = null;
    [SerializeField] private Rigidbody m_rigidBody = null;
    [SerializeField] private ControlMode m_controlMode = ControlMode.Tank;

    private float m_currentV = 0;
    private float m_currentH = 0;
    private readonly float m_interpolation = 10;
    private Vector3 m_currentDirection = Vector3.zero;
    private GameObject m_player = null;

    private void Awake()
    {
        if (!m_animator)
        {
            m_animator = gameObject.GetComponent<Animator>();
        }

        if (!m_rigidBody)
        {
            m_rigidBody = gameObject.GetComponent<Rigidbody>();
        }

        m_player = GameObject.FindGameObjectWithTag("Player");
    }

    private void FixedUpdate()
    {
        Vector3 playerDirection = m_player.transform.position - transform.position;
        float distanceToPlayer = playerDirection.magnitude;

        if (distanceToPlayer > m_detectionDistance)
        {
            m_controlMode = ControlMode.Tank;
        }
        else if (distanceToPlayer > m_attackDistance)
        {
            m_controlMode = ControlMode.Direct;
        }

        switch (m_controlMode)
        {
            case ControlMode.Direct:
                DirectUpdate(playerDirection);
                break;

            case ControlMode.Tank:
                TankUpdate();
                break;

            default:
                Debug.LogError("Unsupported state");
                break;
        }

        // Attack player when in range
        if (distanceToPlayer <= m_attackDistance)
        {
            AttackPlayer();
        }
    }

    private void TankUpdate()
    {
        float v = 1;
        float h = 0;

        m_currentV = Mathf.Lerp(m_currentV, v, Time.deltaTime * m_interpolation);
        m_currentH = Mathf.Lerp(m_currentH, h, Time.deltaTime * m_interpolation);
        

        m_animator.SetFloat("MoveSpeed", m_currentV);
    }

    private void DirectUpdate(Vector3 playerDirection)
    {
        playerDirection.y = 0;

        m_currentDirection = Vector3.Slerp(m_currentDirection, playerDirection, Time.deltaTime * m_interpolation);
        transform.rotation = Quaternion.LookRotation(m_currentDirection);
        transform.position += transform.forward * m_moveSpeed * Time.deltaTime;
    }

    private void AttackPlayer()
    {
        m_animator.SetTrigger("Attack");
    }
}