using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="Player")
        {
            CheckpointMgr.Instance.PushCheckPoint(this.gameObject);
            Debug.Log("Checkpoint´¥·¢: " + this.gameObject.name);
            DialogManager.Instance.ShowDialogPanel(E_DialogFuncType.Type2);
        }
    }
}
