using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MainPanel : BasePanel
{
    private Dictionary<string, Button> buttonDic = new Dictionary<string, Button>();
    public override void Init()
    {
        // 面板的所有按钮都放到字典里
        Button[] btns = GetComponentsInChildren<Button>();
        foreach(Button btn in btns)
        {
            btn.AddComponent<BaseButton>();
            buttonDic.Add(btn.name, btn);
            Debug.Log("Button added: " + btn.name);
        }

        // 给按钮添加点击事件
        buttonDic["SettingButton"].onClick.AddListener(OnClickSettingButton);

        buttonDic["QuitButton"].onClick.AddListener(OnClickQuitButton);

        UpdateInfoBySaveData();
    }

    /// <summary>
    /// 根据是否有存档数据来更新按钮的显示状态
    /// 1.如果有存档 开始游戏变成继续游戏 功能也发生改变
    /// 2.如果有存档 新游戏按钮显示；如果没有存档 新游戏按钮隐藏
    /// </summary>
    private void UpdateInfoBySaveData()
    {
        // 如果没有存档数据
        if(!GameDataMgr.Instance.gameData.hasSaveData)
        {
            Debug.Log("没有存档数据");
            Button startButton = buttonDic["ContinueButton"];
            startButton.name = "StartButton";
            startButton.GetComponentInChildren<Text>().text = "开始游戏";
            buttonDic.Remove("ContinueButton");
            buttonDic.Add("StartButton", startButton);

            Button aboutButton = buttonDic["AboutButton"];
            Button newGameButton = buttonDic["NewGameButton"];
            newGameButton.GetComponentInChildren<Text>().text = "关于";
            newGameButton.transform.localPosition = aboutButton.transform.localPosition;
            Destroy(aboutButton.gameObject);
            buttonDic.Remove("AboutButton");
            buttonDic.Remove("NewGameButton");
            buttonDic["AboutButton"] = newGameButton;
            newGameButton.name = "AboutButton";
            newGameButton.onClick.AddListener(OnClickAboutButton);

            startButton.onClick.AddListener(OnClickStartButton);
        }
        // 如果有存档数据
        else
        {
            buttonDic["ContinueButton"].onClick.AddListener(OnClickContinueButton);
            buttonDic["NewGameButton"].onClick.AddListener(OnClickNewGameButton);
            buttonDic["AboutButton"].onClick.AddListener(OnClickAboutButton);
        }
    }

    private void OnClickStartButton()
    {
        Debug.Log("开始游戏");
        // 待写：单击选项会有清脆敲击声
        
    }

    private void OnClickContinueButton()
    {
        Debug.Log("继续游戏");
        // UIManager.Instance.HidePanel<MainPanel>(true, (panel)=>
        // {
        //     DialogManager.Instance.Init();
        //     DialogManager.Instance.ShowDialogPanel(E_DialogFuncType.Type1);
        // });
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
        Application.Quit();
    }

    private void OnClickAboutButton()
    {
        Debug.Log("关于");
        UIManager.Instance.ShowPanelAsync<AboutPanel>();
        UIManager.Instance.HidePanel<MainPanel>();
    }
}
