using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    private DialogData dialogData;
    private bool hasTriggered = false; // 用于标记是否已经触发过
    void Start()
    {
        
    }

    public void Init(DialogData dialogData)
    {
        this.dialogData = dialogData;
        hasTriggered = dialogData.HasTriggered; // 初始化触发状态
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasTriggered) return; // 如果已经触发过，直接返回
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("DialogTrigger触发: " + this.gameObject.name);
            hasTriggered = true; // 设置为已触发，防止重复触发
            DialogManager.Instance.ShowDialogPanel(dialogData.Type, 1, dialogData.Text);
            GameDataMgr.Instance.UpdateAirwallTriggerState(dialogData.ID); // 更新触发状态
        }
    }
}
