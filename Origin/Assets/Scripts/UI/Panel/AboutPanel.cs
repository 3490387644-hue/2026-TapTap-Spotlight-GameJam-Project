using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AboutPanel : BasePanel
{
    public Button backButton;
    public override void Init()
    {
        Debug.Log("AboutPanel initialized.");
        isDestroy = true;
        backButton.onClick.AddListener(()=>
        {
            UIManager.Instance.ShowPanelAsync<MainPanel>();
            UIManager.Instance.HidePanel<AboutPanel>();
        });
    }
}
