using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 事件中心模块中的事件枚举
/// </summary>
public enum E_EventType
{ 
    SoundValueChanged, // 音效变化时触发
    DialogDoing,
    DialogType,
    MagmaTrigger,
    NPCTrigger,
    MechanismTrigger
}
