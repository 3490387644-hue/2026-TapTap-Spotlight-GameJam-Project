using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BornPointMgr : MonoBehaviour
{
    private static BornPointMgr instance;
    public static BornPointMgr Instance=>instance;

        public List<Transform> bornPoints; //每个关卡出生点

        public int index; //当前记录的出生点

    private void Awake()
    {
        instance = this;
    }

    //更新出生点函数
    public void UpdatePoint(GameObject obj, int targetIndex)
    {
        index = Mathf.Clamp(targetIndex, 0, bornPoints.Count - 1);
        if(index == 1)
            GameSceneManager.Instance.PlayMusicByTriggerType(E_MusicTriggerType.Chapter2_Level2);
        else if(index == 2)
            GameSceneManager.Instance.PlayMusicByTriggerType(E_MusicTriggerType.Chapter2_Level3);
        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.position = bornPoints[index].transform.position;
        else
            obj.transform.position = bornPoints[index].transform.position;
    }
}
