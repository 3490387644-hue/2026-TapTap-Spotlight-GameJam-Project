using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : BasePanel
{
    public Button continueButton;
    public Button settingButton;
    public Button backButton;
    public override void Init()
    {
        //初始化面板的逻辑
        continueButton.onClick.AddListener(()=>
        {
            UIManager.Instance.HidePanel<GamePanel>();
        });

        settingButton.onClick.AddListener(()=>
        {
            UIManager.Instance.ShowPanelAsync<SettingPanel>();
        });

        backButton.onClick.AddListener(()=>
        {
            UIManager.Instance.HidePanel<GamePanel>();
            UIManager.Instance.ShowPanelAsync<MainPanel>();
        });
    }
}
