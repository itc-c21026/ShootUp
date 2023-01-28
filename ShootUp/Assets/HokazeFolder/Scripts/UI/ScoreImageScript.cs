using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*------------------------------------------
 * ƒXƒRƒA‰æ‘œŠÇ—
 -----------------------------------------*/

public class ScoreImageScript : MonoBehaviour
{
    [HideInInspector] public CanvasRenderer[] NumberCR;
    [HideInInspector] public int nowNumber;
    int oldNumber;

    private void Awake()
    {
        NumberCR = new CanvasRenderer[10];
        for (int i = 0; i < 10; i++)
        {
            NumberCR[i] = this.transform.GetChild(i).gameObject.GetComponent<CanvasRenderer>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        AllClear();
        oldNumber = nowNumber;
        nowNunbers();
    }

    // Update is called once per frame
    void Update()
    {
        if (oldNumber != nowNumber)
        {
            nowNunbers();
            oldNumber = nowNumber;
        }

        if (nowNumber >= 10 || nowNumber < 0)
        {
            AllClear();
        }
    }

    void AllClear()
    {
        foreach (CanvasRenderer nowCR in NumberCR)
        {
            nowCR.SetAlpha(0);
        }
    }

    void nowNunbers()
    {
        NumberCR[oldNumber].SetAlpha(0);
        NumberCR[nowNumber].SetAlpha(1);
    }
}
