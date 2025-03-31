using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    [SerializeField] float _currHp;
    [SerializeField] float _maxHp = 50;
    public float currHp;
    public float maxHp;
    public bool isAlive => _currHp>0;

    [SerializeField] Weapon weapon;


    bool _initialized;

    public void Init()
    {
        weapon = GetComponentInChildren<Weapon>();
        weapon.Init();
        _currHp = _maxHp;



        //
        _initialized= true;
    }



    void Update()
    {
        if ( isAlive == false)
        {
            return;
        }

        //
        if( weapon.TryUse())
        {
            //애니메이션 재생
        }
    }
}
