using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 音乐音效管理器
/// </summary>
public class MusicMgr : BaseManager<MusicMgr>
{
    //背景音乐播放组件
    private AudioSource bkMusic = null;

    //背景音乐大小
    private float bkMusicValue = 0.5f;
    public float GetBkMusicValue
    {
        get{return bkMusicValue;}
    }

    //管理正在播放的音效
    private List<AudioSource> soundList = new List<AudioSource>();

    //音效音量大小
    private float soundValue = 0.5f;
    public float GetSoundValue
    {
        get{return soundValue;}
    }

    //音效是否在播放
    private bool soundIsPlay = true;

    //背景音乐预加载缓存 提前加载好背景音乐切片 避免切换音乐时卡顿
    private Dictionary<string, AudioClip> bkMusicCache = new Dictionary<string, AudioClip>();
    //正在预加载中的背景音乐 防止重复加载
    private HashSet<string> bkMusicLoading = new HashSet<string>();

    // 更新音乐和音效的值
    public void UpdateData()
    {
        // 音量始终从全局存档读取（globalGameData 缺省时默认为 0.5），
        // 不能拿 gameData1.hasSaveData 做门控，否则第一章无存档时音量无法恢复
        bkMusicValue = GameDataMgr.Instance.globalGameData.musicData.musicVolume;
        soundValue = GameDataMgr.Instance.globalGameData.musicData.soundVolume;
        EventCenter.Instance.EventTrigger<float>(E_EventType.SoundValueChanged, soundValue);
    }

    //播放背景音乐
    public void PlayBKMusic(string name)
    {
        //动态创建播放背景音乐的组件 并且 不会过场景移除
        //保证背景音乐在过场景时也能播放
        if(bkMusic==null)
        {
            GameObject obj=new GameObject();
            obj.name = "BKMusic";
            GameObject.DontDestroyOnLoad(obj);
            bkMusic = obj.AddComponent<AudioSource>();
        }

        //如果已经预加载好 直接切换 不会卡顿
        if(bkMusicCache.TryGetValue(name, out AudioClip cachedClip))
        {
            bkMusic.Stop();
            bkMusic.clip = cachedClip;
            bkMusic.loop = true;
            bkMusic.volume = bkMusicValue;
            bkMusic.Play();
            return;
        }

        //还没有预加载好 异步加载 加载完成后再切换（避免提前切断旧音乐造成卡顿/断音）
        //根据传入的背景音乐名字 来播放背景音乐
        // ABResMgr.Instance.LoadResAsync<AudioClip>("music", name, (clip) =>
        // {
        //     bkMusic.clip = clip;
        //     bkMusic.loop = true;
        //     bkMusic.volume = bkMusicValue;
        //     bkMusic.Play();
        // });
        // 因为用不到AB包 这边用ResMgr进行音乐和音效的加载
        ResMgr.Instance.LoadRes<AudioClip>(name, true, (clip)=>
        {
            if(clip == null)
                return;
            if(!bkMusicCache.ContainsKey(name))
                bkMusicCache[name] = clip;
            bkMusic.Stop();
            bkMusic.clip = clip;
            bkMusic.loop = true;
            bkMusic.volume = bkMusicValue;
            bkMusic.Play();
        });
    }

    /// <summary>
    /// 预加载背景音乐 在加载场景时提前调用 避免切换音乐时卡顿
    /// </summary>
    /// <param name="name">背景音乐名字</param>
    public void PreloadBKMusic(string name)
    {
        if(bkMusicCache.ContainsKey(name) || bkMusicLoading.Contains(name))
            return;
        bkMusicLoading.Add(name);
        ResMgr.Instance.LoadRes<AudioClip>(name, true, (clip)=>
        {
            bkMusicLoading.Remove(name);
            if(clip != null && !bkMusicCache.ContainsKey(name))
                bkMusicCache[name] = clip;
        });
    }

    //停止背景音乐
    public void StopBKMusic()
    {
        if(bkMusic==null)
            return;
        bkMusic.Stop();
    }

    //暂停背景音乐
    public void PauseBKMusic()
    {
        if (bkMusic == null)
            return;
        bkMusic.Pause();
    }

    //设置背景音乐大小
    public void ChangeBKMusicValue(float v)
    {
        bkMusicValue = v;
        if (bkMusic == null)
            return;
        bkMusic.volume = bkMusicValue;
    }

    public MusicMgr()
    {
        MonoMgr.Instance.AddFixedUpdateListener(MyUpdate);
    }

