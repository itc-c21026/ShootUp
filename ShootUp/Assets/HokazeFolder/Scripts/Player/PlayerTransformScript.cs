using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*-------------------------------------
 �v���C���[�ɍ��W�����킹��v���O����
-------------------------------------*/

public class PlayerTransformScript : MonoBehaviour
{
    Vector3 PlayerPos;

    private void Start()
    {
        PlayerPos = GameObject.Find("Player").transform.position;
    }

    private void Update()
    {
        this.transform.localPosition = PlayerPos;
    }
}
