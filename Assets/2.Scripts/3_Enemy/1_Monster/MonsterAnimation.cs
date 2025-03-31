using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAnimation : MonoBehaviour
{
    Animator _animator;
    static readonly int animId_move = Animator.StringToHash("IsIdle");
    static readonly int animID_jump = Animator.StringToHash("IsIdle");   // 점프 애니메이션 없음
    static readonly int animId_attack = Animator.StringToHash("IsAttacking");
    static readonly int animDI_die = Animator.StringToHash("IsDead");


    //==============================================================================
    public void Init()
    {
        _animator = GetComponent<Animator>();
    }

    public void OnStart_Idle()
    {
        _animator?.SetBool(animId_attack,false);
        _animator?.SetBool(animId_move,true);
    }


    public void OnStart_Move()
    {
        _animator?.SetBool(animId_attack,false);
        _animator?.SetBool(animId_move,true);
    }


    public void OnStart_Attack()
    {
        _animator?.SetBool(animId_attack,true);
    }


    public void OnStart_Jump()
    {
        _animator?.SetBool(animId_attack,false);
        _animator?.SetBool(animID_jump,true);
  
    }

    public void OnStart_Die()
    {
        _animator?.SetBool(animDI_die,true);
    }


}
