using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineSound : MonoBehaviour
{
    public void OnPlayVineBreakSound()
    {
        GameSceneManager.Instance.PlaySoundByTriggerType(E_SoundTriggerType.Chapter3VineBreakSound, false, true);
    }
}
