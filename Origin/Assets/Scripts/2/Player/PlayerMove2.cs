using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove2 : MonoBehaviour
{
    //移动速度
    public float moveSpeed = 2;

    private CharacterController2D2 characterController;

    //玩家的动画状态机
    private Animator animator;

    private float move;
    private bool jump;
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController2D2>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        move = Input.GetAxis("Horizontal") * moveSpeed;
        jump = Input.GetKey(KeyCode.Space);
    }

    private void FixedUpdate()
    {
        characterController.Move(move, jump);
    }
}
