using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointMgr : BaseManager<CheckpointMgr>
{
    private GameObject checkpoint;
    
    //更新复活点
    public void PushCheckPoint(GameObject point)
    {
        checkpoint = point;
    }

    //取得最新的复活点
    public GameObject GetCheckPoint()
    {
        return checkpoint;
    }
}
