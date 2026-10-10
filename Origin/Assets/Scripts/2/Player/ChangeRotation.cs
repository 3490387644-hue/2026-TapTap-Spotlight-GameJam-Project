using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ChangeRotation : MonoBehaviour
{
    public float rayLength = 1; //射线长度
    public bool isRight = true; //玩家是否面朝右边
    public float rotationSpeed = 0.5f; //角度变换速度
    public LayerMask mask; //射线检测层
    private RaycastHit2D hit; //射线击中点
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (isRight)
        {
            hit = Physics2D.Raycast(transform.position, transform.right, rayLength, mask);
            if(hit)
            {
                float targetAngle = Mathf.Atan2(hit.normal.y, hit.normal.x) * Mathf.Rad2Deg - 90;
                // 平滑插值旋转，避免瞬间猛转抖动
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, targetAngle), rotationSpeed * Time.fixedDeltaTime);
            }
        }
        else
        {
            Physics2D.Raycast(transform.position, transform.right, rayLength, mask);
            if (hit)
            {
                float targetAngle = Mathf.Atan2(hit.normal.y, hit.normal.x) * Mathf.Rad2Deg + 90;
                // 平滑插值旋转，避免瞬间猛转抖动
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, targetAngle), rotationSpeed * Time.fixedDeltaTime);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position+transform.right);
        Gizmos.DrawLine(transform.position, transform.position-transform.right);
    }
}
