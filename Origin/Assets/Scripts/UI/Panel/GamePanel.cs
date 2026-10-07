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
            Debug.Log("继续游戏");
            UIManager.Instance.HidePanel<GamePanel>();
        });

        settingButton.onClick.AddListener(()=>
        {
            Debug.Log("设置");
            UIManager.Instance.ShowPanelAsync<SettingPanel>((panel)=>
            {
                panel.SetSource(E_SettingSource.GamePanel);
            });
            UIManager.Instance.HidePanel<GamePanel>();
        });

        backButton.onClick.AddListener(()=>
        {
            Debug.Log("返回主菜单");
            UIManager.Instance.HidePanel<GamePanel>();
            UIManager.Instance.ShowPanelAsync<MainPanel>();
        });
    }
}
