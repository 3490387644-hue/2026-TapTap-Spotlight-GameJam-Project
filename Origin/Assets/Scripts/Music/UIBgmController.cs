using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBgmController
{
    private static int panelCount; // 当前激活的面板数量
    private static bool isPlaying; // UIBGM是否正在播放
    public static void OnPanelEnable()
    {
        panelCount++;
        if (!isPlaying && panelCount > 0)
        {
            Debug.Log("播放UIBGM");
            MusicMgr.Instance.PlayBKMusic(PathUtil.MusicPathOfChapter1 + "UIBGM");
            isPlaying = true;
        }
    }

    public static void OnPanelDisable()
    {
        panelCount--;
        if (panelCount <= 0)
        {
            panelCount = 0;
            if (isPlaying)
            {
                MusicMgr.Instance.StopBKMusic();   // 先停掉 UIBGM
                isPlaying = false;
                // 只有身处游戏场景才恢复游戏音乐，菜单里不恢复
                E_SceneType scene = GameSceneManager.Instance.currentSceneType;
                if (scene == E_SceneType.GameScene01 || scene == E_SceneType.GameScene02
                || scene == E_SceneType.GameScene03 || scene == E_SceneType.GameScene04)
                {
                    GameSceneManager.Instance.PlayMusicByTriggerType(GameSceneManager.Instance.currentMusicTriggerType);
                }
            }
        }
    }
}
