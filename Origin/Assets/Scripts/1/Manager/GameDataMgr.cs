using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum E_SceneType
{
    LoadingScene,
    BeginScene,
    GameScene01,
    GameScene02,
    GameScene03,
    GameScene04,
    None
}

public enum E_DialogDataType
{
    SceneObjectCollisionText, // 场景物体碰撞文本
    AirwallTriggerText // 空气墙触发文本
}

public class GameDataMgr : BaseManager<GameDataMgr>

{
    public GlobalGameData globalGameData = new GlobalGameData();
    public GameData1 gameData1 = new GameData1();
    public GameData3 gameData3 = new GameData3();
    public List<DialogData> typewritterDialogData = new List<DialogData>();
    public List<DialogData> sceneObjectCollisionDialogData = new List<DialogData>();
    public List<DialogData> airwallTriggerDialogData = new List<DialogData>();
    public int dialogIndex; // 这个存放需要利用索引来获取对话数据的索引值
    public List<bool> dialogTriggerStates = new List<bool>(); // 存放对话触发点的状态，true表示已触发，false表示未触发
    public int greenAlgaeCollectedCount; // 已经收集到的绿藻数量
    public PlayerData1 playerData = new PlayerData1(); // 玩家数据

    public void Init()
    {
        EnsureDialogDataInPersistent(); // 确保对话Json已从默认目录复制到读写目录
        LoadGameData(); // 加载游戏存档数据
    }

    // 游戏数据相关说明
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
        SaveGameData3();
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
            currentMusicTriggerType = GameSceneManager.Instance.currentMusicTriggerType,
            checkPointData = GameSceneManager.Instance.GetCheckPointData, // 保存当前游戏中复活点的位置数据
        };
        JsonMgr.Instance.SaveData(globalGameData, "GlobalGameData", JsonType.JsonUtlity);
    }

    public void SaveGameData1()
    {
        Vector3 curPos = GameSceneManager.Instance.player.transform.localPosition;
        GameData1 data = new GameData1
        {
            hasSaveData = true,
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
            blackFogDataList = GameSceneManager.Instance.GetBlackFogDataList // 保存当前游戏中黑雾的状态列表
        };
        JsonMgr.Instance.SaveData(data, "GameData1", JsonType.JsonUtlity);
        JsonMgr.Instance.SaveData(typewritterDialogData, "TypewritterText", JsonType.LitJson);
        JsonMgr.Instance.SaveData(sceneObjectCollisionDialogData, "SceneObjectCollisionText", JsonType.LitJson);
        JsonMgr.Instance.SaveData(airwallTriggerDialogData, "AirwallTriggerText", JsonType.LitJson);
    }

    public void SaveGameData3()
    {
        Vector3 curPos = GameSceneManager.Instance.player.transform.localPosition;
        GameData3 data = new GameData3
        {
            hasSaveData = true,
            playerData = new PlayerData3
            {
                playerPos = new PositionData
                {
                    _x = curPos.x,
                    _y = curPos.y,
                    _z = curPos.z
                }
            },
            stoneData = GameSceneManager.Instance.GetStoneData // 保存当前游戏中石头和藤蔓的状态
        };
        JsonMgr.Instance.SaveData(data, "GameData3", JsonType.JsonUtlity);
    }

    public void UpdateAirwallTriggerState(int id)
    {
        airwallTriggerDialogData[id].HasTriggered = true;
    }

    private void LoadGameData()
    {
        LoadGlobalGameData();
        LoadGameData1();
        LoadGameData3();
    }

    private void LoadGlobalGameData()
    {
        globalGameData = JsonMgr.Instance.LoadData<GlobalGameData>("GlobalGameData");
        if(globalGameData == null)
            globalGameData.sceneType = E_SceneType.None;
        GameSceneManager.Instance.UpdateGlobalInfo(
            globalGameData.currentMusicTriggerType,
            globalGameData.checkPointData);
        typewritterDialogData = JsonMgr.Instance.LoadData<List<DialogData>>("TypewritterText");
        sceneObjectCollisionDialogData = JsonMgr.Instance.LoadData<List<DialogData>>("SceneObjectCollisionText");
        airwallTriggerDialogData = JsonMgr.Instance.LoadData<List<DialogData>>("AirwallTriggerText");
    }


    private void LoadGameData1()
    {
        gameData1 = JsonMgr.Instance.LoadData<GameData1>("GameData1");
        GameSceneManager.Instance.UpdateChapter1Info(
        gameData1.greenAlgaeCollectedCount,
        sceneObjectCollisionDialogData,
        gameData1.rockDataList,
        gameData1.playerData.playerState,
        gameData1.blackFogDataList);
        DialogManager.Instance.Init();
        MusicMgr.Instance.UpdateData(); // 更新音乐和音效的值
    }

    public void LoadGameData3()
    {
        gameData3 = JsonMgr.Instance.LoadData<GameData3>("GameData3");
        // GameSceneManager.Instance.UpdateChapter3Info();
        GameSceneManager.Instance.UpdateChapter3Info(
            gameData3.stoneData);

        DialogManager.Instance.Init();
        MusicMgr.Instance.UpdateData(); // 更新音乐和音效的值
    }

    public void ClearAllSaveData()
    {
        JsonMgr.Instance.ClearAllSaveData(); // 删除读写目录中的所有存档文件
        EnsureDialogDataInPersistent(); // 确保对话Json已从默认目录复制到读写目录
        LoadGlobalGameData(); // 加载全局游戏数据
        LoadGameData1(); // 加载第一章游戏数据
        LoadGameData3(); // 加载第三章游戏数据
    }
}
