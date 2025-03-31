using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class DamageText : MonoBehaviour
{
    
    Transform _t;
    Vector2 initPos;
    

    [SerializeField] float lifeTime = 1f;
    [SerializeField] TextMeshPro text;


    [Header("Animation")]
    [SerializeField] float hMove =2;   // 수평 움직임
    [SerializeField] float vMove = 1;   // 수직 움직임

    static WaitForEndOfFrame wfef = new();

    //====================================================================
    public void Init(float amount)
    {
        text = GetComponent<TextMeshPro>();
        text.SetText($"{amount}");


        _t = transform;
        initPos = _t.position;
        //
        PlayAnim();
        StartCoroutine(LifeRoutine());
    }


    //
    IEnumerator LifeRoutine()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }


    // 애니메이션 (오른쪽으로 포물선 그리면서 투명해짐)
    void PlayAnim()
    {
        StartCoroutine(MoveRoutine());
        StartCoroutine(FadeRoutine());
    }






    //  오른쪽으로 포물선
    IEnumerator MoveRoutine()
    {
        float elapsed = 0f;

        while (elapsed < lifeTime)
        {
            float ratio = elapsed / lifeTime;

            float x = initPos.x + ratio * hMove;
            float y = initPos.y + Mathf.Sin(ratio * Mathf.PI) * vMove;

            _t.position = new Vector2(x, y);

            elapsed += Time.deltaTime;
            yield return wfef;
        }
    }


    // 점차 페이드
    IEnumerator FadeRoutine()
    {
        yield return new WaitForSeconds(lifeTime*0.5f);
        float elapsed=0;

        float targetDuration = lifeTime*0.5f;
        while(elapsed <targetDuration)
        {
            float ratio = 1-elapsed / targetDuration;
            text.color = new Color(1,1,1,ratio);

            elapsed += Time.deltaTime;
            yield return wfef;
        }
    }
}
