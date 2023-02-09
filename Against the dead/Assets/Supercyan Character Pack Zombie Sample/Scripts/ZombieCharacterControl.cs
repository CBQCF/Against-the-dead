using UnityEngine;

public class ZombieCharacterControl : MonoBehaviour
{
    [SerializeField] private float m_moveSpeed = 2;
    [SerializeField] private GameObject player;
    [SerializeField] private Animator m_animator = null;
    [SerializeField] private Rigidbody m_rigidBody = null;

    private Transform m_playerTransform;

    private void Awake()
    {
        if (!m_animator) { gameObject.GetComponent<Animator>(); }
        if (!m_rigidBody) { gameObject.GetComponent<Animator>(); }
    }

    private void Start()
    {
        m_playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void FixedUpdate()
    {
        transform.LookAt(m_playerTransform);
        transform.Translate(Vector3.forward * m_moveSpeed * Time.deltaTime);
    }
}