    private void MyUpdate()
    {
        if(!soundIsPlay)
            return;

        //不停的遍历容器 检测有没有音效播放完毕 播放完了 就移除销毁它
        //为了避免边遍历边移除出问题 我们采用逆向遍历
        for(int i=soundList.Count-1;i>=0;i--)
        {
            if (!soundList[i].isPlaying)
            {
                //音效播放完毕了 不再使用了 我们将这个音效切片置空
                soundList[i].clip = null;
                PoolMgr.Instance.PushObj(soundList[i].gameObject);
                soundList.RemoveAt(i);
            }
        }
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="name">音效名字</param>
    /// <param name="isLoop">是否循环</param>
    /// <param name="isSync">是否同步加载</param>
    /// <param name="callBack">加载结束后的回调</param>
    public void PlaySound(string name,bool isLoop=false,bool isAsync=false,UnityAction<AudioSource> callBack=null)
    {
        //加载音效资源 进行播放
        // ABResMgr.Instance.LoadResAsync<AudioClip>("sound", name, (clip) =>
        // {
        //     //从缓存池中取出音效对象得到对应组件
        //     AudioSource source=PoolMgr.Instance.GetObj("sound/soundObj").GetComponent<AudioSource>();
        //     //如果取出来的音效是之前正在使用的 我们先停止它
        //     source.Stop();
        //     source.clip = clip;
        //     source.loop = isLoop;
        //     source.volume = soundValue;
        //     source.Play();
        //     //存储容器 用于记录 方便之后判断是否停止
        //     //由于从缓存池中取出对象 有可能取出一个之前正在使用的（超上限时）
        //     //所以我们需要判断 容器中没有记录再去记录 不要重复去添加即可
        //     if(!soundList.Contains(source))
        //         soundList.Add(source);
        //     //传递给外部使用
        //     callBack?.Invoke(source);
        // },isSync);

        ResMgr.Instance.LoadRes<AudioClip>(name, isAsync, (clip)=>
        {
            if(clip == null)
                return; // 音频加载失败，直接返回避免空引用
            //从缓存池中取出音效对象得到对应组件
            AudioSource source=PoolMgr.Instance.GetObj("Prefabs/Sound/SoundAudioObj").GetComponent<AudioSource>();
            //如果取出来的音效是之前正在使用的 我们先停止它
            source.Stop();
            source.clip = clip;
            source.loop = isLoop;
            source.volume = soundValue;
            source.Play();
            //存储容器 用于记录 方便之后判断是否停止
            //由于从缓存池中取出对象 有可能取出一个之前正在使用的（超上限时）
            //所以我们需要判断 容器中没有记录再去记录 不要重复去添加即可
            if(!soundList.Contains(source))
                soundList.Add(source);
            //传递给外部使用
            callBack?.Invoke(source);
        });
    }

    /// <summary>
    /// 停止播放音效
    /// </summary>
    /// <param name="source">音效组件对象</param>
    public void StopSound(AudioSource source)
    {
        if(soundList.Contains(source))
        {
            //停止播放
            source.Stop();
            //从容器中移除
            soundList.Remove(source);
            //不用了 清空切片 避免占用
            source.clip = null;
            //放入缓存池
            PoolMgr.Instance.PushObj(source.gameObject);
        }
    }

    /// <summary>
    /// 改变音效大小
    /// </summary>
    /// <param name="v"></param>
    public void ChangeSoundValue(float v)
    {
        soundValue = v;
        for(int i=0;i<soundList.Count;i++)
        {
            soundList[i].volume = v;
        }
        EventCenter.Instance.EventTrigger<float>(E_EventType.SoundValueChanged, v);
    }

    /// <summary>
    /// 继续播放或者暂停所有音效
    /// </summary>
    /// <param name="isPlay">是否继续播放 true为播放 false为暂停</param>
    public void PlayOrPauseSound(bool isPlay)
    {
        if(isPlay)
        {
            soundIsPlay = true;
            for (int i = 0; i < soundList.Count; i++)
            {
                soundList[i].Play();
            }
        }
        else
        {
            soundIsPlay=false;
            for (int i = 0; i < soundList.Count; i++)
            {
                soundList[i].Pause();
            }
        }
    }

    /// <summary>
    /// 清空音效相关记录 过场景时在清空缓存池之前去调用它
    /// </summary>
    public void ClearSound()
    {
        for (int i = 0; i < soundList.Count; i++)
        {
            soundList[i].Stop();
            soundList[i].clip = null;
            PoolMgr.Instance.PushObj(soundList[i].gameObject);
        }
        //清空音效列表
        soundList.Clear();
    }
}
