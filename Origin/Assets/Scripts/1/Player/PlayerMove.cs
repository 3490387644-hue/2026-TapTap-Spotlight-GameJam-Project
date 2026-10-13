using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    //移动速度
    public float moveSpeed = 2;

    private CharacterController2D characterController;

    //玩家的动画状态机
    private Animator animator;
    private bool canMove;
    private float move;
    private bool jump;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("PlayerMove Start" + this.gameObject.transform.localScale);
        GameSceneManager.Instance.SetPlayer(null); // 先设置为null，确保在场景切换时不会引用旧的玩家对象
        GameSceneManager.Instance.SetPlayer(this.gameObject);
        characterController=GetComponent<CharacterController2D>();
        animator=GetComponent<Animator>();
        canMove = true;
        EventCenter.Instance.AddEventListener<bool>(E_EventType.DialogDoing, OnDialogDoing);
    }

    private void Update()
    {
        if(!canMove)
        {
            return;
        }
        move=Input.GetAxis("Horizontal")*moveSpeed;
        jump = Input.GetKey(KeyCode.W);
    }

    private void FixedUpdate()
    {
        characterController.Move(move, jump);
    }

    void OnDestroy()
    {
        EventCenter.Instance.RemoveEventListener<bool>(E_EventType.DialogDoing, OnDialogDoing);
    }

    private void OnDialogDoing(bool canMove)
    {
        this.canMove = canMove;
    }
}
