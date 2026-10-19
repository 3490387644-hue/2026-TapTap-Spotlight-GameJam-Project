using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingPanel : BasePanel
{
    public Slider progressSlider;
    public Text progressText;
    public Text progressDesc;
    private float minLoadingTime = 0.5f; // 最小加载时间，确保玩家能看到加载界面

    public override void Init()
    {
        isDestroy = true;
        progressSlider.value = 0f;
    }

    /// <summary>
    /// 进行场景加载
    /// 因为需要反复用到 所以不能直接写在Init中
    /// </summary>
    public void InitProgress(E_SceneType sceneType, UnityAction callback = null)
    {
        string resPath = sceneType.ToString(); // 将场景类型转换为字符串，作为场景的路径
        if(string.IsNullOrEmpty(resPath))
        {
            Debug.LogError("加载路径为空");
            return;
        }

        SetProgress(0f);

        //提前预加载第一章的背景音乐 避免进入游戏后切换音乐时卡顿
        MusicMgr.Instance.PreloadBKMusic(PathUtil.MusicPathOfChapter1 + "1-1");
        MusicMgr.Instance.PreloadBKMusic(PathUtil.MusicPathOfChapter1 + "1-2");
        MusicMgr.Instance.PreloadBKMusic(PathUtil.MusicPathOfChapter1 + "1-3");

        StartCoroutine(LoadScene(resPath, callback));
    }

    IEnumerator LoadScene(string sceneName, UnityAction callback = null)
    {
        MusicMgr.Instance.ClearSound();
        float startTime = Time.realtimeSinceStartup;

        if (progressDesc != null)
        {
            progressDesc.text = "正在加载场景...";
        }

        AsyncOperation operation =
            SceneManager.LoadSceneAsync(sceneName);

        if (operation == null)
        {
            Debug.LogError("场景加载失败：" + sceneName);
            yield break;
        }

        // 暂时不激活场景
        operation.allowSceneActivation = false;

        while (operation.progress < 0.9f)
        {
            SetProgress(operation.progress);

            yield return null;
        }

        // 场景已经准备完成，但不能立即进入
        SetProgress(0.9f);

        if (progressDesc != null)
        {
            progressDesc.text = "正在完成加载...";
        }

        // 确保 LoadingPanel 至少显示 0.5 秒
        float elapsedTime =
            Time.realtimeSinceStartup - startTime;

        float remainingTime =
            minLoadingTime - elapsedTime;

        if (remainingTime > 0f)
        {
            yield return new WaitForSecondsRealtime(remainingTime + 1f);
        }

        SetProgress(1f);

        if (progressDesc != null)
        {
            progressDesc.text = "加载完成";
        }

        // 确保玩家能看到 100%
        yield return null;

        // 激活场景
        operation.allowSceneActivation = true;
        yield return null;
        Debug.Log("场景加载完成：" + sceneName);
        UIManager.Instance.HidePanel<LoadingPanel>(true, (panel)=>
        {
            Debug.Log("DialogManager");
            E_SceneType scene = GameSceneManager.Instance.currentSceneType;
            if(scene == E_SceneType.GameScene01 || scene == E_SceneType.GameScene02)
                UIManager.Instance.ShowPanelAsync<QuitPanel>();
            else
            {
                if(UIManager.Instance.GetPanel<QuitPanel>() != null)
                    UIManager.Instance.HidePanel<QuitPanel>();
            }
            callback?.Invoke();
        });
    }

    private void SetProgress(float progress)
    {
        progress = Mathf.Clamp01(progress);

        if(progressSlider != null)
        {
            progressSlider.value = progress;
        }

        if(progressText != null)
        {
            progressText.text = Mathf.RoundToInt(progress * 100) + "%";
        }

        if(progressDesc != null)
        {
            progressDesc.text = "加载中...";
        }
    }
}
