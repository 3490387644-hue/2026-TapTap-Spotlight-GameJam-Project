using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

public class ChangeRotation : MonoBehaviour
{
    private static ChangeRotation instance;
    public static ChangeRotation Instance=>instance;

    public float rayLength = 1; //射线长度
    public bool isRight = true; //玩家是否面朝右边
    public float rotationSpeed = 0.5f; //角度变换速度
    public float rotationTime = 0.8f; //旋转时间
    public LayerMask mask; //射线检测层
    public Vector3 offset; //射线发射点偏移对象原点位置
    private RaycastHit2D Righthit; //右射线击中点
    private RaycastHit2D Lefthit; //左射线击中点
    private float rotationVelocity;
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Righthit = Physics2D.Raycast(transform.position + offset, Vector2.down, rayLength, mask);
        Lefthit = Physics2D.Raycast(transform.position - offset, Vector2.down, rayLength, mask);
        if(Righthit&&Lefthit)
        {
            Vector2 avg=(Righthit.normal+Lefthit.normal).normalized;
            float targetAngle = Mathf.Atan2(avg.y, avg.x) * Mathf.Rad2Deg - 90;

            // 角度平滑Damp，比Lerp更稳定，适合旋转跟随斜坡
            float currentAngle = transform.rotation.eulerAngles.z;
            float newAngle = Mathf.SmoothDampAngle(currentAngle, targetAngle, ref rotationVelocity, rotationTime,Mathf.Infinity,Time.fixedDeltaTime);
            transform.rotation = Quaternion.Euler(0, 0, newAngle);
        }
        else if (Righthit)
        {
            float targetAngle = Mathf.Atan2(Righthit.normal.y, Righthit.normal.x) * Mathf.Rad2Deg - 90;
            float currentAngle = transform.rotation.eulerAngles.z;
            float newAngle = Mathf.SmoothDampAngle(currentAngle, targetAngle, ref rotationVelocity, rotationTime, Mathf.Infinity, Time.fixedDeltaTime);
            transform.rotation = Quaternion.Euler(0, 0, newAngle);
        }
        else if (Lefthit)
        {
            float targetAngle = Mathf.Atan2(Lefthit.normal.y, Lefthit.normal.x) * Mathf.Rad2Deg - 90;
            float currentAngle = transform.rotation.eulerAngles.z;
            float newAngle = Mathf.SmoothDampAngle(currentAngle, targetAngle,ref rotationVelocity, rotationTime, Mathf.Infinity, Time.fixedDeltaTime);
            transform.rotation = Quaternion.Euler(0, 0, newAngle);
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + offset, transform.position + offset + Vector3.down);
        Gizmos.DrawLine(transform.position - offset, transform.position - offset + Vector3.down);
    }
}
