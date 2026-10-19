using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonSound : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    private Button button;
    void Awake()
    {
        button = GetComponent<Button>();
        audioSource = gameObject.AddComponent<AudioSource>();
        if(audioSource == null)
        {
            audioSource = gameObject.GetComponent<AudioSource>();
        }
        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.volume = MusicMgr.Instance.GetSoundValue;
        ResMgr.Instance.LoadRes<AudioClip>(PathUtil.SoundPathOfChapter1 + "ClearTappingSound", true, (clip)=>
        {
            audioSource.clip = clip;
            button.onClick.AddListener(() =>
            {
                audioSource.Play();
                Debug.Log("按钮触发音效播放：" + audioSource.volume);
            });
        });

        EventCenter.Instance.AddEventListener<float>(E_EventType.SoundValueChanged, OnSoundValueChanged);
    }

    void OnDestroy()
    {
        EventCenter.Instance.RemoveEventListener<float>(E_EventType.SoundValueChanged, OnSoundValueChanged);
    }

    private void OnSoundValueChanged(float value)
    {
        audioSource.volume = value;
    }
}
