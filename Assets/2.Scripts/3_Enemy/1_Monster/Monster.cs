using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;




public enum MonsterState
{
    Idle,
    Move,
    Jump,
    Attack,
    Nockback,
    Die,
}

// public enum Monster

public class Monster : EnemyEntity
{
    Collider2D _collider;   // 물리 적용 콜라이더 

    

    //
    MonsterAnimation mAnimation;
    MonsterMovement mMovement;
    MonsterAI mAi;



    // ----- status
    [SerializeField] int lineNum;
    [SerializeField] float initY;
    [SerializeField] int layerNum;

    
    [Header("Status")]
    [SerializeField] float _movementSpeed = 3f;
    [SerializeField] float _jumpPower =5f;
    [SerializeField] float _attackPower = 1f;


    // ---- property
    public float movementSpeed => _movementSpeed;
    public float jumpPower=> _jumpPower;
    public float attackPower=> _attackPower;


    // ----- AI
    
    public bool gravityOn;
    [SerializeField] bool _isGrounded;
    [SerializeField] bool _isJumping;
    public bool isGrounded =>_isGrounded;
    public bool isJumping =>_isJumping;

    


    //------ fsm
    [SerializeField] MonsterState currState;
    MonsterFsmState currFsmState;
    Dictionary<MonsterState, MonsterFsmState> fsmStateDic;
    

    //============================================

    public override void Init(int lineNum,Vector2 initPos)
    { 
        // fsm
        fsmStateDic = new(){{MonsterState.Idle, new MFS_Idle(this)},
                            {MonsterState.Move, new MFS_Move(this)},
                            {MonsterState.Jump, new MFS_Jump(this)},
                            {MonsterState.Attack, new MFS_Attack(this)},
                            {MonsterState.Die, new MFS_Die(this) }};
        
        
        
        //
        this.lineNum = lineNum;
        layerNum = GameConstants.GetLineLayerMask(lineNum); 
        initY = GameConstants.GetLineHeight(lineNum);

        myTransform.position = new Vector2(initPos.x,initY);
        gameObject.layer = layerNum;
         
        _collider = GetComponent<PolygonCollider2D>();


        mAnimation = GetComponent<MonsterAnimation>();
        mAnimation.Init();
        mMovement = GetComponent<MonsterMovement>();
        mMovement.Init();
        mAi = GetComponent<MonsterAI>();
        mAi.Init(this,layerNum);


        //
        _currHp = _maxHp;
    }

    void FixedUpdate()
    {
        _isGrounded = mAi.IsGrounded(myTransform.position);
        // 중력 계산
        if(currState == MonsterState.Attack)
        {
            mMovement.ActivateGravity(true);
            gravityOn = true;
        }
        else if(_isGrounded || _isJumping)
        // else 
        {
            mMovement.ActivateGravity(false);
            gravityOn = false;
        }  
        else
        {
        
            mMovement.ActivateGravity(true);
            gravityOn = true;
        }
    }


    void Update()
    {
        if( isAlive == false)
        {
            return;
        }

        // ai 가 계산한 다음 상태로 전이 및 실행 
        if( mAi.TryUpdate(out MonsterState nextState) )
        {            
            //
            ChangeFsmState(nextState);
            currFsmState?.Update();
        }
    }

    
    //===========================================================================
    void ChangeFsmState(MonsterState newState)
    {
        if( currState == newState)
        {
            return;
        }
        currState = newState;

        currFsmState?.Exit();
        currFsmState = fsmStateDic[newState];
        currFsmState?.Enter();
    }


    public override void OnTakeDamage(Vector2 hitPoint, float amount)
    {
        GamePlayManager.Instance.OnEnemyDamaged(hitPoint, amount);
    }

    public override void Die()
    {
        StartCoroutine(DeathSequnce());
    }


    #region Behaviour
    // 정지상태
    public void SetIdle()
    {
        mAnimation.OnStart_Idle();
        mMovement.OnIdle();
    }

    // 이동
    public void StartMove()
    {
        mAnimation.OnStart_Move();
    }
    public void Move()
    {
        mMovement.Move(movementSpeed);
    }

    // 공격
    public void StartAttack()
    {
        mMovement.OnStart_Attack();
        mAnimation.OnStart_Attack();
    }

    // 점프 시도
    public void Jump()
    {        
        _isJumping = true;
        mAnimation.OnStart_Jump();
        StartCoroutine(JumpRoutine());
    }
    public void FinisihJump()
    {
        _isJumping = false;
    }


    IEnumerator JumpRoutine()
    {
        yield return mMovement.JumpRoutine(movementSpeed, jumpPower);
        
        //
        ChangeFsmState(MonsterState.Move);
    }

    // 사망
    public void OnDie()
    {
        mAnimation.OnStart_Die();
        mMovement.OnStart_Die();
    }

    IEnumerator DeathSequnce()
    {
        _collider.enabled=false;
        ChangeFsmState(MonsterState.Die);


        yield return new WaitForSeconds(0.5f);

        Destroy(gameObject);
    }


    #endregion






    // 공격적용.
    void OnAttack(AnimationEvent animationEvent)
    {
        // Debug.Log("공격");
        Collider2D collider = mAi.GetColliderInAttackRange();
        if( collider!=null &&collider.TryGetComponent(out Box box))
        {
            box.TakeDamage(attackPower);
        }
    }


}
