using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*-----------------------------------------
 プレイヤーHP表示管理スクリプト
-----------------------------------------*/

public class LifeScript : MonoBehaviour
{
    // スクリプト *************************
    PlayerScript Pscript;

    // オブジェクト ***********************
    GameObject[] LifeObj;

    // CanvasRenderer *********************
    CanvasRenderer[] LifeCRs;
    CanvasRenderer[] LifeCRs2;
    CanvasRenderer[] lifebackCRs;

    private void Start()
    {
        Pscript = GameObject.Find("Player").GetComponent<PlayerScript>();

        LifeObj = Resources.LoadAll<GameObject>("LifeImage");

        // ライフImage生成(数値1 = ライフ0.5)
        LifeInst(Pscript.HP);

        AllClear();
        AllClearB();
        LifeSet();
        LifeSetB();
        AllClear2();
    }

    public void LifeSet() // 現在のHPを表示させる
    {
        for (int i = 0; i < Pscript.HP; i++)
        {
            LifeCRs[i].SetAlpha(1);
        }
    }

    public void AllClear() // ライフ全て非表示
    {
        foreach (CanvasRenderer nowCR in LifeCRs)
        {
            nowCR.SetAlpha(0);
        }
    }

    public void LifeSet2() // 現在のHPを表示させる
    {
        for (int i = 0; i < Pscript.HP; i++)
        {
            LifeCRs2[i].SetAlpha(1);
        }
    }

    public void AllClear2() // ライフ全て非表示
    {
        foreach (CanvasRenderer nowCR in LifeCRs2)
        {
            nowCR.SetAlpha(0);
        }
    }

    public void LifeSetB() // HPのBackImageを表示
    {
        for (int i = 0; i < Pscript.HP; i++)
        {
            lifebackCRs[i].SetAlpha(1);
        }
    }

    public void AllClearB() // HPのBackImageを全て非表示
    {
        foreach (CanvasRenderer CRs in lifebackCRs)
        {
            CRs.SetAlpha(0);
        }
    }

    void LifeInst(int LifeMax)
    {
        LifeCRs = new CanvasRenderer[Pscript.HP];
        LifeCRs2 = new CanvasRenderer[Pscript.HP];
        lifebackCRs = new CanvasRenderer[Pscript.HP];

        GameObject life;
        int posX = 0;

        if (Screen.width > 1000) posX = 70;
        else posX = 40;

        for (int i = 0; i < LifeMax; i++)
        {
            int objNumber = 0;
            if (i % 2 == 1) objNumber = 1;

            if (objNumber == 0)
            {
                // ライフ(左半分)
                if (Screen.width > 1300)
                {
                    life = Instantiate(LifeObj[0], new Vector3(posX, 1000, 0), Quaternion.identity);
                    life.transform.parent = GameObject.Find("Life").transform;
                    LifeCRs[i] = life.GetComponent<CanvasRenderer>();
                    LifeCRs2[i] = life.transform.GetChild(0).gameObject.GetComponent<CanvasRenderer>();
                }
                else
                {
                    life = Instantiate(LifeObj[0], new Vector3(posX, 665, 0), Quaternion.identity);
                    life.transform.parent = GameObject.Find("Life").transform;
                    LifeCRs[i] = life.GetComponent<CanvasRenderer>();
                    LifeCRs2[i] = life.transform.GetChild(0).gameObject.GetComponent<CanvasRenderer>();
                    life.transform.localScale = new Vector3(1,1,1);
                }

                // ライフ背景(左半分)
                if (Screen.width > 1300)
                {
                    life = Instantiate(LifeObj[2], new Vector3(posX, 1000, 0), Quaternion.identity);
                    life.transform.parent = GameObject.Find("LifeB").transform;
                    lifebackCRs[i] = life.GetComponent<CanvasRenderer>();
                }
                else
                {
                    life = Instantiate(LifeObj[2], new Vector3(posX, 665, 0), Quaternion.identity);
                    life.transform.parent = GameObject.Find("LifeB").transform;
                    lifebackCRs[i] = life.GetComponent<CanvasRenderer>();
                    life.transform.localScale = new Vector3(1, 1, 1);
                }
            }
            else
            {
                if (Screen.width > 1300)
                {
                    // ライフ(右半分)
                    life = Instantiate(LifeObj[1], new Vector3(posX, 1000, 0), Quaternion.identity);
                    life.transform.parent = GameObject.Find("Life").transform;
                    LifeCRs[i] = life.GetComponent<CanvasRenderer>();
                    LifeCRs2[i] = life.transform.GetChild(0).gameObject.GetComponent<CanvasRenderer>();
                }
                else
                {
                    life = Instantiate(LifeObj[1], new Vector3(posX, 665, 0), Quaternion.identity);
                    life.transform.parent = GameObject.Find("Life").transform;
                    LifeCRs[i] = life.GetComponent<CanvasRenderer>();
                    LifeCRs2[i] = life.transform.GetChild(0).gameObject.GetComponent<CanvasRenderer>();
                    life.transform.localScale = new Vector3(1, 1, 1);
                }

                if (Screen.width > 1300)
                {
                    // ライフ背景(右半分)
                    life = Instantiate(LifeObj[3], new Vector3(posX, 1000, 0), Quaternion.identity);
                    life.transform.parent = GameObject.Find("LifeB").transform;
                    lifebackCRs[i] = life.GetComponent<CanvasRenderer>();
                }
                else
                {
                    life = Instantiate(LifeObj[3], new Vector3(posX, 665, 0), Quaternion.identity);
                    life.transform.parent = GameObject.Find("LifeB").transform;
                    lifebackCRs[i] = life.GetComponent<CanvasRenderer>();
                    life.transform.localScale = new Vector3(1, 1, 1);
                }

                if (Screen.width > 1300) posX += 80;
                else posX += 60;
            }
        }
    }
}
