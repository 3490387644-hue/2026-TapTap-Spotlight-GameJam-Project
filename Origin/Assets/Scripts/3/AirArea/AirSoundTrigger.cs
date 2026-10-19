using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirSoundTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameSceneManager.Instance.PlaySoundByTriggerType(E_SoundTriggerType.Chapter3AirflowSound, false, true);
        }
    }
}
