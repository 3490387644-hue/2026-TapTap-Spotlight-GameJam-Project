using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class ChangeRotation : MonoBehaviour
{
    private static ChangeRotation instance;
    public static ChangeRotation Instance=>instance;

    public float rayLength = 1; //射线长度
    public float rotationTime = 0.8f; //旋转时间
    public LayerMask mask; //射线检测层
    public Transform point1; //射线发射点1
    public Transform point2; //射线发射点2
    private RaycastHit2D hit1; //射线击中点1
    private RaycastHit2D hit2; //射线击中点2
    public Vector2 avg; //法线的平均向量
    private float rotationVelocity; //射线暂存量
    private Rigidbody2D rb;
    public bool isWall; //是否攀爬

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        hit1 = Physics2D.Raycast(point1.position, -transform.up, rayLength, mask);
        hit2 = Physics2D.Raycast(point2.position, -transform.up, rayLength, mask);
        avg=((hit1.normal+hit2.normal)/2).normalized;

        float angle = Vector2.Angle(hit1.normal.normalized, Vector2.up); //法线和世界坐标上方向的夹角
        if(angle>=0&&angle<=60||Input.GetKey(KeyCode.LeftShift))
        {
            isWall = true;
        }
        else
        {
            isWall = false;
            this.transform.rotation = Quaternion.identity;
        }

        if (hit1 && hit2 && isWall)
        {
            float targetAngle = Mathf.Atan2(hit1.normal.y, hit1.normal.x) * Mathf.Rad2Deg - 90;
            float currentAngle = transform.rotation.eulerAngles.z;
            float newAngle = Mathf.SmoothDampAngle(currentAngle, targetAngle, ref rotationVelocity, rotationTime, Mathf.Infinity, Time.fixedDeltaTime);
            transform.rotation = Quaternion.Euler(0, 0, newAngle);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(point1.position, point1.position - transform.up);
        Gizmos.DrawLine(point2.position, point2.position - transform.up);
        Gizmos.DrawLine((point1.position + point2.position)/2, (point1.position + point2.position) / 2 + -transform.up);
    }
}
