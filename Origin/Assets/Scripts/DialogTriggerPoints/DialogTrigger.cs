using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    private DialogData dialogData;
    private bool hasTriggered = false; // 用于标记是否已经触发过
    public int dialogCount = 1; // 对话触发的次数，默认为1次
    void Start()
    {
        
    }

    public void Init(DialogData dialogData, int dialogCount)
    {
        this.dialogData = dialogData;
        hasTriggered = dialogData.HasTriggered; // 初始化触发状态
        this.dialogCount = dialogCount; // 初始化对话触发次数
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasTriggered) return; // 如果已经触发过，直接返回
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("DialogTrigger触发: " + this.gameObject.name);
            hasTriggered = true; // 设置为已触发，防止重复触发
            DialogManager.Instance.ShowType2Dialog(E_DialogDataType.AirwallTriggerText, dialogData.ID, dialogCount);
            GameDataMgr.Instance.UpdateAirwallTriggerState(dialogData.ID); // 更新触发状态
        }
    }
}
