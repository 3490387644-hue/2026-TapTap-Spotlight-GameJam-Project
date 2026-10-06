using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    //移动速度
    public float moveSpeed = 5;

    private CharacterController2D characterController;

    //玩家的动画状态机
    private Animator animator;

    private float move;
    private bool jump;
    // Start is called before the first frame update
    void Start()
    {
        characterController=GetComponent<CharacterController2D>();
        animator=GetComponent<Animator>();
    }

    private void Update()
    {
        move=Input.GetAxis("Horizontal")*moveSpeed;
        jump = Input.GetKey(KeyCode.W);
    }

    private void FixedUpdate()
    {
        characterController.Move(move, jump);
    }

    
}
