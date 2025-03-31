using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Truck : MonoBehaviour
{
    [SerializeField] Transform t_boxes;
    [SerializeField] List<Box> boxes;
    [SerializeField] Hero hero;


    public bool isAlive => true; // 박스랑 히어로가 다 파괴되면 false 조건 걸기
    bool isCloseToSpawnBox;


    [Header("Move")]
    [SerializeField] GamePlaySection currSection;
    [SerializeField] List<BackgroundScroller> backgroundScrollers;

    [SerializeField] float movementSpeed_curr = 0;
    [SerializeField] float movementSpeed_max = 3;
    [SerializeField] float movementSpeed_rrps = 0.3f;  //recoverRatio per second

    [SerializeField] bool canMove;
    [SerializeField] bool stucked => Time.time < lastStuckTime + stuckRecoverTime;
    [SerializeField] float lastStuckTime;
    [SerializeField] float stuckRecoverTime = 1f;
    

    public void Init(GamePlaySection gamePlaySection,List<BackgroundScroller> backgroundScrollers)
    {
        // _wallCollider = GetComponent<BoxCollider2D>();

        boxes = new();
        for(int i=0;i<t_boxes.childCount;i++)
        {
            if(t_boxes.GetChild(i).TryGetComponent(out Box box))
            {
                box.Init(this);
                boxes.Add(box);
            }
        }    

        hero = GetComponentInChildren<Hero>();
        hero.Init();


        // 움직이게할 오브젝트들 할당
        currSection = gamePlaySection;
        this.backgroundScrollers = backgroundScrollers;

        //
        movementSpeed_curr = movementSpeed_max;
        canMove = true;
    }

    void FixedUpdate()
    {
        if( isAlive==false)
        {
            return;
        }

        //
        if( canMove)
        {
            // 밀린 후 일정 시간 후에 다시 가속할 수 있도록
            if(stucked==false )
            {
                Accelerate(movementSpeed_rrps * Time.fixedDeltaTime);
            }

            Move();

            // 정지 체크 
            if(isCloseToSpawnBox == false && currSection.cleared == false &&  transform.position.x >=currSection.truckStopPointX )
            {
                
                Debug.Log("브레이크");
                Break(1f);
            }
        }
    }

    // 트럭이 움직이는 게 아니라 배경과 오브젝트가 움직이는것
    void Move()
    {
        float targetSpeed_x = - movementSpeed_curr* Time.fixedDeltaTime;

        currSection.Move(targetSpeed_x);     // 스폰 박스 & 게임 오브젝트
        foreach( BackgroundScroller bg in backgroundScrollers)   // 배경 이미지
        {
            bg.Scroll(targetSpeed_x);
        }
    }

    //가속
    void Accelerate(float ratio)
    {
        float amount = movementSpeed_max*ratio;
        movementSpeed_curr = Mathf.Clamp(movementSpeed_curr + amount,0,movementSpeed_max); 
    }

    // 감속
    public void Decelerate(float ratio)
    {
        lastStuckTime = Time.time;
        
        float amount = movementSpeed_max*ratio;
        movementSpeed_curr = Mathf.Clamp(movementSpeed_curr - amount,0,movementSpeed_max);
    }


    // 브레이크 (스폰박스 앞에서)
    public void Break(float duration)
    {
        isCloseToSpawnBox = true;
        StartCoroutine(BreakRoutine(duration));
    }

    IEnumerator BreakRoutine(float duration)
    {
        float elapsed = 0;
        float ratio = Time.fixedDeltaTime/ duration;
        while( elapsed<duration)
        {
            Decelerate(ratio);

            elapsed += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        canMove = false;
        
        yield return new WaitUntil(()=>currSection.cleared);
        canMove = true;
        isCloseToSpawnBox = false;
    }
}
