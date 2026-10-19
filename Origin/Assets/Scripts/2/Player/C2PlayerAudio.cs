using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class C2PlayerAudio : MonoBehaviour
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
            PathUtil.SoundPathOfChapter2 + "WalkSound",
            false
        );

        if (footstepClip == null)
        {
            Debug.LogError(
                "C1PlayerAudio: 无法加载脚步音效，请检查资源路径："
                + PathUtil.SoundPathOfChapter2
                + "WalkSound",
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
        GameSceneManager.Instance.PlaySoundByTriggerType(E_SoundTriggerType.Chapter2JumpLandingSound, false, true);
    }

    public void OnJumpSound()
    {
        // 发出跳跃音效
        Debug.Log("角色跳跃音效");
        GameSceneManager.Instance.PlaySoundByTriggerType(E_SoundTriggerType.Chapter2JumpSound, false, true);
    }

    public void OnClimbWallSound()
    {
        // 发出攀爬音效
        Debug.Log("角色攀爬音效");
        GameSceneManager.Instance.PlaySoundByTriggerType(E_SoundTriggerType.Chapter2ClimbingSound, false, true);
    }
}
