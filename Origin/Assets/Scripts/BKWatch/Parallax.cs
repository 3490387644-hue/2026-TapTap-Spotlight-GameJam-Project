using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private Transform _mainCamera;  //主相机

    private Vector3 _lastPosition; //上一帧相机位置

    [SerializeField] private float parallaxSpeed = 1f; //视差效果参数

    private void Start()
    {
        _mainCamera=Camera.main.transform;
        _lastPosition=_mainCamera.position;
    }

    private void LateUpdate()
    {
        ParallaxMove();
    }

    //视差移动函数
    private void ParallaxMove()
    {
        float deltaX=_mainCamera.position.x - _lastPosition.x; //deltaX:相机在过去一帧移动的距离
        transform.position += new Vector3(deltaX * parallaxSpeed, 0, 0);//更新背景位置
        _lastPosition = _mainCamera.position; //更新
    }
}
