using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform player;//玩家对象的位置

    public float lX; //左边界位置
    public float lY; //右边界位置

    void Start()
    {

    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        float newX = player.position.x;
        newX = Mathf.Clamp(newX, lX, lY); //限制摄像机X轴的左右界限
        this.transform.position = new Vector3(newX, 0, -10);
    }
}
