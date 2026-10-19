using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class C3PlayerAudio : MonoBehaviour
{
    [SerializeField]
    private float footstepVolume = 0.7f;

    private AudioSource footstepSound;
    private AudioClip footstepClip;

    private void Awake()
    {
        footstepSound = GetComponent<AudioSource>();
        footstepSound.playOnAwake = false;
        footstepSound.loop = false;

        footstepClip = ResMgr.Instance.LoadRes<AudioClip>(
            PathUtil.SoundPathOfChapter3 + "Walk1",
            false
        );

        if (footstepClip == null)
        {
            Debug.LogError(
                "C1PlayerAudio: 无法加载脚步音效，请检查资源路径："
                + PathUtil.SoundPathOfChapter3
                + "Walk1",
                this
            );
        }
    }

    public void OnFootstepSound()
    {
        if (footstepSound == null || footstepClip == null)
        {
            return;
        }

        footstepSound.PlayOneShot(
            footstepClip,
            footstepVolume
        );
    }

    public void OnLandSound()
    {
        Debug.Log("角色落地音效");
        GameSceneManager.Instance.PlaySoundByTriggerType(E_SoundTriggerType.Chapter3FlyLandingSound, false, true);
    }

    public void OnFlySound()
    {
        // 发出跳跃音效
        Debug.Log("角色飞翔音效");
        GameSceneManager.Instance.PlaySoundByTriggerType(E_SoundTriggerType.Chapter3FlySound, false, true);
    }
}
