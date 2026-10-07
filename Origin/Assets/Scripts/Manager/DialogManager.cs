using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogManager : SingletonAutoMono<DialogManager>
{
    private bool isDialogFinished = false; // 对话是否结束
    private int nowDialogIndex; // 当前对话索引
    private List<DialogData> dialogData = new List<DialogData>();
    private DialogPanel dialogPanel; // 对话面板引用
    private UnityAction onDialogFinished; // 对话结束时的回调函数
    private float elapsedTime = 0f;
    private float duration = 2.5f; // 对话显示持续时间
    private Coroutine type1Coroutine; // 类型1的协程引用
    private Coroutine type2Coroutine; // 类型2的协程引用

    public void Init()
    {
        dialogData = GameDataMgr.Instance.dialogData;
        nowDialogIndex = 0;
    }

    public void ShowDialogPanel(E_DialogFuncType type)
    {
        if(nowDialogIndex >= dialogData.Count)
        {
            Debug.Log("没有更多的对话数据");
            return;
        }
        UIManager.Instance.ShowPanelAsync<DialogPanel>((panel)=>
        {
            Debug.Log("DialogPanel loaded and shown");
            if(dialogPanel == null)
                dialogPanel = panel;
            this.UpdateText(type);
        });
    }

    public void UpdateText(E_DialogFuncType type)
    {
        switch (type)
        {
            case E_DialogFuncType.Type1:
                dialogPanel.dialogImage1.gameObject.SetActive(true);
                dialogPanel.dialogText1.gameObject.SetActive(true);
                dialogPanel.type2Group.gameObject.SetActive(false);
                dialogPanel.tip1.gameObject.SetActive(false);
                if(type1Coroutine != null)
                    StopCoroutine(type1Coroutine);
                type1Coroutine = StartCoroutine(LoadTextByType1(type));
                break;
            case E_DialogFuncType.Type2:
                dialogPanel.dialogImage1.gameObject.SetActive(false);
                dialogPanel.dialogText1.gameObject.SetActive(false);
                dialogPanel.type2Group.gameObject.SetActive(true);
                dialogPanel.tip1.gameObject.SetActive(false);
                if(type2Coroutine != null)
                    StopCoroutine(type2Coroutine);
                type2Coroutine = StartCoroutine(LoadTextByType2(type));
                break;
        }
    }

    // 根据对话类型加载对应功能的文本
    IEnumerator LoadTextByType1(E_DialogFuncType type)
    {
        if(type != E_DialogFuncType.Type1)
        {
            Debug.LogError("对话类型出错");
            yield break;
        }
        yield return new WaitForSeconds(1f); // 等待0.5秒后开始显示对话
        Debug.Log("当前对话类型: Type1; nowDialogIndex: " + nowDialogIndex);
        isDialogFinished = false;
        E_DialogFuncType dialogType;
        while (nowDialogIndex < dialogData.Count)
        {
            dialogType = (E_DialogFuncType)dialogData[nowDialogIndex].Type;
            if (dialogType != E_DialogFuncType.Type1)
            {
                Debug.Log($"当前对话类型: Type1 已结束");
                break;
            }

            // 测试阶段 均加载到dialogText1中
            string fullText = dialogData[nowDialogIndex].Text;
            dialogPanel.dialogText1.text = "";
            // 实现打字机效果
            for (int i = 0; i < fullText.Length; i++)
            {
                dialogPanel.dialogText1.text += fullText[i];
                yield return new WaitForSeconds(0.05f); // 每个字母间隔0.05秒
            }

            Debug.Log($"Type1: nowDialogIndex : {nowDialogIndex} 对话内容：" + dialogData[nowDialogIndex].Text);

            // yield return new WaitForSeconds(1f); // 等待1秒后显示提示
            dialogPanel.tip1.gameObject.SetActive(true);
            while (!Input.anyKeyDown)
            {
                yield return null;
            }
            nowDialogIndex++;
            dialogPanel.tip1.gameObject.SetActive(false);
        }
        // 对话结束后切换面板
        UIManager.Instance.HidePanel<DialogPanel>(true, (panel) =>
        {
            isDialogFinished = true;
            // UIManager.Instance.ShowPanelAsync<MainPanel>();
            // 回调函数

        });
    }

    // 触发式对话
    IEnumerator LoadTextByType2(E_DialogFuncType type)
    {
        if(type != E_DialogFuncType.Type2)
        {
            Debug.LogError("对话类型出错");
            yield break;
        }

        elapsedTime = 0f; // 重置计时器
        duration = 2.5f; // 对话显示持续时间
        dialogPanel.type2Group.alpha = 1f;

        dialogPanel.dialogText2.text = dialogData[nowDialogIndex].Text;
        Debug.Log($"Type2: nowDialogIndex : {nowDialogIndex} 对话内容：" + dialogData[nowDialogIndex].Text);
        nowDialogIndex++;
        yield return new WaitForSeconds(2.5f); // 等待2.5秒后隐藏对话面板

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);
            dialogPanel.type2Group.alpha = alpha;

            yield return null;
        }

        dialogPanel.type2Group.alpha = 0f;
        dialogPanel.type2Group.gameObject.SetActive(false);

        dialogPanel.dialogText2.text = "";
        elapsedTime = 0f; // 重置计时器
        UIManager.Instance.HidePanel<DialogPanel>(true, (panel) =>
        {
            isDialogFinished = true;
            // UIManager.Instance.ShowPanelAsync<MainPanel>();
            // 回调函数

        });
    }
}
