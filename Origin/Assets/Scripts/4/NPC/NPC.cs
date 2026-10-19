using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public int npcID; // NPC的唯一标识符,后续用来存档的
    public bool canTalk; // 是否可以交互
    public int talkNum; // 可以交互的次数
    public bool hasTriggered; // 是否已经触发过对话框
    private string dialogText;
    // Start is called before the first frame update
    void Start()
    {
        EventCenter.Instance.AddEventListener<bool>(E_EventType.NPCTrigger, OnDialogTrigger);
        NPC npc = GameSceneManager.Instance.GetNPCList.Find(n => n.npcID == npcID);
        if (npc == null)
        {
            canTalk = true; // 默认可以交互
            GameSceneManager.Instance.GetNPCList.Add(this); // 将当前NPC添加到场景管理器的NPC列表中
        }
        else
            Init(npc);
    }

    void OnDestroy()
    {
        EventCenter.Instance.RemoveEventListener<bool>(E_EventType.NPCTrigger, OnDialogTrigger);
    }

    void Init(NPC npc)
    {
        // 这里需要获取存档中的信息
        canTalk = npc.canTalk;
        talkNum = npc.talkNum;
        hasTriggered = npc.hasTriggered;
        if (!hasTriggered)
        {
            dialogText = GameSceneManager.Instance.GetSceneObjectCollisionDialogData[7].Text; // 获取对话文本
            npc.hasTriggered = true; // 设置为已触发
            hasTriggered = true; // 设置为已触发
        }
        else dialogText = ""; // 如果已经触发过，设置为空字符串
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            if(!hasTriggered)
            {
                //暂时无法播放 DialogManager.Instance.ShowDialogPanel(E_DialogFuncType.Type2, 0, dialogText);
                EventCenter.Instance.EventTrigger<bool>(E_EventType.NPCTrigger, true); // 通知所有监听者NPC触发事件
            }
            PlayerMove4 player = collider.GetComponent<PlayerMove4>();
            if (player == null) return;
            player.CanGetTorchFromNPC = true; // 玩家进入NPC范围，可以获取火把
            player.GetNPC = this; // 获取当前交互的NPC信息
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            PlayerMove4 player = collider.GetComponent<PlayerMove4>();
            if (player == null) return;
            player.CanGetTorchFromNPC = false; // 玩家进入NPC范围，可以获取火把
            player.GetNPC = null; // 清空玩家当前交互的NPC引用
        }
    }

    private void OnDialogTrigger(bool hasTriggered)
    {
        this.hasTriggered = hasTriggered;
        // 存档逻辑
        
    }
}
