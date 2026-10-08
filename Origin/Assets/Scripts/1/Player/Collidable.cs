using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Collidable : MonoBehaviour
{
    public LayerMask m_Mask; //范围检测层
    public float radius = 0.7f; //范围检测半径

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //按'S'键撞击石头 当石头需要撞击次数为0时，石头裂开
        if(Input.GetKeyDown(KeyCode.S))
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(this.transform.position, radius, m_Mask);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject.tag == "Collidable Rock")
                {
                    colliders[i].gameObject.GetComponent<RockNum>().collidableNum--;
                    if (colliders[i].gameObject.GetComponent<RockNum>().collidableNum == 0)
                    {
                        Vector3 p = colliders[i].transform.position;
                        quaternion r = colliders[i].transform.rotation;
                        colliders[i].gameObject.GetComponent<Animator>().SetTrigger("isCollidable"); //播放石头碎裂动画
                        Destroy(colliders[i].gameObject,1.5f);
                        if (colliders[i].gameObject.GetComponent<RockNum>().isBornAlgae)
                            StartCoroutine(myCoroutine(colliders[i].gameObject));
                        if(colliders[i].gameObject.GetComponent<RockNum>().isBornFog)
                            StartCoroutine(myCoroutine2(colliders[i].gameObject));

                    }
                }
            }
        }
    }

    IEnumerator myCoroutine(GameObject obj)
    {
        yield return new WaitForSeconds(1.3f);
        AlgaeBorn.Instance.AlgaeCreate(obj);
    }

    IEnumerator myCoroutine2(GameObject obj)
    {
        Vector3 p=obj.transform.position+Vector3.up;
        quaternion r=obj.transform.rotation;
        yield return new WaitForSeconds(1.35f);
        GameObject fog = Instantiate(ResMgr.Instance.Load<GameObject>("Prefabs/Fogs/Fog"),p,r);
    }

    // Gizmos绘制范围
    void OnDrawGizmosSelected()
    {
        //设置 gizmos颜色，这里绿色
        Gizmos.color = Color.green;
        // 线框球体：中心是玩家位置，半径 detectRadius
        Gizmos.DrawWireSphere(transform.position, 0.7f);
    }
}
