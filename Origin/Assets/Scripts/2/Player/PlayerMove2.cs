using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove2 : MonoBehaviour
{
    private static PlayerMove2 instance;
    public static PlayerMove2 Instance=>instance;
    //移动速度
    public float moveSpeed = 2;

    //玩家是否可以移动
    public bool isMove=true;

    private CharacterController2D2 characterController;

    //玩家的动画状态机
    private Animator animator;

    private float move;
    private bool jump;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        characterController = GetComponent<CharacterController2D2>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        move = Input.GetAxis("Horizontal") * moveSpeed;
        jump = Input.GetKey(KeyCode.Space);
        if(Input.GetKeyDown(KeyCode.F))
        {
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
}
