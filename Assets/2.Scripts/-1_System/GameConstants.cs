using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;

public static class GameConstants
{
    // Tag
    public static readonly string enemyTag = "Enemy";
    
    // layer
    public static readonly int truckLayer = 10;
    public static readonly int BoxLayer = 11;
    public static readonly int HeroLayer = 12;
    public static readonly int monsterLayer_0 = 13;
    public static readonly int monsterLayer_1 = 14;
    public static readonly int monsterLayer_2 = 15;
    public static readonly int spawnBoxLayer = 16;
    public static readonly int totalEnemyLayer= 1<<monsterLayer_0| 1<<monsterLayer_1|1<<monsterLayer_2 | 1<<spawnBoxLayer;

    // Monster
    public static readonly Dictionary<int, float> lineHeights = new(){
                                                                {0, -3.62f},
                                                                {1, -3.62f+0.25f},
                                                                {2, -3.62f+0.5f},
                                                            };
    public static readonly Dictionary<int, int> lineLayerMasks = new(){
                                                                {0, monsterLayer_0},
                                                                {1, monsterLayer_1},
                                                                {2, monsterLayer_2},
                                                            };

    public static float GetLineHeight(int lineNum)
    {
        return lineHeights[lineNum];
    }

    public static int GetLineLayerMask(int lineNum)
    {
        return lineLayerMasks[lineNum];
    }
}

