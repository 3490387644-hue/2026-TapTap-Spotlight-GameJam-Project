using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_DialogFuncType
{
    Type1, // 一定触发-打印机式触发
    Type2, // 一定触发-触碰碰撞体后触发
    None
}

/// <summary>
/// 对话数据类
/// </summary>
public class DialogData
{
    public int ID; // 对话ID
    public string Text; // 对话文本
    public E_DialogFuncType Type; // 对话功能类型
    public bool HasTriggered; // 对话是否已经触发过
}
