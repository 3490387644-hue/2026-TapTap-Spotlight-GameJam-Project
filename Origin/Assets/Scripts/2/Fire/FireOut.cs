using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireOut : MonoBehaviour
{
    public float w; //检测范围的宽
    public float h; //检测范围的高
    public float angle; //检测角度
    public LayerMask mask; //范围检测层
    public Vector3 offset; //检测中心点偏移原点的位置
    public float eachTime; //火焰喷发时间间隔
    private float lastTime; //上次喷发时间
    private bool hasTriggered; //是否触发喷发
    private Animator animator;

    void Start()
    {
        hasTriggered = GameDataMgr.Instance.sceneObjectCollisionDialogData[12].HasTriggered; //读取触发状态，防止重复触发
        EventCenter.Instance.AddEventListener<bool>(E_EventType.FireOutTrigger, OnFireOutTrigger);
        animator = GetComponent<Animator>();
        lastTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastTime >= eachTime)
        {
            animator.SetTrigger("isFire");
            lastTime = Time.time; //重置上次喷发时间
        }
    }

    public void FireCheck()
    {
        StartCoroutine(myCoroutine());
    }

    public void OnFireOut()
    {
        
    }

    IEnumerator myCoroutine()
    {
        Collider2D collider = Physics2D.OverlapBox(
            this.transform.position + offset, new Vector2(w, h), angle, mask);

        // 只有玩家碰到火才触发
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

            collider.gameObject.transform.position =
                BornPointMgr.Instance.bornPoints[BornPointMgr.Instance.index].position;
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
        GameDataMgr.Instance.sceneObjectCollisionDialogData[12].HasTriggered = true; // 写回触发状态，防止重复触发
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
