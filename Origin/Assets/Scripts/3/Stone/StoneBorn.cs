using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneBorn : MonoBehaviour
{
    public float bornTime; //石头生成时间间隔
    public float hStoneSpeed; //石头横向速度
    public float vStoneSpeed; //石头竖向速度
    private GameObject stoneObj; //当前的石头对象
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        while(true)
        {
            StartCoroutine(myCoroutine());
        }
    }

    IEnumerator myCoroutine()
    {
        yield return new WaitForSeconds(bornTime);
        stoneObj = Instantiate(ResMgr.Instance.Load<GameObject>("Prefabs/Stones/Stone"));
        stoneObj.GetComponent<StoneFallen>().hSpeed = hStoneSpeed;
        stoneObj.GetComponent<StoneFallen>().vSpeed = vStoneSpeed;
        Destroy(stoneObj,7);
    }
}
