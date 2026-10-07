using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClearPanel : BasePanel
{
    public Button yesButton;
    public Button noButton;
    public override void Init()
    {
        //初始化面板的逻辑
        yesButton.onClick.AddListener(()=>
        {
            // // 播放开场动画
            
            // 测试脚本
            UIManager.Instance.ShowPanelAsync<LoadingPanel>((panel)=>
            {
                panel.InitProgress("GameScene");
            });
            UIManager.Instance.HidePanel<ClearPanel>();
            UIManager.Instance.HidePanel<MainPanel>();
        });

        noButton.onClick.AddListener(()=>
        {
            UIManager.Instance.ShowPanelAsync<MainPanel>();
            UIManager.Instance.HidePanel<ClearPanel>();
        });
    }
}
