using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏存档数据类
/// 以后游戏中所有需要存档的数据都放在这个类里
/// </summary>
public class GameData
{
    public bool hasSaveData = false; // 是否有存档数据
    public PositionData playerPos; // 玩家位置数据
    public float musicVolume = 0.5f; // 音乐音量
    public float soundVolume = 0.5f; // 音效音量
    public E_SceneType sceneType = E_SceneType.BeginScene; // 场景类型
}

/// <summary>
/// 玩家位置数据类
/// </summary>
public class PositionData
{
    public float _x;
    public float _y;
    public float _z;
}