using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_MusicTriggerType
{
    BKMusic, // 背景音乐
    Chapter1_Level1, // 第一章第一关
    Chapter1_Level2, // 第一章第二关
    Chapter1_Level3, // 第一章第三关
}

public enum E_SoundTriggerType
{
    MagmaSound, // 岩浆
    GreenAlgaeAbsorbSound, // 绿藻
    JumpSound, // 跳跃
    FootstepSound, // 脚步声
    LandSound, // 落地
    RockBreakSound
}

public enum E_PlayerState
{
    Normal, // 正常状态
    Large, // 变大状态
}

/// <summary>
/// 游戏存档的数据类
/// 以后游戏过程中所有需要存档的数据都放在这里面
/// </summary>
[System.Serializable]
public class GameData
{
    public bool hasSaveData = false; // 是否有存档数据
    public E_SceneType sceneType = E_SceneType.BeginScene; // 场景类型
    public int greenAlgaeCollectedCount; // 已经收集的绿藻数量
    public E_MusicTriggerType currentMusicTriggerType; // 当前播放的背景音乐类型
    public MusicData musicData = new MusicData(); // 音乐和音效数据
    public PlayerData playerData = new PlayerData(); // 玩家数据
    public List<RockData> rockDataList = new List<RockData>(); // 特殊岩石数据列表
    public CheckPointData checkPointData = new CheckPointData(); // 回溯点数据
}

/// <summary>
/// 游戏音乐和音效数据类
/// </summary>
[System.Serializable]
public class MusicData
{
    public float musicVolume = 0.5f; // 音乐音量
    public float soundVolume = 0.5f; // 音效音量
}

/// <summary>
/// 玩家数据类
/// </summary>
[System.Serializable]
public class PlayerData
{
    public PositionData playerPos; // 玩家位置数据
    public E_PlayerState playerState; // 玩家状态数据
}

/// <summary>
/// 特殊岩石数据类
/// </summary>
[System.Serializable]
public class RockData
{
    public int rockID; // 岩石的ID
    public bool isBreak; // 岩石是否已经被打碎
}

/// <summary>
/// 回溯点数据类
/// </summary>
[System.Serializable]
public class CheckPointData
{
    public PositionData checkPointPos; // 检查点位置数据
}

/// <summary>
/// 玩家位置数据结构
/// </summary>
[System.Serializable]
public class PositionData
{
    public float _x;
    public float _y;
    public float _z;
    public PositionData(){}
    public PositionData(float x, float y, float z)
    {
        _x = x;
        _y = y;
        _z = z;
    }

    internal Vector3 ToVector3()
    {
        return new Vector3(_x, _y, _z);
    }

}
