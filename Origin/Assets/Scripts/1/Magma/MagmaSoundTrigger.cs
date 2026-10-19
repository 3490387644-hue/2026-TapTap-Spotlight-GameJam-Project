using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagmaSoundTrigger : MonoBehaviour
{
    private AudioSource magmaSound;
    private bool playerInside;
    private bool isLoading; // 音效异步加载中标记，防止重复触发产生孤儿音源

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        if (playerInside)
        {
            return;
        }

        playerInside = true;

        if (isLoading)
        {
            return; // 已在加载中，加载完成的回调会处理播放/停止
        }
        isLoading = true;

        GameSceneManager.Instance.PlaySoundByTriggerType(E_SoundTriggerType.Chapter1MagmaSound, true, true, OnMagmaSoundLoaded);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        playerInside = false;
        StopMagmaSound();
    }

    private void OnMagmaSoundLoaded(AudioSource source)
    {
        Debug.Log("MagmaSoundTrigger: Magma sound loaded and playing.");
        isLoading = false;
        magmaSound = source;

        // 如果声音加载完成前玩家已经离开触发范围
        if (!playerInside)
        {
            StopMagmaSound();
        }
    }

    private void StopMagmaSound()
    {
        if (magmaSound == null)
        {
            return;
        }

        MusicMgr.Instance.StopSound(magmaSound);
        magmaSound = null;
    }

    private void OnDestroy()
    {
        StopMagmaSound();
    }
}
