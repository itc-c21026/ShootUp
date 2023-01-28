using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*-------------------------------------------------
 敵撃破時に生成されるスコアオブジェクトのプログラム
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
