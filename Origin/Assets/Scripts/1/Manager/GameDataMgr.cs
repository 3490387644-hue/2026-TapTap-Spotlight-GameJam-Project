using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum E_SceneType
{
    LoadingScene,
    BeginScene,
    GameScene01,
    GameScene02
}

public class GameDataMgr : BaseManager<GameDataMgr>

{
    public GlobalGameData globalGameData = new GlobalGameData();
    public GameData1 gameData1 = new GameData1();
    public List<DialogData> typewritterDialogData = new List<DialogData>();
    public List<DialogData> sceneObjectCollisionDialogData = new List<DialogData>();
    public List<DialogData> airwallTriggerDialogData = new List<DialogData>();
    public int dialogIndex; // 这个存放需要利用索引来获取对话数据的索引值
    public List<bool> dialogTriggerStates = new List<bool>(); // 存放对话触发点的状态，true表示已触发，false表示未触发
    public int greenAlgaeCollectedCount; // 已经收集到的绿藻数量
    public PlayerData1 playerData = new PlayerData1(); // 玩家数据

    /// <summary>
    /// 游戏一进入就需要初始化游戏存档数据
    /// </summary>
    public void Init()
    {
        EnsureDialogDataInPersistent(); // 确保对话Json已从默认目录复制到读写目录
        LoadGameData(); // 加载游戏存档数据
    }

    /// <summary>
    /// 开启游戏时检查读写目录中是否存在三个对话Json，如果没有则从StreamingAssets默认目录复制过去
    /// </summary>
    private void EnsureDialogDataInPersistent()
    {
        string[] dialogFiles = new string[]
        {
            "TypewritterText.json",
            "SceneObjectCollisionText.json",
            "AirwallTriggerText.json"
        };
        foreach (string fileName in dialogFiles)
        {
            string targetPath = Application.persistentDataPath + "/" + fileName;
            if (!File.Exists(targetPath))
            {
                string sourcePath = Application.streamingAssetsPath + "/" + fileName;
                if (File.Exists(sourcePath))
                {
                    File.Copy(sourcePath, targetPath);
                }
            }
        }
    }

    public void SaveGameData()
    {
        SaveGlobalGameData();
        SaveGameData1();
    }

    public void SaveGlobalGameData()
    {
        GlobalGameData globalGameData = new GlobalGameData
        {
            sceneType = GameSceneManager.Instance.currentSceneType,
            musicData = new MusicData
            {
                musicVolume = MusicMgr.Instance.GetBkMusicValue,
                soundVolume = MusicMgr.Instance.GetSoundValue
            },
            currentMusicTriggerType = GameSceneManager.Instance.currentMusicTriggerType
        };
        JsonMgr.Instance.SaveData(globalGameData, "GlobalGameData", JsonType.JsonUtlity);
    }

    /// <summary>
    /// 专门用来对场景1进行游戏存档的方法
    /// </summary>
    public void SaveGameData1()
    {
        Vector3 curPos = GameSceneManager.Instance.player.transform.localPosition;
        GameData1 data = new GameData1
        {
            hasSaveData = true,
            // 当进入游戏存档的时候 再从游戏场景管理器中获取绿藻收集数量 并保存到存档数据中
            greenAlgaeCollectedCount = GameSceneManager.Instance.GetGreenAlgaeCount,
            playerData = new PlayerData1
            {
                playerPos = new PositionData
                {
                    _x = curPos.x,
                    _y = curPos.y,
                    _z = curPos.z
                },
                playerState = GameSceneManager.Instance.GetPlayerState
            },
            rockDataList = gameData1.rockDataList, // 保存当前游戏中岩石的状态列表,
            checkPointData = GameSceneManager.Instance.GetCheckPointData, // 保存当前游戏中复活点的位置数据
            blackFogDataList = GameSceneManager.Instance.GetBlackFogDataList // 保存当前游戏中黑雾的状态列表
        };
        JsonMgr.Instance.SaveData(data, "GameData1", JsonType.JsonUtlity);
        JsonMgr.Instance.SaveData(typewritterDialogData, "TypewritterText", JsonType.LitJson);
        JsonMgr.Instance.SaveData(sceneObjectCollisionDialogData, "SceneObjectCollisionText", JsonType.LitJson);
        JsonMgr.Instance.SaveData(airwallTriggerDialogData, "AirwallTriggerText", JsonType.LitJson);
    }

    public void UpdateAirwallTriggerState(int id)
    {
        airwallTriggerDialogData[id].HasTriggered = true;
    }

    private void LoadGameData()
    {
        LoadGlobalGameData();
        LoadGameData1();
    }

    private void LoadGlobalGameData()
    {
        globalGameData = JsonMgr.Instance.LoadData<GlobalGameData>("GlobalGameData");
    }

    // 加载第一章的数据
    private void LoadGameData1()
    {
        gameData1 = JsonMgr.Instance.LoadData<GameData1>("GameData1");
        typewritterDialogData = JsonMgr.Instance.LoadData<List<DialogData>>("TypewritterText");
        sceneObjectCollisionDialogData = JsonMgr.Instance.LoadData<List<DialogData>>("SceneObjectCollisionText");
        airwallTriggerDialogData = JsonMgr.Instance.LoadData<List<DialogData>>("AirwallTriggerText");
        GameSceneManager.Instance.UpdateAllInfo1(globalGameData.currentMusicTriggerType,
        gameData1.greenAlgaeCollectedCount,
        sceneObjectCollisionDialogData,
        gameData1.rockDataList,
        gameData1.playerData.playerState,
        gameData1.checkPointData,
        gameData1.blackFogDataList);
        DialogManager.Instance.Init();
        MusicMgr.Instance.UpdateData(); // 更新音乐和音效的值
    }

    public void ClearAllSaveData()
    {
        JsonMgr.Instance.ClearAllSaveData(); // 删除读写目录中的所有存档文件
        EnsureDialogDataInPersistent(); // 重新从默认目录复制对话Json
        LoadGlobalGameData(); // 重新加载全局数据
        LoadGameData1(); // 重新加载默认数据
    }
}
