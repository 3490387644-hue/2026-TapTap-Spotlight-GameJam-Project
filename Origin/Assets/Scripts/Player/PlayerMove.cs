using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    //ÒÆ¶¯ËÙ¶È
    public float moveSpeed = 5;

    private CharacterController2D characterController;

    private float move;
    private bool jump;
    // Start is called before the first frame update
    void Start()
    {
        characterController=GetComponent<CharacterController2D>();
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
