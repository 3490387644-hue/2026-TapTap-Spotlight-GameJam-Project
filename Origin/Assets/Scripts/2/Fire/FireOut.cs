using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireOut : MonoBehaviour
{
    public float w; //检测范围宽度
    public float h; //检测范围高度
    public float angle; //旋转角度
    public LayerMask mask; //检测层级
    public Vector3 offset; //检测中心相对原点的偏移位置
    public float eachTime; //火焰喷发间隔
    private float lastTime; //上次火焰喷发时间
    private bool hasTriggered; //是否触发火焰喷发
    private Animator animator;

    void Start()
    {
        hasTriggered = GameDataMgr.Instance.sceneObjectCollisionDialogData[12].HasTriggered; //读取触发状态，避免重复触发
        EventCenter.Instance.AddEventListener<bool>(E_EventType.FireOutTrigger, OnFireOutTrigger);
        animator = GetComponent<Animator>();
        lastTime = Time.time; //记录上次火焰喷发时间
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastTime >= eachTime)
        {
            animator.SetTrigger("isFire");
            lastTime = Time.time; //记录上次火焰喷发时间
        }
    }

    public void FireCheck()
    {
        StartCoroutine(myCoroutine());
    }

    public void OnFireOut()
    {
        //只处理玩家触发
    }

    IEnumerator myCoroutine()
    {
        Collider2D collider = Physics2D.OverlapBox(
            this.transform.position + offset, new Vector2(w, h), angle, mask);

        //只处理玩家触发
        if (collider != null && collider.gameObject.CompareTag("Player"))
        {
            DeadChange.Instance.DarkChange();
            yield return new WaitForSeconds(2);
            print("检测到玩家");

            if (!hasTriggered)
            {
                EventCenter.Instance.EventTrigger<bool>(E_EventType.FireOutTrigger, true);
                DialogManager.Instance.ShowType2Dialog(E_DialogDataType.SceneObjectCollisionText, 12);
            }

            Rigidbody2D rb = collider.gameObject.GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.position = BornPointMgr.Instance.bornPoints[BornPointMgr.Instance.index].position;
            else
                collider.gameObject.transform.position = BornPointMgr.Instance.bornPoints[BornPointMgr.Instance.index].position;
            if (collider.gameObject.transform.localScale !=
                BornPointMgr.Instance.bornPoints[BornPointMgr.Instance.index].localScale)
            {
                collider.gameObject.GetComponent<CharacterController2D2>().m_FacingRight =
                    !collider.gameObject.GetComponent<CharacterController2D2>().m_FacingRight;
            }
            collider.gameObject.transform.localScale =
                BornPointMgr.Instance.bornPoints[BornPointMgr.Instance.index].localScale;
        }
    }

    void OnDestroy()
    {
        EventCenter.Instance.RemoveEventListener<bool>(E_EventType.FireOutTrigger, OnFireOutTrigger);
    }

    private void OnFireOutTrigger(bool isTriggered)
    {
        hasTriggered = isTriggered;
        GameDataMgr.Instance.sceneObjectCollisionDialogData[12].HasTriggered = true; //记录触发状态，避免重复触发
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Matrix4x4 oldMatrix = Gizmos.matrix;
        Gizmos.matrix = Matrix4x4.TRS(transform.position + offset, Quaternion.Euler(0, 0, angle), Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(w, h, 0));
        Gizmos.matrix = oldMatrix;
    }
}
