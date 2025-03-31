using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMovement : MonoBehaviour
{
    Rigidbody2D _rb;

    [SerializeField]float targetGravityScale = 3;
    [SerializeField]float jumpDuration = 0.2f;
    
    //========================================
    public void Init()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void ActivateGravity(bool isOn)
    {
        if( isOn )
        {
            _rb.gravityScale = targetGravityScale;
        }
        else
        {
            _rb.gravityScale = 0;
        }
        
    }


    //===================================================
    public void OnIdle()
    {
        _rb.velocity = new Vector2( 0, 0);
    }

    public void OnStart_Move()
    {

    }


    public void OnStart_Attack()
    {
        _rb.velocity = new Vector2(0, _rb.velocity.y);
        _rb.gravityScale = targetGravityScale;
    }


    public void OnStart_Jump()
    {

    }

    public void OnStart_Die()
    {
        _rb.velocity = Vector2.zero;
        _rb.gravityScale= 0;
    }


    // 움직임
    public void Move(float movementSpeed)
    {
        _rb.velocity = new Vector2(- movementSpeed, _rb.velocity.y);
    }

    public IEnumerator JumpRoutine(float movementSpeed, float jumpPower)
    {
        // 점프 - 
        float p1= jumpDuration*0.8f;
        _rb.velocity = new Vector2( 0, jumpPower);
        yield return new WaitForSeconds(p1);

        // 속력보정 : 여기까지 진행되면 몬스터가 앞 몬스터 머리 위에 있다.
        _rb.velocity = new Vector2( -movementSpeed, jumpPower);
        yield return new WaitForSeconds(jumpDuration - p1);
    }


}
