using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockNum : MonoBehaviour
{
    //石头需要撞击几次才碎裂
    public int collidableNum;
    public int rockID; //石头的ID
    //是否生成绿藻
    public bool isBornAlgae;
    //是否生成黑雾
    public bool isBornFog;

    void Start()
    {
        var data = GameDataMgr.Instance.gameData1.rockDataList.Find(r => r.rockID == rockID);
        if (data != null && data.isBreak)
        {
            Destroy(this.gameObject);   // 已碎，直接不生成
            return;
        }
        if (data == null)               // 第一次进入，注册一条默认数据
        {
            GameDataMgr.Instance.gameData1.rockDataList.Add(new RockData { rockID = rockID, isBreak = false });
        }
    }
}
