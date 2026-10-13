using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum E_SceneType
{
    LoadingScene,
    BeginScene,
    GameScene1,
    GameScene2
}

public class GameDataMgr : BaseManager<GameDataMgr>
{
    public GameData gameData = new GameData();
    public List<DialogData> typewritterDialogData = new List<DialogData>();
    public List<DialogData> sceneObjectCollisionDialogData = new List<DialogData>();
    public List<DialogData> airwallTriggerDialogData = new List<DialogData>();
    public int dialogIndex; // 这个存放需要利用索引来获取对话数据的索引值
    public List<bool> dialogTriggerStates = new List<bool>(); // 存放对话触发点的状态，true表示已触发，false表示未触发
    public int greenAlgaeCollectedCount; // 已经收集到的绿藻数量
    public PlayerData playerData = new PlayerData(); // 玩家数据

    /// <summary>
    /// 游戏一进入就需要初始化游戏存档数据
    /// </summary>
    public void Init()
    {
        EnsureDialogDataInPersistent(); // 确保对话Json已从默认目录复制到读写目录
        LoadGameData();
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
        foreach(string fileName in dialogFiles)
        {
            string targetPath = Application.persistentDataPath + "/" + fileName;
            if(!File.Exists(targetPath))
            {
                string sourcePath = Application.streamingAssetsPath + "/" + fileName;
                if(File.Exists(sourcePath))
                {
                    File.Copy(sourcePath, targetPath);
                }
            }
        }
    }

    /// <summary>
    /// 专门用来进行游戏存档的方法
    /// </summary>
    public void SaveGameData()
    {
        Vector3 curPos = GameSceneManager.Instance.player.transform.localPosition;
        GameData data = new GameData
        {
            hasSaveData = true,
            sceneType = GameSceneManager.Instance.currentSceneType,
            // 当进入游戏存档的时候 再从游戏场景管理器中获取绿藻收集数量 并保存到存档数据中
            greenAlgaeCollectedCount = GameSceneManager.Instance.GetGreenAlgaeCount,
            currentMusicTriggerType = GameSceneManager.Instance.currentMusicTriggerType,
            musicData = new MusicData
            {
                musicVolume = MusicMgr.Instance.GetBkMusicValue,
                soundVolume = MusicMgr.Instance.GetSoundValue
            },
            playerData = new PlayerData
            {
                playerPos = new PositionData
                {
                    _x = curPos.x,
                    _y = curPos.y,
                    _z = curPos.z
                },
                playerState = GameSceneManager.Instance.GetPlayerState
            },
            rockDataList = gameData.rockDataList, // 保存当前游戏中岩石的状态列表,
            checkPointData = GameSceneManager.Instance.GetCheckPointData // 保存当前游戏中复活点的位置数据
        };
        JsonMgr.Instance.SaveData(data, "GameData", JsonType.JsonUtlity);
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
        gameData = JsonMgr.Instance.LoadData<GameData>("GameData");
        typewritterDialogData = JsonMgr.Instance.LoadData<List<DialogData>>("TypewritterText");
        sceneObjectCollisionDialogData = JsonMgr.Instance.LoadData<List<DialogData>>("SceneObjectCollisionText");
        airwallTriggerDialogData = JsonMgr.Instance.LoadData<List<DialogData>>("AirwallTriggerText");
        GameSceneManager.Instance.UpdateAllInfo(gameData.currentMusicTriggerType, 
        gameData.greenAlgaeCollectedCount, 
        sceneObjectCollisionDialogData,
        gameData.rockDataList,
        gameData.playerData.playerState,
        gameData.checkPointData);
        DialogManager.Instance.Init();
        MusicMgr.Instance.UpdateData(); // 更新音乐和音效的值
    }

    public void ClearAllSaveData()
    {
        JsonMgr.Instance.ClearAllSaveData(); // 删除读写目录中的所有存档文件
        EnsureDialogDataInPersistent(); // 重新从默认目录复制对话Json
        LoadGameData(); // 重新加载默认数据
    }
}
