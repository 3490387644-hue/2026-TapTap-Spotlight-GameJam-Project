using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShapeMgr : MonoBehaviour
{
    private static CameraShapeMgr instance;
    public static CameraShapeMgr Instance=>instance;

    public List<PolygonCollider2D> cameraShapes; //记录的边界范围对象

    public CinemachineConfiner2D confiner;

    private int index; //当前记录的边界范围

    private void Awake()
    {
        instance = this;
    }

    public void UpdateCameraShape()
    {
        confiner.m_BoundingShape2D=cameraShapes[index];
        index++;
    }
}
