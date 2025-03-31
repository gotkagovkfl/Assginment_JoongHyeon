using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyInitData", menuName = "SO/EnemyInitData", order = int.MaxValue)]
public class EnemyInitDataSO : ScriptableObject
{
    public float maxHp;

    
    public float attackPower;
    public float movementSpeed;
    public float jumpPower;


}
