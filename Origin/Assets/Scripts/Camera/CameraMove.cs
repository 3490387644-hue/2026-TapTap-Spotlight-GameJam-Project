using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform player;//玩家对象的位置

    private float lastPlayerPosition; //玩家上一次x轴的位置

    void Start()
    {
        lastPlayerPosition=player.position.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(this.transform.position.x<=185.5)
        {
            float xChange = player.position.x - lastPlayerPosition;
            this.transform.position += new Vector3(xChange, 0, 0);
            lastPlayerPosition = player.position.x;
        }
    }
}
