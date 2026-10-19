using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove2 : MonoBehaviour
{
    private static PlayerMove2 instance;
    public static PlayerMove2 Instance => instance;
    //移动速度
    public float moveSpeed = 2;

    //玩家是否可以移动
    public bool isMove = true;

    private CharacterController2D2 characterController;

    //玩家的动画状态机
    private Animator animator;

    private float move;
    private bool jump;
    private bool canMove;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        characterController = GetComponent<CharacterController2D2>();
        animator = GetComponent<Animator>();
        canMove = true;
        EventCenter.Instance.AddEventListener<bool>(E_EventType.DialogDoing, OnDialogDoing);
    }

    private void Update()
    {
        if(!canMove)
        {
            move = 0;
            jump = false;
            return;
        }
        move = Input.GetAxis("Horizontal") * moveSpeed;
        jump = Input.GetKey(KeyCode.Space);
        if (Input.GetKeyDown(KeyCode.F))
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.position = BornPointMgr.Instance.bornPoints[BornPointMgr.Instance.index].position;
            else
                this.transform.position = BornPointMgr.Instance.bornPoints[BornPointMgr.Instance.index].position;
        }
    }

    private void FixedUpdate()
    {
        if (isMove)
        {
            characterController.Move(move, jump);
        }
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
