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
    
    
    private void FixedUpdate()
    {
        GameObject[] mPlayers = serverInfo.playerList;
        
        foreach (GameObject player in mPlayers)
        {
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

            // Attack player when in range
            if (distanceToPlayer <= mAttackDistance)
            {
                mAnimator.SetBool("should_attack", true);
                // faire perdre de la vie au joueur
                //appeler AddHealth dans le script Stats pour lui faire perdre la vie
                //ajouter dans addHealth une condition pour savoir si c'est un joueur ou pas (regarder le tag) 
                //si c'est un joueur afficher un gameOver
                _mControlMode = ControlMode.Tank;
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