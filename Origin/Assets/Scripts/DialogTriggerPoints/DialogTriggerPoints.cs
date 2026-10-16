using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 这个文本的触发是一定会触发的，触发条件是碰到碰撞体 
public class DialogTriggerPoints : MonoBehaviour
{
    public GameObject[] triggerPoints;
    public int startIndex; // 对话触发的起始索引
    public int endIndex; // 对话触发的结束索引
    public int dialogCount; // 对话触发的数量
    // Start is called before the first frame update
    void Start()
    {
        int triggerIndex = 0;
        for (int i = startIndex; i <= endIndex; i++)
        {
            DialogTrigger dialogTrigger = triggerPoints[triggerIndex].AddComponent<DialogTrigger>();
            dialogTrigger.Init(GameDataMgr.Instance.airwallTriggerDialogData[i], dialogCount);
            triggerIndex++;
        }
    }
}
