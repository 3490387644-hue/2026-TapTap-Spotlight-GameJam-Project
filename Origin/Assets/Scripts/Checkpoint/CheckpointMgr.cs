using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointMgr : BaseManager<CheckpointMgr>
{
    // 检查点对象
    private GameObject checkpoint;

    // 更新存档
    public void PushCheckPoint(GameObject point)
    {
        CheckPointData data = new CheckPointData
        {
            checkPointPos = new PositionData(point.transform.position.x, point.transform.position.y, point.transform.position.z)
        };
        GameSceneManager.Instance.UpdateCheckPointData(data);
        GameDataMgr.Instance.SaveGameData(); // 每触发一个新的检查点就保存一次游戏数据
    }

    // 获取最新的检查点
    public PositionData GetCheckPoint()
    {
        return GameSceneManager.Instance.GetCheckPointData.checkPointPos;
    }
}
