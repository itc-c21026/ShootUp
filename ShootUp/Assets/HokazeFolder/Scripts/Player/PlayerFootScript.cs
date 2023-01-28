using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*---------------------------------------
 プレイヤーの足(地面との接触)のプログラム
----------------------------------------*/

public class PlayerFootScript : MonoBehaviour
{
    // スクリプト ***********************
    PlayerScript Pscript;

    private void Start()
    {
        Pscript = GameObject.Find("Player").GetComponent<PlayerScript>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground")) // 床と接触
        {
            string groundSt = collision.gameObject.name;

            if (groundSt.Contains("Harf")) // 薄い床
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
            else // 床
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
                Pscript.downCheck = false; // 薄い床から離れた
                Pscript.HfloorCheck = true;
            }
            else
            {
                Pscript.downCheck = false; // 床から離れた
            }
        }
    }
}
