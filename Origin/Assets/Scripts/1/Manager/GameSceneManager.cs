using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 场景管理器 用于管理游戏场景的切换和场景资源管理
/// 保存数据的时候由GameDataMgr来保存数据
/// 修改数据由GameSceneManager来修改数据
/// 加载数据由GameDataMgr来加载数据 然后给GameSceneManager进入数据的更新
/// </summary>
public class GameSceneManager : SingletonAutoMono<GameSceneManager>
{
    public E_MusicTriggerType currentMusicTriggerType = E_MusicTriggerType.Chapter1_Level1; // 当前触发的音乐类型
    public GameObject player; // 玩家对象
    public E_SceneType currentSceneType; // 当前场景类型
    #region 第一章数据
    private int greenAlgaeCount = 0; // 收集到的绿藻数量
    public int GetGreenAlgaeCount { get { return greenAlgaeCount; } } // 获取收集到的绿藻数量
    private List<DialogData> sceneObjectCollisionDialogData = new List<DialogData>(); // 场景物体碰撞触发的对话数据列表
    public List<DialogData> GetSceneObjectCollisionDialogData { get { return sceneObjectCollisionDialogData; } } // 获取场景物体碰撞触发的对话数据列表
    private List<RockData> sceneRockDataList = new List<RockData>(); // 场景岩石数据列表
    public List<RockData> GetSceneRockDataList { get { return sceneRockDataList; } } // 获取场景岩石数据列表
    private List<BlackFogData> blackFogDataList = new List<BlackFogData>(); // 场景黑雾数据列表
    public List<BlackFogData> GetBlackFogDataList { get { return blackFogDataList; } } // 获取场景黑雾数据列表
    private E_PlayerState playerState = E_PlayerState.Normal; // 玩家状态
    public E_PlayerState GetPlayerState { get { return playerState; } } // 获取玩家状态
    private CheckPointData checkPointData = new CheckPointData(); // 回溯点数据
    public CheckPointData GetCheckPointData { get { return checkPointData; } } // 获取回溯点数据
    #endregion

    #region 第三章数据
    private StoneData stoneData = new StoneData(); // 石头和藤蔓数据
    public StoneData GetStoneData { get { return stoneData; } } // 获取石头和藤蔓数据
    #endregion

    #region 第四章数据
    private List<NPC> npcList = new List<NPC>(); // 场景NPC列表
    public List<NPC> GetNPCList { get { return npcList; } } // 获取场景NPC列表
    private List<MechanismData> mechanismList = new List<MechanismData>(); // 场景机关列表
    public List<MechanismData> GetMechanismDataList { get { return mechanismList; } } // 获取场景机关列表
    #endregion

    public void LoadScene(E_SceneType sceneType, UnityAction callback = null)
    {
        currentSceneType = sceneType;
        UIManager.Instance.ShowPanelAsync<LoadingPanel>((panel) =>
        {
            panel.InitProgress(sceneType, callback);
        });
    }

    public void UpdateGlobalInfo(E_MusicTriggerType triggerType, CheckPointData checkPointData)
    {
        this.UpdateCurrentMusicTriggerType(triggerType);
        this.UpdateCheckPointData(checkPointData);
    }
    public void UpdateCheckPointData(CheckPointData checkPointData)
    {
        this.checkPointData = checkPointData;
    }

    #region 第一章数据更新方法
    public void UpdateChapter1Info(int greenAlgaeCount, List<DialogData> dialogDataList, List<RockData> rockDataList, E_PlayerState playerState, List<BlackFogData> blackFogDataList)
    {
        this.UpdateGreenAlgaeCount(greenAlgaeCount);
        this.UpdateSceneObjectCollisionDialogData(dialogDataList);
        this.UpdateSceneRockDataList(rockDataList);
        this.UpdatePlayerState(playerState);
        this.UpdateBlackFogDataList(blackFogDataList);
    }

    public void UpdateBlackFogDataList(List<BlackFogData> blackFogDataList)
    {
        this.blackFogDataList = blackFogDataList;
    }

