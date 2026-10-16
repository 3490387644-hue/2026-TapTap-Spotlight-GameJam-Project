using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MainPanel : BasePanel
{
    public GameObject[] mainPanels; // 两种主面板
    public Button[] buttons;
    private Dictionary<string, Button> buttonDic = new Dictionary<string, Button>();
    private int pIndex = 0; // 当前主面板索引
    public override void Init()
    {
        for(int i = 0; i < mainPanels.Length; i++)
        {
            mainPanels[i].SetActive(i == pIndex);
        }
        // 面板的所有按钮都放到字典里
        buttonDic.Clear();
        for(int i = 0; i < buttons.Length; i++)
        {
            buttonDic.Add(buttons[i].name, buttons[i]);
        }
        // 根据面板类型为按钮添加点击事件
        UpdateInfoBySaveData();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        GameDataMgr.Instance.Init(); // 确保游戏数据已初始化
        pIndex = GameDataMgr.Instance.gameData1.hasSaveData ? 1 : 0;
        for(int i = 0; i < mainPanels.Length; i++)
        {
            mainPanels[i].SetActive(i == pIndex);
        }
    }

    /// <summary>
    /// 根据是否有存档数据来更新按钮的显示状态
    /// 1.如果有存档 开始游戏变成继续游戏 功能也发生改变
    /// 2.如果有存档 新游戏按钮显示；如果没有存档 新游戏按钮隐藏
    /// </summary>
    private void UpdateInfoBySaveData()
    {
        buttonDic["StartButton"].onClick.AddListener(OnClickStartButton);
        buttonDic["SettingButton"].onClick.AddListener(OnClickSettingButton);
        buttonDic["QuitButton"].onClick.AddListener(OnClickQuitButton);
        buttonDic["AboutButton"].onClick.AddListener(OnClickAboutButton);
        buttonDic["ContinueButton"].onClick.AddListener(OnClickContinueButton);
        buttonDic["NewGameButton"].onClick.AddListener(OnClickNewGameButton);
    }

    private void OnClickStartButton()
    {
        Debug.Log("开始游戏");
        UIManager.Instance.HidePanel<MainPanel>();
        E_SceneType sceneType = GameDataMgr.Instance.globalGameData.sceneType == E_SceneType.None ? E_SceneType.GameScene01 : GameDataMgr.Instance.globalGameData.sceneType;
        GameSceneManager.Instance.LoadScene(E_SceneType.GameScene03, () =>
        {
            DialogManager.Instance.ShowType1Dialog(1);
            GameSceneManager.Instance.PlayMusicByTriggerType(E_MusicTriggerType.Chapter3_Level1);
        });
    }

    private void OnClickContinueButton()
    {
        Debug.Log("继续游戏");
        GameDataMgr.Instance.Init(); // 重新加载存档数据，确保对话触发状态正确
        UIManager.Instance.HidePanel<MainPanel>();
        GameSceneManager.Instance.LoadScene(GameDataMgr.Instance.globalGameData.sceneType, ()=>
        {
            GameSceneManager.Instance.PlayMusicByTriggerType(GameDataMgr.Instance.globalGameData.currentMusicTriggerType);
        });
        // UIManager.Instance.HidePanel<MainPanel>(true, (panel)=>
        // {
        //     DialogManager.Instance.Init();
        //     DialogManager.Instance.ShowDialogPanel(E_DialogFuncType.Type1);
        // });
        // 根据存档数据的场景类型来决定进入哪个场景
        
    }

    private void OnClickSettingButton()
    {
        Debug.Log("设置");
        // 点击按钮 打开设置面板
        UIManager.Instance.ShowPanelAsync<SettingPanel>((panel)=>
        {
            panel.SetSource(E_SettingSource.MainPanel);
        });
        UIManager.Instance.HidePanel<MainPanel>();
    }

    private void OnClickNewGameButton()
    {
        Debug.Log("新游戏");
        UIManager.Instance.ShowPanelAsync<ClearPanel>();
        UIManager.Instance.HidePanel<MainPanel>();
    }

    private void OnClickQuitButton()
    {
        Debug.Log("退出游戏");
        if(GameSceneManager.Instance.player != null)
        {
            GameDataMgr.Instance.SaveGameData();
        }
        Application.Quit();
    }

    private void OnClickAboutButton()
    {
        Debug.Log("关于");
        UIManager.Instance.ShowPanelAsync<AboutPanel>();
        UIManager.Instance.HidePanel<MainPanel>();
    }
}
