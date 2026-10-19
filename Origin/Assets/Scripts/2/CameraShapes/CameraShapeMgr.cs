using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShapeMgr : MonoBehaviour
{
    private static CameraShapeMgr instance;
    public static CameraShapeMgr Instance => instance;

    public List<PolygonCollider2D> cameraShapes; //记录的边界范围对象

    public CinemachineConfiner2D confiner;

    private int index; //当前记录的边界范围

    private void Awake()
    {
        instance = this;
        int saved = GameSceneManager.Instance.GetCameraShapeIndex;      // 0/1/2
        saved = Mathf.Clamp(saved, 0, cameraShapes.Count - 1);
        index = Mathf.Min(saved + 1, cameraShapes.Count - 1);          // 下一个要切换的索引
        confiner.m_BoundingShape2D = cameraShapes[saved];              // 当前显示第 saved 个边界
        confiner.InvalidateCache();
    }

    public void UpdateCameraShape()
    {
        Debug.Log("CameraShapeMgr:更新相机边界范围");
        confiner.m_BoundingShape2D = cameraShapes[index];
        confiner.InvalidateCache();
        GameSceneManager.Instance.UpdateCameraShapeIndex(index); //更新GameSceneManager中的相机边界范围索引
        index = (index + 1) < cameraShapes.Count ? index + 1 : cameraShapes.Count - 1; //防止越界
    }
}
