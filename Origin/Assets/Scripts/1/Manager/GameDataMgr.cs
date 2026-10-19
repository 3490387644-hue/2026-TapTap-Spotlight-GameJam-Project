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
    SceneObjectCollisionText, // 场景物体碰撞触发的对话文本
    AirwallTriggerText        // 空气墙触发触发的对话文本
}

public class GameDataMgr : BaseManager<GameDataMgr>
{
    public GlobalGameData globalGameData = new GlobalGameData();
    public GameData1 gameData1 = new GameData1();
    public GameData2 gameData2 = new GameData2();
    public GameData3 gameData3 = new GameData3();
    public List<DialogData> typewritterDialogData = new List<DialogData>();
    public List<DialogData> sceneObjectCollisionDialogData = new List<DialogData>();
    public List<DialogData> airwallTriggerDialogData = new List<DialogData>();
    public int dialogIndex; // 当前对话需要读取或写入的索引值
    public List<bool> dialogTriggerStates = new List<bool>(); // 各对话的触发状态，true表示已触发，false表示未触发
    public int greenAlgaeCollectedCount; // 绿藻收集数量
    public PlayerData1 playerData = new PlayerData1(); // 玩家数据

    public void Init()
    {
        EnsureDialogDataInPersistent(); // 确保对话Json文件从默认目录复制到持久化可写目录
        LoadGameData(); // 加载游戏存档数据
    }

    // 游戏启动时确保对话数据在持久化目录中
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
        switch (GameSceneManager.Instance.currentSceneType)
        {
            case E_SceneType.GameScene01:
                SaveGameData1();
                break;
            case E_SceneType.GameScene02:
                SaveGameData2();
                break;
            case E_SceneType.GameScene03:
                SaveGameData3();
                break;
        }
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
            checkPointData = GameSceneManager.Instance.GetCheckPointData, // 保存当前场景的检查点位置数据
            timelineData = GameSceneManager.Instance.GetTimelineData // 保存当前场景的时间线触发数据
        };
        JsonMgr.Instance.SaveData(globalGameData, "GlobalGameData", JsonType.JsonUtlity);
        JsonMgr.Instance.SaveData(typewritterDialogData, "TypewritterText", JsonType.LitJson);
        JsonMgr.Instance.SaveData(sceneObjectCollisionDialogData, "SceneObjectCollisionText", JsonType.LitJson);
        JsonMgr.Instance.SaveData(airwallTriggerDialogData, "AirwallTriggerText", JsonType.LitJson);
    }

    public void SaveGameData1()
    {
        Vector3 curPos = GameSceneManager.Instance.player.transform.position;
        GameData1 data = new GameData1
        {
            hasSaveData = true,
            greenAlgaeCollectedCount = GameSceneManager.Instance.GetGreenAlgaeCount,
            greenAlgaeDataList = GameSceneManager.Instance.GetGreenAlgaeDataList,
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
            rockDataList = gameData1.rockDataList, // 保存当前场景中岩石的状态数据
            blackFogDataList = GameSceneManager.Instance.GetBlackFogDataList // 保存当前场景中黑雾的状态数据
        };
        JsonMgr.Instance.SaveData(data, "GameData1", JsonType.JsonUtlity);
    }

    public void SaveGameData2()
    {
        Vector3 curPos = GameSceneManager.Instance.player.transform.position;
        GameData2 data = new GameData2
        {
            hasSaveData = true,
            playerData = new PlayerData2
            {
                playerPos = new PositionData
                {
                    _x = curPos.x,
                    _y = curPos.y,
                    _z = curPos.z
                }
            },
            // 此处保存第二章相关数据，如机关状态、物品状态等
            cameraShapeIndex = GameSceneManager.Instance.GetCameraShapeIndex // 保存当前相机边界范围索引
        };
        JsonMgr.Instance.SaveData(data, "GameData2", JsonType.JsonUtlity);
    }

    public void SaveGameData3()
    {
        Vector3 curPos = GameSceneManager.Instance.player.transform.position;
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
                },
                isFlying = GameSceneManager.Instance.GetIsFlying // 保存玩家是否在飞行状态
            },
            stoneData = GameSceneManager.Instance.GetStoneData // 保存当前场景中石头的状态数据
        };
        JsonMgr.Instance.SaveData(data, "GameData3", JsonType.JsonUtlity);
    }

    public void UpdateAirwallTriggerState(int id)
    {
        var data = airwallTriggerDialogData.Find(d => d.ID == id);
        if (data != null)
            data.HasTriggered = true;
    }

    private void LoadGameData()
    {
        LoadGlobalGameData();
        LoadGameData1();
        LoadGameData2();
        LoadGameData3();
    }

    private void LoadGlobalGameData()
    {
        globalGameData = JsonMgr.Instance.LoadData<GlobalGameData>("GlobalGameData");
        if (globalGameData == null)
            globalGameData = new GlobalGameData();
        GameSceneManager.Instance.UpdateGlobalInfo(
            globalGameData.currentMusicTriggerType,
            globalGameData.checkPointData,
            globalGameData.timelineData);
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
        GameSceneManager.Instance.UpdateGreenAlgaeDataList(gameData1.greenAlgaeDataList);
        DialogManager.Instance.Init();
        MusicMgr.Instance.UpdateData(); // 更新背景音乐和音效的音量值
    }

    public void LoadGameData2()
    {
        gameData2 = JsonMgr.Instance.LoadData<GameData2>("GameData2");
        GameSceneManager.Instance.UpdateChapter2Info(
            gameData2.cameraShapeIndex);
    }

    public void LoadGameData3()
    {
        gameData3 = JsonMgr.Instance.LoadData<GameData3>("GameData3");
        // GameSceneManager.Instance.UpdateChapter3Info();
        GameSceneManager.Instance.UpdateChapter3Info(
            gameData3.stoneData,
            gameData3.playerData.isFlying);

        DialogManager.Instance.Init();
        MusicMgr.Instance.UpdateData(); // 更新背景音乐和音效的音量值
    }

    public void ClearAllSaveData()
    {
        JsonMgr.Instance.ClearAllSaveData(); // 删除持久化目录中的所有存档文件
        EnsureDialogDataInPersistent(); // 确保对话Json文件从默认目录复制到持久化可写目录
        LoadGlobalGameData(); // 重新加载全局游戏数据
        LoadGameData1(); // 重新加载第一章游戏数据
        LoadGameData2(); // 重新加载第二章游戏数据
        LoadGameData3(); // 重新加载第三章游戏数据
    }
}