using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_SceneType
{
    BeginScene,
    GameScene
}

public class GameDataMgr : BaseManager<GameDataMgr>
{
    public GameData gameData = new GameData();
    public List<DialogData> dialogData = new List<DialogData>();

    /// <summary>
    /// 游戏一进入就需要初始化游戏存档数据
    /// </summary>
    public void Init()
    {
        LoadGameData();
        if(gameData != null)
            gameData.hasSaveData = true; // 有存档数据
    }
    
    /// <summary>
    /// 专门用来进行游戏存档的方法
    /// </summary>
    public void SaveGameData(Vector3 playerPos, float musicVolume, float soundVolume, E_SceneType type)
    {
        GameData data = new GameData
        {
            hasSaveData = true,
            playerPos = new PositionData
            {
                _x = playerPos.x,
                _y = playerPos.y,
                _z = playerPos.z
            },
            musicVolume = musicVolume,
            soundVolume = soundVolume,
            sceneType = type
        };
        JsonMgr.Instance.SaveData(data, "GameData", JsonType.JsonUtlity);
    }

    private void LoadGameData()
    {
        gameData = JsonMgr.Instance.LoadData<GameData>("GameData");
        dialogData = JsonMgr.Instance.LoadData<List<DialogData>>("DialogData");
    }
}
