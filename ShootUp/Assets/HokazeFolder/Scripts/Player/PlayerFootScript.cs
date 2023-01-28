using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*---------------------------------------
 �v���C���[�̑�(�n�ʂƂ̐ڐG)�̃v���O����
----------------------------------------*/

public class PlayerFootScript : MonoBehaviour
{
    // �X�N���v�g ***********************
    PlayerScript Pscript;

    private void Start()
    {
        Pscript = GameObject.Find("Player").GetComponent<PlayerScript>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground")) // ���ƐڐG
        {
            string groundSt = collision.gameObject.name;

            if (groundSt.Contains("Harf")) // ������
            {
                Pscript.jumpCnt = 0;
                if (Pscript.HfloorCheck)
                {
                    Pscript.Hfloor = collision.gameObject.GetComponent<Collider2D>();
                    Pscript.HfloorCheck = false;
                }
                Pscript.anim.SetBool("Jump", false);
                Pscript.anim.SetBool("Down", false);
            }
            else // ��
            {
                Pscript.jumpCnt = 0;
                Pscript.anim.SetBool("Jump", false);
                Pscript.anim.SetBool("Down", false);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            Pscript.downCheck = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {

            string groundSt = collision.gameObject.name;

            if (groundSt.Contains("Harf"))
            {
                Pscript.downCheck = false; // ���������痣�ꂽ
                Pscript.HfloorCheck = true;
            }
            else
            {
                Pscript.downCheck = false; // �����痣�ꂽ
            }
        }
    }
}
