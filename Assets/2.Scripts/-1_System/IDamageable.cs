using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable 
{  
    public float currHp{get;set;}
    public float maxHp{get;set;}

    public bool isAlive {get;}

    public void OnTakeDamage(Vector2 hitPoint, float amount);
    public void Die();
}
