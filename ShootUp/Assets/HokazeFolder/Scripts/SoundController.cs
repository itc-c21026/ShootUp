using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/*------------------------------------------
 * サウンドスクリプト(インターフェース)
 -----------------------------------------*/

public class SoundController : MonoBehaviour
{
    private static SoundController instance;
    public static SoundController Instance
    {
        get { return instance; }
        private set { instance = value; }
    }

    AudioSource[] audioSources = new AudioSource[2];

    // Audio Mixerとそのグループ
    [SerializeField]
    AudioMixer audioMixer;
    [SerializeField]
    AudioMixerGroup musicMixerGroup;
    [SerializeField]
    AudioMixerGroup seMixerGroup;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // 既にSoundControllerが存在するなら破棄する
            Destroy(this);
            return;
        }

        audioSources = GetComponents<AudioSource>();
        foreach (AudioSource a in audioSources)
        {
            a.outputAudioMixerGroup = musicMixerGroup;
        }
    }


    [SerializeField]
    float fadeDuration = 1.0f;              // フェードに掛ける時間

    int currentIndex = 0;                   // 現在使っているAudioSourceの番号
    bool isPlayingMusic = false;            // 現在音楽を再生しているか

    public void PlayMusic(AudioClip nextMusic, bool looping = true)
    {
        if (isPlayingMusic)
        {
            // 現在流れている音楽を止め、次の音楽を再生する
            StartCoroutine(StopMusic(currentIndex));
            currentIndex = 1 - currentIndex; // 使うAudioSourceを入れ替える
            audioSources[currentIndex].clip = nextMusic;    // 次の音楽を設定
            audioSources[currentIndex].volume = 1.0f;       // 次の音楽のボリュームを1にする
            audioSources[currentIndex].Play();
        }
        else
        {
            currentIndex = 0;   // 0番目のAudioSourceを使う
            audioSources[currentIndex].clip = nextMusic;
            audioSources[currentIndex].volume = 1.0f;
            audioSources[currentIndex].Play();
        }

        isPlayingMusic = true;
    }

    // 指定されたIndexのAudioSourceを止める
    private IEnumerator StopMusic(int sourceIndex)
    {
        yield return new WaitForSeconds(fadeDuration);
        audioSources[sourceIndex].Stop();
    }

    // 音楽を完全に止める
    public void StopAllMusic()
    {
        StartCoroutine(StopMusic(0));
        StartCoroutine(StopMusic(1));

        isPlayingMusic = false;
    }

    // 効果音を再生する
    public void PlaySE(AudioClip se, float lifetime = 1.0f)
    {
        GameObject seObject = new GameObject();
        seObject.AddComponent<AudioSource>();

        // このゲームのみで使われるプログラム
        switch (se.name)
        {
            case "0pistol":
                seObject.GetComponent<AudioSource>().volume = 1.0f;
                break;

            case "1SR":
                seObject.GetComponent<AudioSource>().volume = 0.3f;
                break;

            case "2SG":
                seObject.GetComponent<AudioSource>().volume = 0.4f;
                break;

            case "3SMG":
                seObject.GetComponent<AudioSource>().volume = 0.4f;
                break;

            case "4B002-call":
                seObject.GetComponent<AudioSource>().volume = 0.3f;
                break;

            case "5B002-laser":
                seObject.GetComponent<AudioSource>().volume = 0.03f;
                break;

            case "6click":
                seObject.GetComponent<AudioSource>().volume = 0.3f;
                break;
        }
        //************************************

        seObject.GetComponent<AudioSource>().outputAudioMixerGroup = seMixerGroup;
        seObject.GetComponent<AudioSource>().PlayOneShot(se);

        // 一定時間後に消滅するようにする
        Destroy(seObject, lifetime);
    }
}
