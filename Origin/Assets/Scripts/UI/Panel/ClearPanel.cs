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
        yesButton.onClick.AddListener(() =>
        {
            // 1) 先清档，异常包住，绝不影响界面切换
            try
            {
                GameDataMgr.Instance.ClearAllSaveData();
            }
            catch (System.Exception e)
            {
                Debug.LogError("[ClearPanel] 清档失败：" + e);
            }

            // 2) 直接隐藏自己，不要放进 ShowPanelAsync 的回调里
            UIManager.Instance.HidePanel<ClearPanel>();

            // 3) 显示主面板（此时 OnEnable 读到的已经是清空后的数据）
            UIManager.Instance.ShowPanelAsync<MainPanel>();
        });

        noButton.onClick.AddListener(() =>
        {
            UIManager.Instance.ShowPanelAsync<MainPanel>();
            UIManager.Instance.HidePanel<ClearPanel>();
        });
    }
}
