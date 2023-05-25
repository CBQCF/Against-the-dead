using System;
using Mirror;
using UnityEngine;
using UnityEngine.Serialization;

public class ZombieCharacterControl : NetworkBehaviour
{
    private enum ControlMode
    {
        Tank,
        Direct
    }

    public float attackDelay = 1.0f; // Délai avant la première attaque
    public float attackInterval = 1.5f; // Intervalle entre les attaques successives

    private bool isAttacking = false;
    private float timeSinceLastAttack = 0f;
    public float mMoveSpeed = 2;
    public float mTurnSpeed = 200;
    public float mDetectionDistance = 10;
    public float mAttackDistance = 2;
    public Animator mAnimator;

    //private bool _shouldAttack = false;
    private ControlMode _mControlMode = ControlMode.Tank;
    private float _mCurrentV;
    private float _mCurrentH;
    private readonly float _mInterpolation = 10;
    private Vector3 _mCurrentDirection = Vector3.zero;
    public ServerInfo serverInfo;

    private void Awake()
    {
        mAnimator = GetComponent<Animator>();
    }
    
    private void LateUpdate()
    {
        GameObject[] mPlayers = serverInfo.playerList;

        for(int i = 0; i < mPlayers.Length; i++)
        {
            GameObject player = mPlayers[i];
            Vector3 playerDirection = player.transform.position - transform.position;
            float distanceToPlayer = playerDirection.magnitude;

            if (distanceToPlayer > mDetectionDistance)
            {
                _mControlMode = ControlMode.Tank;
                mAnimator.SetBool("should_attack", false);
                mAnimator.SetFloat("distanceDetection", distanceToPlayer);
            }
            else if (distanceToPlayer > mAttackDistance)
            {
                mAnimator.SetFloat("distanceDetection", distanceToPlayer);
                mAnimator.SetBool("should_attack", false);
                _mControlMode = ControlMode.Direct;
            }

            if (distanceToPlayer <= mAttackDistance)
            {
                if (!isAttacking)
                {
                    isAttacking = true;
                    mAnimator.SetBool("should_attack", true);
                    InvokeRepeating(nameof(PerformAttack), attackDelay, attackInterval);
                }
            }
            else
            {
                if (isAttacking)
                {
                    isAttacking = false;
                    mAnimator.SetBool("should_attack", false);
                    CancelInvoke(nameof(PerformAttack));
                }
            }

            switch (_mControlMode)
            {
                case ControlMode.Direct:
                    DirectUpdate(playerDirection);
                    break;

                case ControlMode.Tank:
                    TankUpdate();
                    break;
            }
        }
    }

    private void PerformAttack()
    {
        GameObject[] mPlayers = serverInfo.playerList;

        foreach (GameObject player in mPlayers)
        {
            player.GetComponent<Player>().CmdInflictDamage(10);
            Debug.Log("Le zombie attaque");
        }
    }

    private void TankUpdate()
    {
        float v = 1;
        float h = 0;

        _mCurrentV = Mathf.Lerp(_mCurrentV, v, Time.deltaTime * _mInterpolation);
        _mCurrentH = Mathf.Lerp(_mCurrentH, h, Time.deltaTime * _mInterpolation);

        mAnimator.SetFloat("MoveSpeed", _mCurrentV);
    }

    private void DirectUpdate(Vector3 playerDirection)
    {
        playerDirection.y = 0;
        float distanceToPlayer = playerDirection.magnitude;
        if (mDetectionDistance >= distanceToPlayer)
        {
            _mCurrentDirection = Vector3.Slerp(_mCurrentDirection, playerDirection, Time.deltaTime * _mInterpolation);
            transform.rotation = Quaternion.LookRotation(_mCurrentDirection);
            transform.position += transform.forward * mMoveSpeed * Time.deltaTime;
        }
    }
}