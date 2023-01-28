using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*------------------------------------------
 動くブロックのプログラム
------------------------------------------*/

public class MoveBlockScript : MonoBehaviour
{
    float TransformTime;

    PlayerScript Pscript;

    private void Start()
    {
        Pscript = GameObject.Find("Player").GetComponent<PlayerScript>();
    }

    void Update()
    {
        if (Pscript.pause)
        {
            TransformTime += Time.deltaTime;

            // ブロック(移動範囲(移動時間), 移動速度, 回転速度)
            MoveBlock(4.0f, 0.1f, 0.5f);
        }
    }

    void MoveBlock(float MoveWidth, float MoveSpeed, float RotateSpeed)
    {
        if (this.gameObject.name == "103MoveIBlock(Clone)")
        {
            if (TransformTime % MoveWidth * 2 <= MoveWidth)
                this.transform.position += new Vector3(MoveSpeed, 0, 0);
            else
                this.transform.position += new Vector3(-MoveSpeed, 0, 0);
        }
        else if (this.gameObject.name == "102RotationCrossBlock(Clone)")
        {
            this.gameObject.transform.localEulerAngles += new Vector3(0, 0, RotateSpeed);
        }
        else
        {
            if (TransformTime % MoveWidth * 2 <= MoveWidth)
                this.transform.position += new Vector3(0, MoveSpeed, 0);
            else
                this.transform.position += new Vector3(0, -MoveSpeed, 0);
        }
    }
}
