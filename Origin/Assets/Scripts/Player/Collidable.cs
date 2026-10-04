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
                        Destroy(colliders[i].gameObject);
                        if(colliders[i].gameObject.GetComponent<RockNum>().isBornAlgae)
                        Instantiate(ResMgr.Instance.Load<GameObject>("Text/green algae"), p, r);
                    }
                }
            }
        }
    }
}
