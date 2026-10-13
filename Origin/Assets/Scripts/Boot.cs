using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏启动脚本
/// 主要用于游戏启动时的初始化操作
/// </summary>
public class Boot : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // 加载游戏数据
        GameSceneManager.Instance.LoadScene(E_SceneType.BeginScene, ()=>
        {
            GameDataMgr.Instance.Init();
            Debug.Log("Boot:资源加载完成，游戏数据初始化完成");
            UIManager.Instance.ShowPanelAsync<MainPanel>((panel)=>
            {
                Debug.Log("Boot:主面板加载完成");
            });
        });
    }
}
