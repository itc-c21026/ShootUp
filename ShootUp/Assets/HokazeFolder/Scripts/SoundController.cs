using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/*------------------------------------------
 * �T�E���h�X�N���v�g(�C���^�[�t�F�[�X)
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

    // Audio Mixer�Ƃ��̃O���[�v
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
            // ����SoundController�����݂���Ȃ�j������
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
    float fadeDuration = 1.0f;              // �t�F�[�h�Ɋ|���鎞��

    int currentIndex = 0;                   // ���ݎg���Ă���AudioSource�̔ԍ�
    bool isPlayingMusic = false;            // ���݉��y���Đ����Ă��邩

    public void PlayMusic(AudioClip nextMusic, bool looping = true)
    {
        if (isPlayingMusic)
        {
            // ���ݗ���Ă��鉹�y���~�߁A���̉��y���Đ�����
            StartCoroutine(StopMusic(currentIndex));
            currentIndex = 1 - currentIndex; // �g��AudioSource�����ւ���
            audioSources[currentIndex].clip = nextMusic;    // ���̉��y��ݒ�
            audioSources[currentIndex].volume = 1.0f;       // ���̉��y�̃{�����[����1�ɂ���
            audioSources[currentIndex].Play();
        }
        else
        {
            currentIndex = 0;   // 0�Ԗڂ�AudioSource���g��
            audioSources[currentIndex].clip = nextMusic;
            audioSources[currentIndex].volume = 1.0f;
            audioSources[currentIndex].Play();
        }

        isPlayingMusic = true;
    }

    // �w�肳�ꂽIndex��AudioSource���~�߂�
    private IEnumerator StopMusic(int sourceIndex)
    {
        yield return new WaitForSeconds(fadeDuration);
        audioSources[sourceIndex].Stop();
    }

    // ���y�����S�Ɏ~�߂�
    public void StopAllMusic()
    {
        StartCoroutine(StopMusic(0));
        StartCoroutine(StopMusic(1));

        isPlayingMusic = false;
    }

    // ���ʉ����Đ�����
    public void PlaySE(AudioClip se, float lifetime = 1.0f)
    {
        GameObject seObject = new GameObject();
        seObject.AddComponent<AudioSource>();

        // ���̃Q�[���݂̂Ŏg����v���O����
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

        // ��莞�Ԍ�ɏ��ł���悤�ɂ���
        Destroy(seObject, lifetime);
    }
}
