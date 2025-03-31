using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class EnemyEntity : MonoBehaviour, IDamageable
{
    Transform _t;
    protected Transform myTransform =>_t ??= transform;
    public Vector2 currPos => myTransform.position;

    [Header("Damageable Setting")]
    [SerializeField] protected float _currHp = 100;
    [SerializeField] protected float _maxHp;
    //
    public float currHp{get {return _currHp;} set{ _currHp = Mathf.Clamp(value,0,maxHp);}}
    public float maxHp {get {return _maxHp;} set{ _maxHp = value;}}
    //
    public bool isAlive => currHp>0;

    //=============================================================================
    public abstract void Init(int lineNum,Vector2 initPos);

    public abstract void Die();                         // 인터페이스 함수

    public abstract void OnTakeDamage(Vector2 hitPoint, float amount);    // 인터페이스 함수    

    //===========================================================================

    public void TakeDamage(Vector2 hitPoint, float amount)
    {
        if( isAlive == false)
        {
            return;
        }

        currHp -= amount;
        OnTakeDamage(hitPoint, amount);

        if( isAlive == false)
        {
            Die();
        }
    }


    
}
