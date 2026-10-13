using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitPanel : BasePanel
{
    public Button QuitButton;
    
    override public void Init()
    {
        QuitButton.onClick.AddListener(()=>
        {
            Application.Quit();
        });
    }
}
