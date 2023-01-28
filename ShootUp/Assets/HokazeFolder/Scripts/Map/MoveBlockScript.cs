using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*------------------------------------------
 �����u���b�N�̃v���O����
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

            // �u���b�N(�ړ��͈�(�ړ�����), �ړ����x, ��]���x)
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
