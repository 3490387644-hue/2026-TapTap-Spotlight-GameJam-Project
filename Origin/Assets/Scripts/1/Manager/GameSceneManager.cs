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
    private int greenAlgaeCount = 0; // 收集到的绿藻数量
    public int GetGreenAlgaeCount { get { return greenAlgaeCount; } } // 获取收集到的绿藻数量
    private List<DialogData> sceneObjectCollisionDialogData = new List<DialogData>(); // 场景物体碰撞触发的对话数据列表
    private List<RockData> sceneRockDataList = new List<RockData>(); // 场景岩石数据列表
    public List<RockData> GetSceneRockDataList { get { return sceneRockDataList; } } // 获取场景岩石数据列表
    private E_PlayerState playerState = E_PlayerState.Normal; // 玩家状态
    public E_PlayerState GetPlayerState { get { return playerState; } } // 获取玩家状态
    private CheckPointData checkPointData = new CheckPointData(); // 回溯点数据
    public CheckPointData GetCheckPointData { get { return checkPointData; } } // 获取回溯点数据

    public void LoadScene(E_SceneType sceneType, UnityAction callback = null)
    {
        currentSceneType = sceneType;
        UIManager.Instance.ShowPanelAsync<LoadingPanel>((panel)=>
        {
            panel.InitProgress(sceneType, callback);
        });
    }
    
    public void UpdateAllInfo(E_MusicTriggerType triggerType, int greenAlgaeCount, List<DialogData> dialogDataList, List<RockData> rockDataList, E_PlayerState playerState, CheckPointData checkPointData)
    {
        this.UpdateCurrentMusicTriggerType(triggerType);
        this.UpdateGreenAlgaeCount(greenAlgaeCount);
        this.UpdateSceneObjectCollisionDialogData(dialogDataList);
        this.UpdateSceneRockDataList(rockDataList);
        this.UpdatePlayerState(playerState);
        this.UpdateCheckPointData(checkPointData);
    }

    public void UpdateCheckPointData(CheckPointData checkPointData)
    {
        this.checkPointData = checkPointData;
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

    public void OnGreenAlgaeCollected()
    {
        greenAlgaeCount++;
        if(greenAlgaeCount == 1)
            DialogManager.Instance.ShowDialogPanel(E_DialogFuncType.Type2, 6, sceneObjectCollisionDialogData[2].Text);
        else if(greenAlgaeCount == 2)
            DialogManager.Instance.ShowDialogPanel(E_DialogFuncType.Type2, 7, sceneObjectCollisionDialogData[3].Text);
        else if(greenAlgaeCount == 3)
            DialogManager.Instance.ShowDialogPanel(E_DialogFuncType.Type2, 8, sceneObjectCollisionDialogData[4].Text);
        else if(greenAlgaeCount == 4)
            DialogManager.Instance.ShowDialogPanel(E_DialogFuncType.Type2, 9, sceneObjectCollisionDialogData[5].Text);
        else if(greenAlgaeCount == 5)
            DialogManager.Instance.ShowDialogPanel(E_DialogFuncType.Type2, 10, sceneObjectCollisionDialogData[6].Text);
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
        }
    }

    public void PlaySoundByTriggerType(E_SoundTriggerType triggerType, bool isLoop = false, bool isAsync = false ,UnityAction<AudioSource> callback = null)
    {
        switch(triggerType)
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
        switch(type)
        {
            case E_SceneType.GameScene1:
                GameObject playerObj = Instantiate(ResMgr.Instance.Load<GameObject>(PathUtil.PlayerPath + "Player1"));
                player = playerObj;
                playerObj.transform.position = GameDataMgr.Instance.gameData.playerData.playerPos != null 
                ? new Vector3(GameDataMgr.Instance.gameData.playerData.playerPos._x, 
                GameDataMgr.Instance.gameData.playerData.playerPos._y,
                GameDataMgr.Instance.gameData.playerData.playerPos._z) 
                : position;
                ChangePlayerScale(GameDataMgr.Instance.gameData.playerData.playerState);
                break;
        }
    }

    public void SetPlayer(GameObject playerObj)
    {
        player = playerObj;
    }

    public void ChangePlayerScale(E_PlayerState state)
    {
        switch(state)
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
}
