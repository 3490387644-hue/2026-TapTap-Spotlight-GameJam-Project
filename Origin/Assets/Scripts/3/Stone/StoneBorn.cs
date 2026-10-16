using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneBorn : MonoBehaviour
{
    public float bornTime; //石头生成时间间隔
    public float hStoneSpeed; //石头横向速度
    public float vStoneSpeed; //石头竖向速度
    private GameObject stoneObj; //当前的石头对象
    private float lastTime; //上次生成石头的时间
    // Start is called before the first frame update
    void Start()
    {
        lastTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time-lastTime>=bornTime)
        {
            stoneObj = Instantiate(ResMgr.Instance.Load<GameObject>("Prefabs/Stones/Stone"),this.transform.position,this.transform.rotation);
            stoneObj.GetComponent<StoneFallen>().hSpeed = hStoneSpeed;
            stoneObj.GetComponent<StoneFallen>().vSpeed = vStoneSpeed;
            lastTime = Time.time; //更新石头生成时间
            Destroy(stoneObj, 7);
        }
    }
}