    public void UpdatePlayerState(E_PlayerState state)
    {
        playerState = state;
    }

    public void UpdateSceneObjectCollisionDialogData(List<DialogData> dialogDataList)
    {
        sceneObjectCollisionDialogData = dialogDataList;
    }

    public void UpdateCurrentMusicTriggerType(E_MusicTriggerType triggerType)
    {
        currentMusicTriggerType = triggerType;
    }

    public void UpdateGreenAlgaeCount(int num)
    {
        greenAlgaeCount = num;
    }

    public void UpdateSceneRockDataList(List<RockData> rockDataList)
    {
        sceneRockDataList = rockDataList;
    }
    #endregion

    #region 第三章数据更新方法
    public void UpdateChapter3Info(StoneData stoneData)
    {
        UpdateStoneData(stoneData);
    }

    public void UpdateStoneData(StoneData stoneData)
    {
        this.stoneData = stoneData;
    }
    #endregion

    public void UpdateMechanismDataList(List<MechanismData> mechanismDataList)
    {
        this.mechanismList = mechanismDataList;
    }
    public void OnGreenAlgaeCollected()
    {
        greenAlgaeCount++;
        if (greenAlgaeCount == 1)
            DialogManager.Instance.ShowType2Dialog(E_DialogDataType.SceneObjectCollisionText, 2);
        else if (greenAlgaeCount == 2)
            DialogManager.Instance.ShowType2Dialog(E_DialogDataType.SceneObjectCollisionText, 3);
        else if (greenAlgaeCount == 3)
            DialogManager.Instance.ShowType2Dialog(E_DialogDataType.SceneObjectCollisionText, 4);
        else if (greenAlgaeCount == 4)
            DialogManager.Instance.ShowType2Dialog(E_DialogDataType.SceneObjectCollisionText, 5);
        else if (greenAlgaeCount == 5)
            DialogManager.Instance.ShowType2Dialog(E_DialogDataType.SceneObjectCollisionText, 6);
    }

    public void PlayMusicByTriggerType(E_MusicTriggerType triggerType)
    {
        currentMusicTriggerType = triggerType;
        switch (triggerType)
        {
            case E_MusicTriggerType.Chapter1_Level1:
                MusicMgr.Instance.PlayBKMusic(PathUtil.MusicPathOfChapter1 + "1-1");
                break;
            case E_MusicTriggerType.Chapter1_Level2:
                MusicMgr.Instance.PlayBKMusic(PathUtil.MusicPathOfChapter1 + "1-2");
                break;
            case E_MusicTriggerType.Chapter1_Level3:
                MusicMgr.Instance.PlayBKMusic(PathUtil.MusicPathOfChapter1 + "1-3");
                break;
            case E_MusicTriggerType.Chapter2_Level1:
                MusicMgr.Instance.PlayBKMusic(PathUtil.MusicPathOfChapter2 + "2-1");
                break;
            case E_MusicTriggerType.Chapter2_Level2:
                MusicMgr.Instance.PlayBKMusic(PathUtil.MusicPathOfChapter2 + "2-2");
                break;
            case E_MusicTriggerType.Chapter2_Level3:
                MusicMgr.Instance.PlayBKMusic(PathUtil.MusicPathOfChapter2 + "2-3");
                break;
            case E_MusicTriggerType.Chapter3_Level1:
                MusicMgr.Instance.PlayBKMusic(PathUtil.MusicPathOfChapter3 + "3-1");
                break;
            case E_MusicTriggerType.Chapter3_Level2:
                MusicMgr.Instance.PlayBKMusic(PathUtil.MusicPathOfChapter3 + "3-2");
                break;
            case E_MusicTriggerType.Chapter3_Level3:
                MusicMgr.Instance.PlayBKMusic(PathUtil.MusicPathOfChapter3 + "3-3");
                break;
        }
    }

