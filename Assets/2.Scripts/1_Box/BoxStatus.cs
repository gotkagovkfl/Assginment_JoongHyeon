using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxStatus : MonoBehaviour
{
    [SerializeField] float _currHp;
    [SerializeField] float _maxHp;


    public float currHp =>_currHp;
    public float maxHp =>_maxHp;


    public bool isAlive => _currHp>0;



    public void Init()
    {
        _currHp = maxHp;
    }

    public void ChangeCurrHp(float amount)
    {
        _currHp = Mathf.Clamp(_currHp+amount,0, _maxHp );
    }
}
