using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTrigger : MonoBehaviour
{
    public E_MusicTriggerType triggerType;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if(triggerType != GameSceneManager.Instance.currentMusicTriggerType)
            {
                GameSceneManager.Instance.PlayMusicByTriggerType(triggerType);
            }
        }
    }
}
