using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

// 挂火把上的，但是因为现在没有火把资源 所以先挂空物体上
public class Torch : MonoBehaviour
{
    public Transform checkPoint; //范围检测点
    public float lastTime = 3f; // 启动机关所需时间
    public LayerMask mask; //检测层
    private float radius = 5f; // 检测半径
    private float timer; // 计时器
    private GameObject obj; //检测到的机关对象
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Collider2D hit = Physics2D.OverlapCircle(checkPoint.position, radius, mask);
            obj = hit != null ? hit.gameObject : null;
            if(obj != null)
            {
                Debug.Log("Torch:检测到机关对象，启动机关");
            }
            else
            {
                Debug.Log("Torch:未检测到机关对象");
            }
        }
        if (obj != null && Input.GetKey(KeyCode.Q))
        {
            timer += Time.deltaTime;
            if(timer >= lastTime)
            {
                obj.GetComponent<Mechanism>().OpenMechanism();
                // 机关启动后 火把失灵
                Destroy(this.gameObject);
            }
        }
    }
}
