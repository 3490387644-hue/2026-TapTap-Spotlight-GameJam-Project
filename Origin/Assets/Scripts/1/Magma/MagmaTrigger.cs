using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagmaTrigger : MonoBehaviour
{
    private bool hasTriggered; // 防止重复触发
    private string dialogText;
    void Start()
    {
        // hasTriggered = GameSceneManager.Instance.GetSceneObjectCollisionDialogData[1].HasTriggered; // 获取是否触发
        hasTriggered = GameDataMgr.Instance.sceneObjectCollisionDialogData[1].HasTriggered; // 获取是否触发
        if(!hasTriggered)
            dialogText = GameDataMgr.Instance.sceneObjectCollisionDialogData[1].Text; // 获取对话文本
        else
            dialogText = ""; // 如果已经触发过，设置为空字符串
        EventCenter.Instance.AddEventListener<bool>(E_EventType.MagmaTrigger, OnTriggered); // 监听对话事件，传递索引1
    }

    //玩家碰到岩浆会回到最新的复活点
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="Player")
        {
            StartCoroutine(myCoroutine(collision.gameObject));
        }
    }

    IEnumerator myCoroutine(GameObject obj)
    {
        DeadChange.Instance.DarkChange();
        yield return new WaitForSeconds(1.2f);
        obj.GetComponent<Rigidbody2D>().velocity=Vector2.zero;
        obj.transform.position = CheckpointMgr.Instance.GetCheckPoint().ToVector3();
        obj.transform.position=GameSceneManager.Instance.GetCheckPointData.checkPointPos.ToVector3(); // 回到最新的复活点
        Debug.Log("MagmaTrigger触发: " + this.gameObject.name);
        if(!hasTriggered)
        {
            EventCenter.Instance.EventTrigger<bool>(E_EventType.MagmaTrigger, true); // 通知所有监听者岩浆触发事件
            // 显示对话面板
            DialogManager.Instance.ShowDialogPanel(E_DialogFuncType.Type2, 0, dialogText);
        }
    }

    void OnDestroy()
    {
        EventCenter.Instance.RemoveEventListener<bool>(E_EventType.MagmaTrigger, OnTriggered);
    }

    private void OnTriggered(bool isTriggered)
    {
        hasTriggered = isTriggered;
        GameDataMgr.Instance.sceneObjectCollisionDialogData[1].HasTriggered = true; // 写回触发状态，防止重复触发
    }
}
