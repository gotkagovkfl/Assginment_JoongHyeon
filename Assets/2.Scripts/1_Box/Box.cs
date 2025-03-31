using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxStatus))]
[RequireComponent(typeof(BoxUI))]
public class Box : MonoBehaviour
{

    BoxCollider2D _collider;
    Truck truck;

    BoxStatus status;
    BoxUI boxUi;



    public void Init(Truck truck)
    {
        this.truck = truck;
        _collider = GetComponent<BoxCollider2D>();
        _collider.enabled = true;

        //
        status = GetComponent<BoxStatus>();
        status.Init();

        boxUi = GetComponent<BoxUI>();
        boxUi.Init(this,status.currHp,status.maxHp);

    }


    public void TakeDamage(float amount)
    {
        truck.Decelerate(0.1f);
        status.ChangeCurrHp(-amount);
        boxUi.OnBoxHpChanged(status.currHp,status.maxHp);

        // 파괴검사
        if (status.isAlive==false)
        {
            StartCoroutine(DeathRoutine());
        }
    }

    IEnumerator DeathRoutine()
    {
        _collider.enabled = false;
        boxUi.OnBoxDestroyed();        
        yield return null;
    }
}
