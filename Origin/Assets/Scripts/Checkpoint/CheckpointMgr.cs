using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointMgr : BaseManager<CheckpointMgr>
{
    // private GameObject checkpoint;
    
    //更新复活点
    public void PushCheckPoint(GameObject point)
    {
        CheckPointData data = new CheckPointData
        {
            checkPointPos = new PositionData(point.transform.position.x, point.transform.position.y, point.transform.position.z)
        };
        GameSceneManager.Instance.UpdateCheckPointData(data);
        GameDataMgr.Instance.SaveGameData(); // 每遇到一个新的复活点就保存一次游戏数据
    }

    //取得最新的复活点
    public PositionData GetCheckPoint()
    {
        return GameSceneManager.Instance.GetCheckPointData.checkPointPos;
    }
}
