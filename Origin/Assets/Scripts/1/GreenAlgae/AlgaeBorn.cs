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
        //为场景上的绿藻生成点随机生成不同的绿藻，只生成未被收集的那几株
        for(int i=0;i<AlgaePoint.Count;i++)
        {
            if (!GameSceneManager.Instance.IsGreenAlgaeCollected(i))
                AlgaeCreate(AlgaePoint[i], i);
        }
    }

    //绿藻生成方法
    public void AlgaeCreate(GameObject obj, int algaeID)
    {
        int nowNum = Random.Range(0, 3);
        while(nowNum==lastNum)
        {
            nowNum = Random.Range(0, 3);
        }
        GameObject algae = Instantiate(AlgaeType[nowNum], obj.transform.position,obj.transform.rotation);
        GreenAlgae ga = algae.GetComponent<GreenAlgae>();
        if (ga != null)
            ga.algaeID = algaeID;
        lastNum = nowNum;
    }
}
