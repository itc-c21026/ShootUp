using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*-----------------------------------------------------
 �Q�[���R���g���[���X�N���v�g
-----------------------------------------------------*/

public class GameController : MonoBehaviour
{
    AudioClipScript ACSC;

    bool a = false;

    int audioBorder;
    private void Awake()
    {
        Cursor.visible = false;
        ACSC = GameObject.Find("Audio").GetComponent<AudioClipScript>();
    }

    private void Update()
    {
        // �Q�[���X�s�[�h�ύX
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!a)
            {
                Time.timeScale = 0.5f;
                a = !a;
            }
            else
            {
                Time.timeScale = 1.0f;
                a = !a;
            }
        }

        // �J�[�\�����Q�[����ʂɂ��邩UI��ʂɂ��邩��SE��ς���
        Vector3 mousePos = Input.mousePosition;
        float border = Screen.width / 3 * 2;

        if (mousePos.x >= border)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
                SoundController.Instance.PlaySE(ACSC.SE[6]);
        }
    }
}
