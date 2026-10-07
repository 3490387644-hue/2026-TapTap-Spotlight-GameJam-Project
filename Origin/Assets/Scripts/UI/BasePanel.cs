using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public abstract class BasePanel : MonoBehaviour
{
    //整体控制淡入淡出的画布组  组件
    private CanvasGroup canvasGroup;
    //淡入淡出的速度
    private float alphaSpeed = 10;
    //是否开始显示
    private bool isShow = false;
    //当自己淡出成功时 要执行的委托函数
    private UnityAction hideCallBack;
    // 决定面板关闭时是直接销毁还是失活隐藏 true表示销毁 false表示失活隐藏
    public bool isDestroy = false;
    // 防止重复点击 想着先留着 后续可能会使用
    public bool isClicked = false;

    protected virtual void  Awake()
    {
        //一开始获取面板上挂载的组件  如果没有 我们为它添加一个  
        canvasGroup = GetComponent<CanvasGroup>();
        if(canvasGroup==null)
        {
            canvasGroup=this.AddComponent<CanvasGroup>();
        }
    }
    protected virtual void Start()
    {
        Init();
    }

    public abstract void Init();

    public virtual void ShowMe()
    {
        isShow = true;
        canvasGroup.alpha = 0;
    }

    public virtual void HideMe(UnityAction callBack)
    {
        isShow=false;
        canvasGroup.alpha = 1;
        //记录 传入的 当淡出成功后会执行的函数
        hideCallBack = callBack;
    }

    // Update is called once per frame
    void Update()
    {
        //面板淡入
        if(isShow)
        {
            canvasGroup.alpha += alphaSpeed * Time.deltaTime;
            if(canvasGroup.alpha>=1)
            {
                canvasGroup.alpha = 1;
            }
        }
        //面板淡出
        else
        {
            canvasGroup.alpha -= alphaSpeed * Time.deltaTime;
            if (canvasGroup.alpha<=0)
            {
                canvasGroup.alpha = 0;
                //应该让管理器 删除自己
                UnityAction callback = hideCallBack;
                hideCallBack = null;
                callback?.Invoke();
            }
        }
    }
}