    public void PlaySoundByTriggerType(E_SoundTriggerType triggerType, bool isLoop = false, bool isAsync = false, UnityAction<AudioSource> callback = null)
    {
        switch (triggerType)
        {
            case E_SoundTriggerType.MagmaSound:
                MusicMgr.Instance.PlaySound(PathUtil.SoundPathOfChapter1 + "MagmaLoopingSound", isLoop, isAsync, callback);
                break;
            case E_SoundTriggerType.GreenAlgaeAbsorbSound:
                MusicMgr.Instance.PlaySound(PathUtil.SoundPathOfChapter1 + "GreenAlgaeAbsorbSound", isLoop, isAsync, callback);
                break;
            case E_SoundTriggerType.JumpSound:
                MusicMgr.Instance.PlaySound(PathUtil.SoundPathOfChapter1 + "JumpSound", isLoop, isAsync, callback);
                break;
            case E_SoundTriggerType.FootstepSound:
                MusicMgr.Instance.PlaySound(PathUtil.SoundPathOfChapter1 + "FootstepSound", isLoop, isAsync, callback);
                break;
            case E_SoundTriggerType.LandSound:
                MusicMgr.Instance.PlaySound(PathUtil.SoundPathOfChapter1 + "LandingSound", isLoop, isAsync, callback);
                break;
            case E_SoundTriggerType.RockBreakSound:
                MusicMgr.Instance.PlaySound(PathUtil.SoundPathOfChapter1 + "RockShatterSound", isLoop, isAsync, callback);
                break;
        }
    }

    public void GeneratePlayer(E_SceneType type, Vector3 position)
    {
        switch (type)
        {
            case E_SceneType.GameScene01:
                GameObject playerObj = Instantiate(ResMgr.Instance.Load<GameObject>(PathUtil.PlayerPath + "Player1"));
                player = playerObj;
                playerObj.transform.position = GameDataMgr.Instance.gameData1.playerData.playerPos != null
                ? new Vector3(GameDataMgr.Instance.gameData1.playerData.playerPos._x,
                GameDataMgr.Instance.gameData1.playerData.playerPos._y,
                GameDataMgr.Instance.gameData1.playerData.playerPos._z)
                : position;
                ChangePlayerScale(GameDataMgr.Instance.gameData1.playerData.playerState);
                break;
            case E_SceneType.GameScene02:
                break;
            case E_SceneType.GameScene03:
                GameObject playerObj3 = Instantiate(ResMgr.Instance.Load<GameObject>(PathUtil.PlayerPath + "Player3"));
                player = playerObj3;
                playerObj3.transform.position = GameDataMgr.Instance.gameData3.playerData.playerPos != null
                ? new Vector3(GameDataMgr.Instance.gameData3.playerData.playerPos._x,
                GameDataMgr.Instance.gameData3.playerData.playerPos._y,
                GameDataMgr.Instance.gameData3.playerData.playerPos._z)
                : position;
                player.transform.localScale = new Vector3(-0.3f, 0.3f, 1);
                break;
        }
    }

    public void SetPlayer(GameObject playerObj)
    {
        player = playerObj;
    }

    public void ChangePlayerScale(E_PlayerState state)
    {
        switch (state)
        {
            case E_PlayerState.Normal:
                player.transform.localScale = new Vector3(0.2f, 0.2f, 1);
                player.GetComponent<CharacterController2D>().jumpForce = 400;
                break;
            case E_PlayerState.Large:
                player.transform.localScale = new Vector3(0.35f, 0.35f, 1);
                player.GetComponent<CharacterController2D>().jumpForce = 500;
                break;
        }
    }

    public void UpdateBlackFogDataList(int ID)
    {
        blackFogDataList[ID].isDestroyed = true;
    }

    public void UpdateNPCState(int npcID)
    {
        NPC npc = npcList.Find(n => n.npcID == npcID);
        if (npc.talkNum > 0)
            npc.talkNum--; // 交互次数减1

        if (npc != null && npc.talkNum <= 0)
            npc.canTalk = false; // 如果交互次数为0，则设置为不能交互
    }
}
