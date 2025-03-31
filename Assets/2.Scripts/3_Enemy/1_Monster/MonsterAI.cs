using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;




public class MonsterAI : MonoBehaviour
{
    enum MonsterPositionState
    {
        Default,
        BlockedByMonster,
        PlayerInAttackRange,

    }

    //
    Monster monster;

    [Header("Update Setting")]
    [SerializeField] float aiUpdateTime = 0.5f;
    [SerializeField] float lastAiUpdateTime = -2;
    bool canUpdateAI => Time.time>= lastAiUpdateTime + aiUpdateTime;

    [Header("Sensor")]
    [SerializeField] int layerNum;
    

    [SerializeField] Transform t_bottomSensor;    // 바닥센서 : 지면or 무언가 위
    [SerializeField] Transform t_forwardSensor;  // 전방센서 : 장애물감지
    [SerializeField] Transform t_backSensor;    // 후방센서 : 점프가능여부감지

    [SerializeField] float _bottomSensorRadius = 0.18f; // 바닥센서 감지반경
    [SerializeField] float _forwardSensorRange = 0.1f;
    [SerializeField] float _backSensorRange = 0.5f;
    

    // public Vector2 forwardSensorPos => t_forwardSensor.position;
    // public float forwardSensorRange => _forwardSensorRange;


    //=======================================================================
    public void Init(Monster monster,int layerNum)
    {
        this.monster = monster;
        this.layerNum = layerNum;
    }

    

    // 다음 fsm 상태 계산
    public bool TryUpdate(out MonsterState nextState)
    {
        //
        nextState = MonsterState.Idle; 
        if( canUpdateAI ==false || monster.isJumping)
        {
            return false;
        }
        lastAiUpdateTime = Time.time;


        //
        MonsterPositionState newState = GetNewPositionState();

        // 앞에 플레이어가 있으면,
        if ( newState == MonsterPositionState.PlayerInAttackRange )
        {
            nextState = MonsterState.Attack;
        }
        // 앞이 몬스터로 막히면
        else if ( newState == MonsterPositionState.BlockedByMonster  )
        {
            if( CanJump())
            {
                nextState = MonsterState.Jump;
            }
            else
            {
                nextState = MonsterState.Idle;
            }    
        }
        // 이동 
        else
        {
            nextState = MonsterState.Move; 
        }

        return true;
    }

    //=================================================================================================

    bool CanJump()
    {
        return monster.isJumping == false && monster.isGrounded && OtherOnBackside()==false;
    }


    // 상태검사 
    // 뒤에 몬스터가 있는지, (점프 가능한지 판별용)
    public bool OtherOnBackside()
    {
        RaycastHit2D hit = Physics2D.Raycast(t_backSensor.position, Vector2.right, _backSensorRange, 1<< layerNum );
        return hit.collider!=null && hit.collider.gameObject != gameObject;
    }

    // 
    [SerializeField] Vector2 checkedPos;
    public bool IsGrounded(Vector2 currPos)
    {
        Vector2 checkPos = (Vector2)t_bottomSensor.position;
        checkedPos = checkPos;
        var hits = Physics2D.OverlapCircleAll(checkPos, _bottomSensorRadius,1<<layerNum);
        foreach( var hit in hits)
        {
            if (hit.gameObject!=gameObject)
            {
                return true;
            }
        }
        
        return false;
    }


    // 몬스터 앞에 있는 장애물 검사
    MonsterPositionState GetNewPositionState()
    {
        //
        var hit = Physics2D.Raycast(t_forwardSensor.position - new Vector3(float.Epsilon,0), Vector2.left, _forwardSensorRange, 1<<GameConstants.truckLayer| 1<<layerNum);

        MonsterPositionState ret = MonsterPositionState.Default;
        if( hit.collider == null)
        {
            return ret;
        }

        //
        int hitLayer = hit.collider.gameObject.layer;
        if( hitLayer  == GameConstants.truckLayer)
        {
            ret = MonsterPositionState.PlayerInAttackRange;
        }
        else if (hitLayer  == layerNum)
        {
            ret = MonsterPositionState.BlockedByMonster;
        }

        return ret;
    }

    public Collider2D GetColliderInAttackRange()
    {

        var ret = Physics2D.OverlapCircle(t_forwardSensor.position,_forwardSensorRange, 1<<GameConstants.BoxLayer| 1<<GameConstants.HeroLayer);

        return ret;
    }





    // 센서 범위 표시
    void OnDrawGizmos()
    {
        if(monster==null || t_forwardSensor==null || t_backSensor==null || t_bottomSensor==null )
        {
            return;
        }


        // 전방 센서
        Gizmos.color = Color.red;
        Vector3 start = t_forwardSensor.position;
        Vector3 end = start + new Vector3(-_forwardSensorRange,0);
        Gizmos.DrawLine(start, end);

        // 후방 센서
        Gizmos.color = Color.green;
        start = t_backSensor.position;
        end = start + new Vector3(_backSensorRange, 0);
        
        Gizmos.DrawLine(start , end);

        // 바닥 센서
        Gizmos.color = monster.gravityOn? Color.yellow: Color.blue;
        // Vector2 size = _bottomSensor.size;  
        Gizmos.DrawWireSphere(checkedPos , _bottomSensorRadius);
    }

}
