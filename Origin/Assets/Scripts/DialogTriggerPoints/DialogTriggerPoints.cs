using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 这个文本的触发是一定会触发的，触发条件是碰到碰撞体 
public class DialogTriggerPoints : MonoBehaviour
{
    public int dialogIndex = 0;
    public GameObject[] triggerPoints;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0;i < triggerPoints.Length; i++)
        {
            DialogTrigger dialogTrigger = triggerPoints[i].AddComponent<DialogTrigger>();
            dialogTrigger.Init(GameDataMgr.Instance.airwallTriggerDialogData[i]);
        }
    }
}
