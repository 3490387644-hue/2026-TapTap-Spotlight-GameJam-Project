using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove4 : MonoBehaviour
{
    //移动速度
    public float moveSpeed = 2;
    public Transform torchPos;
    private CharacterController2D4 characterController;
    //玩家的动画状态机
    private Animator animator;
    private bool canMove;
    private float move;
    private bool jump;
    private bool canGetTorchFromNPC = false; // 是否可以从NPC获取火把
    private NPC npc = null;
    public NPC GetNPC
    {
        get { return npc; }
        set { npc = value; }
    }
    public bool CanGetTorchFromNPC
    {
        get { return canGetTorchFromNPC; }
        set { canGetTorchFromNPC = value; }
    }
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("PlayerMove Start" + this.gameObject.transform.localScale);
        // GameSceneManager.Instance.SetPlayer(null); // 先设置为null，确保在场景切换时不会引用旧的玩家对象
        // GameSceneManager.Instance.SetPlayer(this.gameObject);
        characterController = GetComponent<CharacterController2D4>();
        animator = GetComponent<Animator>();
        canMove = true;
        EventCenter.Instance.AddEventListener<bool>(E_EventType.DialogDoing, OnDialogDoing);
    }

    private void Update()
    {
        if (!canMove)
        {
            move = 0;
            jump = false;
            return;
        }

        if (canGetTorchFromNPC && Input.GetKeyDown(KeyCode.E) && npc.canTalk)
        {
            canGetTorchFromNPC = false; // 获取火把后，设置为不能再获取
            // 播放拿火把的动画

            // 获取火把预制体放到相应的位置上
            GameObject torchObj = Instantiate(ResMgr.Instance.LoadRes<GameObject>("Prefabs/Torch/Torch", false));
            torchObj.transform.SetParent(torchPos);
            torchObj.transform.localPosition = Vector3.zero;
            // 更新NPC状态
            GameSceneManager.Instance.UpdateNPCState(npc.npcID);
        }

        move = Input.GetAxis("Horizontal") * moveSpeed;
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

    // void OnTriggerEnter2D(Collider2D collider)
    // {
    //     if(collider.gameObject.layer == LayerMask.NameToLayer("NPC"))
    //     {
    //         canGetTorchFromNPC = true; // 玩家进入NPC范围，可以获取火把
    //         npc = collider.GetComponent<NPC>(); // 获取当前交互的NPC ID
    //     }
    // }

    // void OnTriggerExit2D(Collider2D collider)
    // {
    //     if(collider.gameObject.layer == LayerMask.NameToLayer("NPC"))
    //     {
    //         canGetTorchFromNPC = false; // 玩家离开NPC范围，不能获取火把
    //         npc.npcID = -1; // 重置当前交互的NPC ID
    //     }
    // }
}
