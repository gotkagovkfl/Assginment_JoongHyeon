using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;


[RequireComponent(typeof(Rigidbody2D))]
public class PlayerProjectile : MonoBehaviour
{


    Weapon weapon;
    EnemyEntity target;
    Rigidbody2D _rb;
    CircleCollider2D _collider;

    [SerializeField] Vector2 dir;
    [SerializeField] float movementSpeed = 10f;
    [SerializeField] float lifeTime = 3f;
    
    

    // 방향 설정하고 
    public void Init(Weapon weapon,EnemyEntity target,Vector2 dir)
    {
        this.weapon = weapon;
        this.target = target;
        this.dir = dir;

        _collider = GetComponent<CircleCollider2D>();
        _rb = GetComponent<Rigidbody2D>();

        SetDirAndMove();

        
        StartCoroutine(DestroyRoutine());
    }


    void SetDirAndMove()
    {
        _rb.velocity = dir* movementSpeed;
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Monster") && other.TryGetComponent(out EnemyEntity enemyEntity))
        {
            Vector2 hitPoint = other.ClosestPoint(transform.position);
            enemyEntity.TakeDamage(hitPoint,weapon.damage);
            DestroyProjectile();
        }
    }


    IEnumerator DestroyRoutine()
    {
        yield return new WaitForSeconds(lifeTime);

        DestroyProjectile();
    }

    void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}
