using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*---------------------------------------------
 * �����Ǘ�����X�N���v�g
---------------------------------------------*/

public class AudioClipScript : MonoBehaviour
{
    [HideInInspector] public AudioSource audioSource;
    [HideInInspector] public AudioClip[] SE;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        SE = Resources.LoadAll<AudioClip>("Audio");
    }
}
