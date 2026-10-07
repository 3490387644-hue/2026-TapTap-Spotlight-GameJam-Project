using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    }

    /// <summary>
    /// 进行场景加载
    /// 因为需要反复用到 所以不能直接写在Init中
    /// </summary>
    public void InitProgress(string resPath)
    {
        if(string.IsNullOrEmpty(resPath))
        {
            Debug.LogError("加载路径为空");
            return;
        }

        SetProgress(0f);

        StartCoroutine(LoadScene(resPath));
    }

    IEnumerator LoadScene(string sceneName)
    {
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
            yield return new WaitForSecondsRealtime(remainingTime);
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
        Debug.Log("场景加载完成：" + sceneName);
        UIManager.Instance.HidePanel<LoadingPanel>(true, (panel)=>
        {
            Debug.Log("DialogManager");
            DialogManager.Instance.Init();
            DialogManager.Instance.ShowDialogPanel(E_DialogFuncType.Type1);
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
