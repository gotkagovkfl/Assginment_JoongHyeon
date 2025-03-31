using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [SerializeField] Sprite targetSprite;
    [SerializeField] List<Transform> _backgrounds;  // 3개의 배경 오브젝트 (다 같은 )
    [Range(0,1)][SerializeField] float weight =1f; // 먼 배경은 좀 더 느리게 움직이도록 보정
    [SerializeField] float width;
    [SerializeField] int targetSortLayer;

    public void Init()
    {
        //
        if( _backgrounds.Count<2)
        {
            return;
        }

        //
        SpriteRenderer firstSr = _backgrounds[0].GetComponent<SpriteRenderer>();
        width = firstSr.bounds.size.x;
        targetSprite = firstSr.sprite;
        firstSr.sortingOrder = targetSortLayer;
    
    
        // 맨처음 이미지로 통일시키고 위치를 맞춤. 
        for(int i=1;i<_backgrounds.Count;i++)
        {
            var currBg  = _backgrounds[i];
            var prevBgPos = _backgrounds[i-1].position;
            currBg.position = prevBgPos + new Vector3(width,0,0);
            if(currBg.TryGetComponent(out SpriteRenderer sr))
            {
                sr.sprite = targetSprite;
                sr.sortingOrder = targetSortLayer;
            }
        }
    }

    public void Scroll(float targetSpeed_x)
    {
        foreach (Transform bg in _backgrounds)
        {
            // 배경을 왼쪽으로 이동
            bg.position += new Vector3( targetSpeed_x * weight,0,0);
            
            //  임계를 넘어가면 다시 오른쪽으로 이동 
            if (bg.position.x < -width)
            {
                float rightMostX = GetMaxX();
                bg.position = new Vector3(rightMostX + width, bg.position.y, bg.position.z);
            }
        }
    }


    // 가장 오른쪽 배경의 x값 찾기
    float GetMaxX()
    {
        float maxX = float.MinValue;
        foreach (Transform bg in _backgrounds)
        {
            if (bg.position.x > maxX)
                maxX = bg.position.x;
        }
        return maxX;
    }
}
