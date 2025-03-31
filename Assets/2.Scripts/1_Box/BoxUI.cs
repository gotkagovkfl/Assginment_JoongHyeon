using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoxUI : MonoBehaviour
{

    Box box;
    [SerializeField] GameObject uiObject;
    [SerializeField] Slider hpSlider;

    Coroutine activationRoutine;



    public void Init(Box box,float currHp,float maxHp)
    {
        this.box=box;

        hpSlider.maxValue = maxHp;
        hpSlider.value = currHp;

        uiObject?.SetActive(false);
    }


    public void OnBoxHpChanged(float currHp,float maxHp)
    {
        //
        hpSlider.maxValue = maxHp;
        hpSlider.value = currHp;


        //
        if(activationRoutine!=null)
        {
            StopCoroutine(activationRoutine);
        }
        activationRoutine = StartCoroutine(ActivationRoutine());
    }

    // 피해 받지 않으면 2초뒤에 꺼짐. 
    IEnumerator ActivationRoutine()
    {
        uiObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        uiObject.SetActive(false);
    }

    // 박스파괴시.
    public void OnBoxDestroyed()
    {
        if(activationRoutine!=null)
        {
            StopCoroutine(activationRoutine);
        }
        gameObject.SetActive(false);
    }


}
