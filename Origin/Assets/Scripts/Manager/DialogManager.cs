using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogManager : SingletonAutoMono<DialogManager>
{
    private bool isDialogFinished = false; // 对话是否结束
    private int nowDialogIndex; // 当前对话索引
    private List<DialogData> dialogData = new List<DialogData>();
    // private List<int>
    private DialogPanel dialogPanel; // 对话面板引用
    private UnityAction onDialogFinished; // 对话结束时的回调函数
    private float elapsedTime = 0f;
    private float duration = 2.5f; // 对话显示持续时间
    private Coroutine type1Coroutine; // 类型1的协程引用
    private Coroutine type2Coroutine; // 类型2的协程引用

    public void Init()
    {
        nowDialogIndex = 0;
    }

    // 类型1：一定触发-打字机式对话
    public void ShowType1Dialog(int dialogIndex, int dialogCount = 1)
    {
        UIManager.Instance.ShowPanelAsync<DialogPanel>((panel)=>
        {
            Debug.Log("DialogPanel loaded and shown");
            if(dialogPanel == null)
                dialogPanel = panel;
            dialogPanel.dialogImage1.gameObject.SetActive(true);
            dialogPanel.dialogText1.gameObject.SetActive(true);
            dialogPanel.type2Group.gameObject.SetActive(false);
            dialogPanel.tip1.gameObject.SetActive(false);
            dialogPanel.dialogText1.text = ""; // 清空之前的文本
            if(type1Coroutine != null)
                StopCoroutine(type1Coroutine);
            type1Coroutine = StartCoroutine(LoadTextByType1(dialogIndex, dialogCount));
        });
    }

    // 类型2：一定触发-触碰碰撞体后触发式对话，根据 dataType 和索引从对应数据列表读取文本
    public void ShowType2Dialog(E_DialogDataType dataType, int dialogIndex, int dialogCount = 1)
    {
        UIManager.Instance.ShowPanelAsync<DialogPanel>((panel)=>
        {
            Debug.Log("DialogPanel loaded and shown");
            if(dialogPanel == null)
                dialogPanel = panel;
            dialogPanel.dialogImage1.gameObject.SetActive(false);
            dialogPanel.dialogText1.gameObject.SetActive(false);
            dialogPanel.type2Group.gameObject.SetActive(true);
            dialogPanel.tip1.gameObject.SetActive(false);
            dialogPanel.dialogText2.text = ""; // 清空之前的文本
            if(type2Coroutine != null)
                StopCoroutine(type2Coroutine);
            type2Coroutine = StartCoroutine(LoadTextByType2(dataType, dialogIndex, dialogCount));
        });
    }

    // 打字机式对话协程
    IEnumerator LoadTextByType1(int dialogIndex, int dialogCount)
    {
        nowDialogIndex = dialogIndex;
        EventCenter.Instance.EventTrigger<bool>(E_EventType.DialogDoing, false); // 禁止玩家移动
        yield return new WaitForSeconds(1f); // 等待0.5秒后开始显示对话
        isDialogFinished = false;
        E_DialogFuncType dialogType;
        while (dialogCount > 0)
        {
            GameDataMgr.Instance.sceneObjectCollisionDialogData[nowDialogIndex].HasTriggered = true; // 标记当前文本已触发
            dialogType = (E_DialogFuncType)GameDataMgr.Instance.typewritterDialogData[nowDialogIndex].Type;
            if (dialogType != E_DialogFuncType.Type1)
            {
                Debug.Log($"当前对话类型: Type1 已结束");
                break;
            }

            // 测试阶段 均加载到dialogText1中
            string fullText = GameDataMgr.Instance.typewritterDialogData[nowDialogIndex].Text;
            GameDataMgr.Instance.typewritterDialogData[nowDialogIndex].HasTriggered = true; // 标记当前文本已触发
            dialogPanel.dialogText1.text = "";
            // 实现打字机效果
            for (int i = 0; i < fullText.Length; i++)
            {
                dialogPanel.dialogText1.text += fullText[i];
                yield return new WaitForSeconds(0.05f); // 每个字母间隔0.05秒
            }

            // yield return new WaitForSeconds(1f); // 等待1秒后显示提示
            dialogPanel.tip1.gameObject.SetActive(true);
            while (!Input.anyKeyDown)
            {
                yield return null;
            }
            nowDialogIndex++;
            dialogCount--;
            dialogPanel.tip1.gameObject.SetActive(false);
        }
        // 对话结束后切换面板
        UIManager.Instance.HidePanel<DialogPanel>(true, (panel) =>
        {
            isDialogFinished = true;
            // UIManager.Instance.ShowPanelAsync<MainPanel>();
            // // 回调函数
            ShowFirstDialogAfterEnterScene();
            EventCenter.Instance.EventTrigger<bool>(E_EventType.DialogDoing, true); // 允许玩家移动
        });
    }

    private void ShowFirstDialogAfterEnterScene()
    {
        E_SceneType currentScene = GameSceneManager.Instance.currentSceneType;
        switch(currentScene)
        {
            case E_SceneType.GameScene01:
                if(!GameDataMgr.Instance.sceneObjectCollisionDialogData[0].HasTriggered)
                {
                    DialogManager.Instance.ShowType2Dialog(E_DialogDataType.SceneObjectCollisionText, 0);
                }
                break;
            case E_SceneType.GameScene03:
                if(!GameDataMgr.Instance.sceneObjectCollisionDialogData[9].HasTriggered)
                {
                    DialogManager.Instance.ShowType2Dialog(E_DialogDataType.SceneObjectCollisionText, 9);
                }
                break;
        }
    }

    // 触发式对话（支持按 dialogCount 连续播放多条文本）
    IEnumerator LoadTextByType2(E_DialogDataType dataType, int dialogIndex, int dialogCount)
    {
        List<DialogData> dataList = GetDialogDataList(dataType);
        if(dataList == null)
        {
            Debug.LogWarning("对话数据类型无效：" + dataType);
            yield break;
        }

        int index = dialogIndex;
        while (dialogCount > 0)
        {
            if(index < 0 || index >= dataList.Count)
            {
                Debug.LogWarning($"对话索引越界：{index}，数据数量：{dataList.Count}");
                break;
            }
            DialogData data = dataList[index];
            data.HasTriggered = true; // 标记当前文本已触发

            // 以下为 Type2 的文本出现形式（淡入淡出）
            elapsedTime = 0f; // 重置计时器
            duration = 2.5f; // 对话显示持续时间
            dialogPanel.type2Group.alpha = 1f;
            dialogPanel.type2Group.gameObject.SetActive(true);

            dialogPanel.dialogText2.text = data.Text;
            Debug.Log($"对话内容：" + data.Text);
            yield return new WaitForSeconds(2.5f); // 等待2.5秒后隐藏对话面板

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;

                float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);
                dialogPanel.type2Group.alpha = alpha;

                yield return null;
            }

            dialogPanel.type2Group.alpha = 0f;
            dialogPanel.dialogText2.text = "";

            index++;
            dialogCount--;
        }

        dialogPanel.type2Group.gameObject.SetActive(false);
        elapsedTime = 0f; // 重置计时器
        UIManager.Instance.HidePanel<DialogPanel>(true, (panel) =>
        {
            isDialogFinished = true;
            // UIManager.Instance.ShowPanelAsync<MainPanel>();
            // 回调函数

        });
    }

    // 根据对话数据类型返回对应的对话数据列表
    private List<DialogData> GetDialogDataList(E_DialogDataType dataType)
    {
        switch(dataType)
        {
            case E_DialogDataType.SceneObjectCollisionText:
                return GameDataMgr.Instance.sceneObjectCollisionDialogData;
            case E_DialogDataType.AirwallTriggerText:
                return GameDataMgr.Instance.airwallTriggerDialogData;
            default:
                Debug.LogWarning("未知的对话数据类型：" + dataType);
                return null;
        }
    }
}
