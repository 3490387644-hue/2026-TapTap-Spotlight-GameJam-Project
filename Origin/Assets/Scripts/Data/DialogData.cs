using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_DialogFuncType
{
    Type1,
    Type2,
    Type3,
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
}
