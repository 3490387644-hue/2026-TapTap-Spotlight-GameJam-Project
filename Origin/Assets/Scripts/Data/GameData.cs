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
    Chapter2_Level1, // 第二章第一关
    Chapter2_Level2, // 第二章第二关
    Chapter2_Level3, // 第二章第三关
    Chapter3_Level1, // 第三章第一关
    Chapter3_Level2, // 第三章第二关
    Chapter3_Level3, // 第三章第三关
    Chapter4_Level1, // 第四章整体音乐
}

public enum E_SoundTriggerType
{
    Chapter1MagmaSound, // 岩浆
    ChapterGreenAlgaeAbsorbSound, // 绿藻
    ChapterJumpSound, // 跳跃
    Chapter1FootstepSound, // 脚步声
    Chapter1LandSound, // 落地
    Chapter1RockBreakSound,
    
}

public enum E_PlayerState
{
    Normal, // 正常状态
    Large, // 变大状态
}

public class GlobalGameData
{
    public E_SceneType sceneType = E_SceneType.BeginScene; // 场景类型
    public MusicData musicData = new MusicData(); // 音乐和音效数据
    public E_MusicTriggerType currentMusicTriggerType; // 当前播放的背景音乐类型
    public CheckPointData checkPointData = new CheckPointData(); // 回溯点数据
}

/// <summary>
/// 游戏存档的数据类
/// 以后游戏过程中所有需要存档的数据都放在这里面
/// </summary>
[System.Serializable]
public class GameData1
{
    public bool hasSaveData = false; // 是否有存档数据
    public int greenAlgaeCollectedCount; // 已经收集的绿藻数量
    public PlayerData1 playerData = new PlayerData1(); // 玩家数据
    public List<RockData> rockDataList = new List<RockData>(); // 特殊岩石数据列表
    public List<BlackFogData> blackFogDataList = new List<BlackFogData>(); // 黑雾数据列表
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
public class PlayerData1
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
    public PositionData checkPointPos = new PositionData(); // 检查点位置数据
}

/// <summary>
/// 黑雾数据类
/// </summary>
[System.Serializable]
public class BlackFogData
{
    public int blackFogID; // 黑雾的ID
    public bool isDestroyed; // 黑雾是否已经被销毁
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

#region 第二关数据
[System.Serializable]
public class GameData2
{
    public bool hasSaveData = false; // 是否有存档数据
    public PlayerData2 playerData = new PlayerData2(); // 玩家数据
}

[System.Serializable]
public class PlayerData2
{
    public PositionData playerPos; // 玩家位置数据
}
#endregion

#region 第三关数据
/// <summary>
/// 场景3的游戏存档数据类
/// </summary>
[System.Serializable]
public class GameData3
{
    public bool hasSaveData = false; // 是否有存档数据
    public PlayerData3 playerData = new PlayerData3(); // 玩家数据
    public StoneData stoneData = new StoneData(); // 石头和藤蔓数据
}

/// <summary>
/// 场景3的玩家数据类
/// </summary>
[System.Serializable]
public class PlayerData3
{
    public PositionData playerPos; // 玩家位置数据
}

/// <summary>
/// 岩石数据类
/// </summary>
[System.Serializable]
public class StoneData
{
    public bool isStoneFallen; // 石头是否已经掉落
}
#endregion

/// <summary>
/// 第四章场景机关数据类
/// </summary>
[System.Serializable]
public class MechanismData
{
    public int mechanismID; // 机关的ID
    public bool isOpen; // 机关是否已经打开
}
