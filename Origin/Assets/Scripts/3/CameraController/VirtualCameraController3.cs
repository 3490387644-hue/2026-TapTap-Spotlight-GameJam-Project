using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class VirtualCameraController3 : MonoBehaviour
{
    [Header("角色移动控制")]
    [Tooltip("勾选此项将在运镜开始时禁用角色移动，运镜结束后恢复")]
    public bool lockPlayerMovement = true;

    // 在这里拖入你的角色控制器组件（如 CharacterController, Rigidbody, 或自定义移动脚本）
    private PlayerMove3 playerMove;

    private PlayableDirector director;

    private bool isTriggered;

    private void Awake()
    {
        director = GetComponent<PlayableDirector>();
        if (GameSceneManager.Instance.GetTimelineData.isTriggered3)
        {
            director.playOnAwake = false;
            director.Stop();
        }
        else
            isTriggered = false;

        // 绑定事件
        EventCenter.Instance.AddEventListener<PlayerMove3>(E_EventType.Timeline3Register, OnRegisterPlayer);
    }

    private void OnRegisterPlayer(PlayerMove3 player)
    {
        playerMove = player;
    }

    // 防止内存泄漏，务必解绑
    private void OnDestroy()
    {
        EventCenter.Instance.RemoveEventListener<PlayerMove3>(E_EventType.Timeline3Register, OnRegisterPlayer);
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
            GameSceneManager.Instance.UpdateTimeline3Info(true); // 更新第一章的Timeline触发状态为已触发
            DialogManager.Instance.ShowType1Dialog(4);
        }
    }
}
