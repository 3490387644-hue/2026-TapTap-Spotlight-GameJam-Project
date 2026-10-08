using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlgaeBorn : MonoBehaviour
{
    private static AlgaeBorn instance;
    public static AlgaeBorn Instance=>instance;

    public List<GameObject> AlgaeType; //绿藻的类型
    public List<GameObject> AlgaePoint; //绿藻生成点
    private int lastNum; //上一次随机生成的数字

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        //为场景上的所有绿藻生成点随机生成不同的绿藻
        for(int i=0;i<AlgaePoint.Count;i++)
        {
            AlgaeCreate(AlgaePoint[i]);
        }
        
    }

    //绿藻生成方法
    public void AlgaeCreate(GameObject obj)
    {
        int nowNum = Random.Range(0, 3);
        while(nowNum==lastNum)
        {
            nowNum = Random.Range(0, 3);
        }
        Instantiate(AlgaeType[nowNum], obj.transform.position,obj.transform.rotation);
        lastNum = nowNum;
    }
}
