using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*---------------------------------------
 カメラ全般のスクリプト
---------------------------------------*/

public class CameraScript : MonoBehaviour
{
    // スクリプト ***********************
    GunController Gunscript;
    UIScript UIsc;
    PlayerScript Pscript;
    MapScript MapSC;

    // オブジェクト *********************
    GameObject Player;

    // Vector3 **************************
    Vector3 defaultPos = new Vector3(0, 4.2f, -10);

    // bool *****************************
    [HideInInspector] public bool sniperAimCheck = false;

    private void Start()
    {
        Player = GameObject.Find("Player");
        Pscript = Player.GetComponent<PlayerScript>();

        Gunscript = GameObject.Find("Gun").GetComponent<GunController>();

        GameObject obj = GameObject.Find("GC");
        UIsc = obj.GetComponent<UIScript>();
        MapSC = obj.GetComponent<MapScript>();
    }

    private void Update()
    {
        // ゲーム表示画面とUI画面で照準を変える(照準を変える座標[境目])
        AimImageChange();

        if (Player != null)
        {
            if (Pscript.pause && !Pscript.dead)
                // スナイパーライフル所持時の画面操作(エイム最大距離補正[数値が高いほど距離が小さくなる])
                SniperAim(120f);
        }

        if (Gunscript.MainWeapon == "Sniper")
        // スナイパーライフルのエイム距離補正オンオフ
        if (Input.GetKeyDown(KeyCode.Q)) 
            sniperAimCheck = !sniperAimCheck;
    }

    void AimImageChange()
    {
        Vector3 mousePos = Input.mousePosition;
        float border = Screen.width / 3 * 2;

        // 照準(カーソル)の座標調整
        if (mousePos.x < border) // x座標1280f未満
        {
            UIsc.AimObj.SetActive(true);
            UIsc.AimObj.transform.position = mousePos;
            UIsc.AimObj2.SetActive(false);
            Gunscript.GunShotCheck = false;
        }
        else // x座標1280f以上
        {
            UIsc.AimObj2.SetActive(true);
            UIsc.AimObj2.transform.position = mousePos;
            UIsc.AimObj.SetActive(false);
            Gunscript.GunShotCheck = true;
        }
    }

    void SniperAim(float AimRangeMax)
    {
        Vector3 mousePos = Input.mousePosition;
        if (!MapSC.cameraBool)
        {
            if (mousePos.y > 540) // 視点の座標を上げる ※スナイパーライフル所持の時のみ
            {
                if (Gunscript.MainWeapon == "Sniper")
                {
                    if (sniperAimCheck) // オン
                    {
                        float pos = mousePos.y - 540;

                        this.transform.position = new Vector3(0, Player.transform.position.y + pos / AimRangeMax, -10);
                    }
                    else // オフ
                         //　プレイヤー中心
                        this.transform.position = new Vector3(0, Player.transform.position.y, -10);
                }
                else
                    //　プレイヤー中心
                    this.transform.position = new Vector3(0, Player.transform.position.y, -10);

                if (this.transform.position.y < 4.2) this.transform.position = defaultPos;
            }
            else if (mousePos.y < 540) // 視点の座標を下げる ※スナイパーライフル所持の時のみ
            {
                if (Gunscript.MainWeapon == "Sniper")
                {
                    if (sniperAimCheck) // オン
                    {
                        float pos = mousePos.y - 540;

                        this.transform.position = new Vector3(0, Player.transform.position.y + pos / AimRangeMax, -10);
                    }
                    else // オフ
                         //　プレイヤー中心
                        this.transform.position = new Vector3(0, Player.transform.position.y, -10);
                }
                else
                    //　プレイヤー中心
                    this.transform.position = new Vector3(0, Player.transform.position.y, -10);

                // 地面より下には視点が動かない
                if (this.transform.position.y < 4.2) this.transform.position = defaultPos;
            }
            else // 視点を中心プレイヤーに戻す
            {
                this.transform.position = new Vector3(0, Player.transform.position.y, -10);

                // 地面より下には視点が動かない
                if (this.transform.position.y < 4.2) this.transform.position = defaultPos;
            }
        }
    }
}
