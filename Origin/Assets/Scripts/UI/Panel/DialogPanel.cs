using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// 类型1：打字机效果 按任意键继续
// 类型2：直接显示完整文本 2.5s后消失
public class DialogPanel : BasePanel
{
    public Text dialogText1;
    public Image dialogImage1;
    public Text dialogText2;
    public Image dialogImage2;
    public CanvasGroup type2Group; // 这个是类型2的文本和图片的父对象上的CanvasGroup 用于将它们一起隐藏和显示
    public Text tip1;
    private UnityAction onDialogFinished; // 对话结束时的回调函数
    public override void Init()
    {
        tip1.text = "按任意键继续";
        tip1.gameObject.SetActive(false);
    }

    private void FunctionByDialogType(E_DialogFuncType type)
    {
        switch(type)
        {
            case E_DialogFuncType.Type1:
                // 执行类型1的功能
                Func1();
                break;
            case E_DialogFuncType.Type2:
                // 执行类型2的功能
                Func2();
                break;
            case E_DialogFuncType.Type3:
                // 执行类型3的功能
                Func3();
                break;
        }
    }

    private void Func1()
    {
        // 锁住玩家移动的逻辑
        
    }

    private void Func2()
    {
        
    }

    private void Func3()
    {
        
    }
}
