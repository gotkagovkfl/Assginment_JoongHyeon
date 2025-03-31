using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBox : EnemyEntity
{
    float initPosY = -3.62f;

    [SerializeField] Transform t_enemies;

    [SerializeField] Monster prefab_monster;

    [SerializeField] float spawnInterval = 3f;


    //==============================================================
    public override void Init(int lineNum, Vector2 initPos)
    {
        _currHp = _maxHp;

        StartCoroutine( SpawnRoutine() );
    }

    public override void Die()
    {
        Debug.Log("스폰 박스 파괴!");
        GamePlayManager.Instance.ClearCurrSection();
        Destroy(gameObject);
    }

    public override void OnTakeDamage(Vector2 hitPoint, float amount)
    {
        GamePlayManager.Instance.OnEnemyDamaged(hitPoint, amount);
    }
    //===============================


    //===================
    IEnumerator SpawnRoutine()
    {
        
        WaitForSeconds wfs = new(spawnInterval);
        while( true )
        {
            SpawnMonster();


            yield return wfs;
        }
    }

    void SpawnMonster()
    {

        int rand = UnityEngine.Random.Range(0,3);
        // int rand = 0;

        Monster monster = Instantiate(prefab_monster.gameObject,t_enemies).GetComponent<Monster>();
        monster.Init(rand,transform.position);
    }


    void OnGUI()
    {
        GUIStyle btnStyle = new GUIStyle(GUI.skin.button);
        btnStyle.fontSize = 40; 
        
        if (GUI.Button(new Rect(50, 50, 300, 100), "적 생성",btnStyle))
        {
            SpawnMonster();
        }
    }


}
