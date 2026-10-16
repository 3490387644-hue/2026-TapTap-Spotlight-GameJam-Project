using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BornPointMgr : MonoBehaviour
{
    private static BornPointMgr instance;
    public static BornPointMgr Instance=>instance;

    public List<Transform> bornPoints; //每关重生点

    public int index; //当前记录重生点

    private void Awake()
    {
        instance = this;
    }

    //更新重生点函数
    public void UpdatePoint(GameObject obj)
    {
        index++;
        obj.transform.position=bornPoints[index].transform.position;
    }
}
