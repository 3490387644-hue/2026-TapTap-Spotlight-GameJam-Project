using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mechanism : MonoBehaviour
{
    public int mechanismID; // 机关的唯一标识符,后续用来存档的
    public Animator animator; // 动画组件
    public GameObject parent; // 机关的父物体
    private bool isOpen = false; // 机关是否已经打开
    private bool hasTriggered = false; // 防止重复触发
    private string dialogText; // 对话文本

    void Awake()
    {
        GameDataMgr.Instance.Init(); // 确保游戏数据已初始化
    }

    void Start()
    {
        animator = GetComponent<Animator>(); // 获取动画组件
        dialogText = GameSceneManager.Instance.GetSceneObjectCollisionDialogData[8].Text; // 获取对话文本
        EventCenter.Instance.AddEventListener<bool>(E_EventType.MechanismTrigger, OnMechanismTriggered);
        MechanismData mechanism = GameSceneManager.Instance.GetMechanismDataList.Find(m => m.mechanismID == mechanismID);
        if (mechanism == null)
        {
            isOpen = false; // 默认机关未打开
            GameSceneManager.Instance.GetMechanismDataList.Add(new MechanismData { mechanismID = mechanismID, isOpen = isOpen }); // 将当前机关添加到场景管理器的机关数据列表中
        }
        else
            Init(mechanism);
    }

    void OnDestroy()
    {
        EventCenter.Instance.RemoveEventListener<bool>(E_EventType.MechanismTrigger, OnMechanismTriggered);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            if(!hasTriggered)
            {
                //暂时无法播放 DialogManager.Instance.ShowDialogPanel(E_DialogFuncType.Type2, 0, dialogText);
                EventCenter.Instance.EventTrigger<bool>(E_EventType.MechanismTrigger, true); // 通知所有监听者机关触发事件
            }
        }   
    }

    private void Init(MechanismData mechanism)
    {
        isOpen = mechanism.isOpen; // 更新机关的打开状态
        if (isOpen)
            OpenMechanism(); // 如果机关已经打开，直接执行打开操作
    }

    /// <summary>
    /// 机关打开的方法
    /// </summary>
    public void OpenMechanism()
    {
        if (isOpen) return; // 如果机关已经打开，直接返回
        // 执行功能
        isOpen = true; // 设置机关为已打开状态
        // 存储机关是否打开的逻辑
        MechanismData mechanism = GameSceneManager.Instance.GetMechanismDataList.Find(m => m.mechanismID == mechanismID);
        if (mechanism != null)
            mechanism.isOpen = true; // 回写机关打开状态，用于存档

        // 播放动画
        animator.SetTrigger("RockMove");
    }

    // 用于机关打开后销毁的函数 已经挂到动画事件上了
    public void DestroyAfterOpened()
    {
        Debug.Log("Mechanism:机关已打开，销毁父物体");
        Destroy(parent);
    }

    public void OnMechanismTriggered(bool hasTriggered)
    {
        this.hasTriggered = hasTriggered;
    }
}
