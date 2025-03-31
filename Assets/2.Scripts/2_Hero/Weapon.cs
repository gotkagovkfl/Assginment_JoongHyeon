using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.VFX;

public class Weapon : MonoBehaviour
{
    Truck truck;
    [SerializeField] Transform t_muzzle;


    [Header("Status")]
    [SerializeField] float _damage = 10f;
    [SerializeField] float _range = 10f;
    public float damage=>_damage;
    [SerializeField] int bulletCount = 5;
    [SerializeField] float spreadAngle = 30f;
    
    float anglePerBullet => bulletCount>1? spreadAngle/(bulletCount-1): 0;
    


    [Header("Use Condition")]
    [SerializeField] float useCooltime = 1.5f;
    [SerializeField] float lastUseTime = -2f;
    [SerializeField] bool useCooltimeOk => Time.time >= lastUseTime + useCooltime; 


    [Header("Target")]
    [SerializeField] bool autoTarget;
    [SerializeField] float defaultRotation = -32.978f; // 무기 기본상태의 회전값 (오른쪽 보도록)
    [SerializeField] EnemyEntity currTarget;

    [SerializeField] float currAngle;
    [SerializeField] float targetAngle;
    [SerializeField] float angularVelocity = 0f; // 내부 상태 저장용 (ref 필요)
    [SerializeField] float smoothTime = 0.1f;
    


    [Header("Other")]
    [SerializeField] PlayerProjectile prefab_playerProjectile;
    [SerializeField] bool available;



    //==========================================================
    public void Init()
    {
        lastUseTime = -2f;
        available = true;
    }



    public bool TryUse()
    {
        if( available==false)
        {
            return false;
        }
        
        //
        if(useCooltimeOk)
        {
            Use();
            return true;
        }

        //
        return false;
    }

    void Update()
    {
        autoTarget = !Input.GetMouseButton(0);   // 마우스 클릭시 자동공격 
        
        CalRotation(currTarget);    
        Rotate();
    }

    // 무기 사용
    void Use()
    {
        // 타겟 탐색
        if(currTarget == null|| currTarget.isAlive ==false)
        {
            EnemyEntity monster = FindTarget();
            currTarget = monster;
        }


        // 무기 방향 지정
        CalRotation(currTarget);

        // 발사
        GenerateBullets();

        //
        lastUseTime = Time.time;

    }


    // 타겟 탐색 - 가장 가까운 적
    EnemyEntity FindTarget()
    {
        Vector2 centerPos = (Vector2)transform.position;

        float  min = float.MaxValue;
        EnemyEntity e_min = null;
        int monsterLayer = GameConstants.totalEnemyLayer;

        //
        Vector2 size = new Vector2(_range*2, _range*2 );
        Collider2D[] hits = Physics2D.OverlapBoxAll(centerPos, size , monsterLayer);
        foreach(var hit in hits)
        {
            EnemyEntity e_curr = hit.GetComponent<EnemyEntity>(); 
            if( e_curr ==null)
            {
                continue;
            }

            float curr = (e_curr.currPos - centerPos).sqrMagnitude;
            if( curr < min)
            {
                min = curr;
                e_min = e_curr;
            }
        }

        //
        if( e_min !=null)
        {
            return e_min;
        }
        return null;
    }

    // 타겟 방향으로의 회전각도 구함
    void CalRotation(EnemyEntity target)
    {
        
        //
        Vector2 dir =  Vector2.zero;
        if(autoTarget)
        {
            if (target== null || target.isAlive == false)
            {    
                return;
            }

            dir =  currTarget.currPos - (Vector2)transform.position;
        }
        else
        {
            Vector2 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            dir =  clickPos - (Vector2)transform.position;

        }
        float angle = Mathf.Clamp(Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg,-90,90);
        targetAngle = angle;
    }

    // 자연스러운 회전
    void Rotate()
    {
        currAngle = Mathf.SmoothDampAngle(currAngle, targetAngle, ref angularVelocity, smoothTime);
        transform.localRotation = Quaternion.Euler(0, 0, currAngle + defaultRotation);
    }

    // 총구 회전 방향 계산 
    public Vector2 GetRotatedDir(float zOffset)
    {
        float baseAngle = transform.eulerAngles.z;
        if (baseAngle > 180f) baseAngle -= 360f;

        float finalAngle = baseAngle - defaultRotation + zOffset;
        float rad = finalAngle * Mathf.Deg2Rad;

        return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
    }



    // 공격
    void GenerateBullets()
    {
        float angleOffset = -spreadAngle*0.5f;
        for(int i=0;i<bulletCount;i++)
        {
            Vector2 dir = GetRotatedDir(angleOffset);
            // 발사
            PlayerProjectile pp = Instantiate(prefab_playerProjectile.gameObject, t_muzzle.position, Quaternion.identity).GetComponent<PlayerProjectile>();
            pp.Init(this,currTarget,dir);


            angleOffset += anglePerBullet;
        }
        
        
       
    }


    //===================================================================
    void OnDrawGizmos()
    {
        // 무기의 적 탐색 범위 표시
        Gizmos.color = Color.magenta;
        Vector2 size = new Vector2(_range*2, _range*2 );
        Gizmos.DrawWireCube(transform.position,size );


        // 현재 무기의 타겟 표시
        if( currTarget !=null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(currTarget.currPos,0.1f);   
        }
    }

    void OnGUI()
    {
        GUIStyle btnStyle = new GUIStyle(GUI.skin.button);
        btnStyle.fontSize = 40; 
        
        if (GUI.Button(new Rect(400, 50, 300, 100), "무기 활성화",btnStyle))
        {
            available = !available;
        }
    }
}
