using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*-------------------------------------------------
 �G���j���ɐ��������X�R�A�I�u�W�F�N�g�̃v���O����
-------------------------------------------------*/

public class EnemyScoreMoveScript : MonoBehaviour
{
    float GoalPosY;
    Vector3 ThisPos;

    private void Start()
    {
        ThisPos = this.transform.position;

        GoalPosY = ThisPos.y + 120;
    }

    private void Update()
    {
        if (GoalPosY > ThisPos.y)
        {
            ThisPos.y += 0.8f;

            this.transform.position = ThisPos;
        }
    }
}
