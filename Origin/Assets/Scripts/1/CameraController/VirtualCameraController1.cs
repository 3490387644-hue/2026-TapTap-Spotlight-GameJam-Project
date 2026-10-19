using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class VirtualCameraController1 : MonoBehaviour
{
    [Header("角色移动控制")]
    [Tooltip("勾选此项将在运镜开始时禁用角色移动，运镜结束后恢复")]
    public bool lockPlayerMovement = true;

    // 在这里拖入你的角色控制器组件（如 CharacterController, Rigidbody, 或自定义移动脚本）
    private PlayerMove playerMove;

    private PlayableDirector director;

    private bool isTriggered;

    private void Awake()
    {
        director = GetComponent<PlayableDirector>();
        if(GameSceneManager.Instance.GetTimelineData.isTriggered1)
        {
            director.playOnAwake = false;
            director.Stop();
        }
        else
            isTriggered = false;

        // 绑定事件
        EventCenter.Instance.AddEventListener<PlayerMove>(E_EventType.Timeline1Register, OnRegisterPlayer);
    }

    private void OnRegisterPlayer(PlayerMove player)
    {
        playerMove = player;
    }

    // 防止内存泄漏，务必解绑
    private void OnDestroy()
    {
        EventCenter.Instance.RemoveEventListener<PlayerMove>(E_EventType.Timeline1Register, OnRegisterPlayer);
    }

    public void LockPlayerMovement()
    {
        GameSceneManager.Instance.LockInput();
        if (lockPlayerMovement && playerMove != null)
        {
            playerMove.GetCanMove = false;
        }
    }

    public void UnlockPlayerMovement()
    {
        GameSceneManager.Instance.UnlockInput();
        if (lockPlayerMovement && playerMove != null)
        {
            playerMove.GetCanMove = true;
            GameSceneManager.Instance.UpdateTimeline1Info(true); // 更新第一章的Timeline触发状态为已触发
            DialogManager.Instance.ShowType1Dialog(0,2);
        }
    }
}
