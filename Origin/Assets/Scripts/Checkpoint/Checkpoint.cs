using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    //��һ�ξ�����ʱ��Ż�����Ϊ�µ�������
    int count = 1;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="Player"&&count>0)
        {
            CheckpointMgr.Instance.PushCheckPoint(this.gameObject);
            count--;
            Debug.Log("Checkpoint����: " + this.gameObject.name);
            DialogManager.Instance.ShowDialogPanel(E_DialogFuncType.Type2);
        }
    }
}
