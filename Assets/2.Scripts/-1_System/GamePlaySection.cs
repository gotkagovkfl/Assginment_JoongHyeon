using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlaySection : MonoBehaviour
{

    Transform _t;
    [SerializeField] SpawnBox spawnBox;
    [SerializeField] Transform t_truckStopPoint;

    [SerializeField] bool _cleared;
    public bool cleared =>_cleared;

    public float truckStopPointX => t_truckStopPoint.position.x;

    public void Init()
    {
        _t = transform;
        spawnBox.Init(0, Vector2.zero);        // 적 생성 ( 인자는 의미없음 )
    }


    public void Move(float targetSpeed_x)
    {
        _t.position += new Vector3( targetSpeed_x ,0,0);
    }

    public void SetCleared()
    {
        _cleared = true;
    }




    void OnDrawGizmos()
    {
        if(t_truckStopPoint==null)
        {
            return;
        }

        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(t_truckStopPoint.position,0.2f);
    }
}
