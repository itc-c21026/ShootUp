using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*-------------------------------------------
 プレイヤー全般のスクリプト
-------------------------------------------*/

public class PlayerScript : MonoBehaviour
{
    // コンポーネント ***********************
    Rigidbody2D rbody2D; // プレイヤー
    BoxCollider2D bcol2D; // プレイヤー
    [HideInInspector] public Animator anim; // プレイヤー
    [HideInInspector] public Collider2D Hfloor; // 薄い床
    private BoxCollider2D[] Headbcol2D; // 頭
    private BoxCollider2D[] Bodybcol2D; // 体

    // スクリプト ***************************
    UIScript UIsc;
    SceneScript SceneManageSC;
    MapScript MapSC;

    // オブジェクト *************************
    GameObject AimObj; // 武器(エイム用)
    GameObject Lifesc; // ライフスクリプト(関数呼び出し用)
    GameObject admin; 

    // ジャンプ *****************************
    float jumpForce = 700f; // ジャンプ力

    [HideInInspector] public int jumpCnt = 0; // ジャンプの回数

    // bool *********************************
    [HideInInspector] public bool downCheck = false; // しゃがみチェック
    [HideInInspector] public bool downPcheck = true; // しゃがみチェック2
    [HideInInspector] public bool floorDownCheck = false; // 薄い床を降りるチェック
    [HideInInspector] public bool HfloorCheck = true; // 薄い床のチェック
    [HideInInspector] public bool pause = true; // 一時停止用
    bool Pleft = false; // プレイヤーの向き(左)
    bool Pright = false; // プレイヤーの向き(右)
    [HideInInspector] public bool Guard; // 無敵の有無
    [HideInInspector] public bool dead = false; // 生死の判定

    // float ********************************
    float nextDownTime = 0.3f; // しゃがみと薄い床を降りる判定の時間
    float nowTime = 0f; // 上記のための経過時間
    float guardTimeSR;

    // int **********************************
    public int HP; // プレイヤーのヒットポイント

    // SpriteRenderer ***********************
    SpriteRenderer PlayerSR;

    private void Awake()
    {
        Application.targetFrameRate = 30;

        HP = 10;
    }

    void Start()
    {
        rbody2D = GetComponent<Rigidbody2D>();
        bcol2D = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();

        Headbcol2D = transform.Find("Head").gameObject.GetComponents<BoxCollider2D>();
        Bodybcol2D = transform.Find("Body").gameObject.GetComponents<BoxCollider2D>();

        admin = GameObject.Find("Admin");

        GameObject obj = GameObject.Find("GC");
        UIsc = obj.GetComponent<UIScript>();
        SceneManageSC = obj.GetComponent<SceneScript>();
        MapSC = obj.GetComponent<MapScript>();

        AimObj = GameObject.Find("PAimObj");
        Lifesc = GameObject.Find("GC");

        PlayerSR = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // プレイヤーの動き
        if (pause) Move();
    }

    void Move()
    {
        // 移動速度(通常, 後ろ歩きorしゃがみ歩き)
        WALK(0.1f, 0.06f);

        // ジャンプ(ジャンプ回数上限)
        JUMP(1);

        if (Guard)
        {
            guardTimeSR += Time.deltaTime;

            if (guardTimeSR <= 0.5f) PlayerSR.enabled = true;

            if (guardTimeSR > 0.5f) PlayerSR.enabled = false;

            if (guardTimeSR >= 0.7f) guardTimeSR = 0;
        }
        else
        {
            PlayerSR.enabled = true;
        }

        // 薄い床を降りる
        if (floorDownCheck)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                anim.SetBool("Down", true);
                StartCoroutine("triggerF");

                downCheck = false;
                Hfloor.isTrigger = true;
                floorDownCheck = false;
            }
            nowTime += Time.deltaTime;
            if (nowTime >= nextDownTime)
            {
                floorDownCheck = false;
            }
        }
        else
        {
            nowTime = 0f;
        }

        // しゃがむ
        if (Input.GetKey(KeyCode.S) && downCheck)
        {
            if (downPcheck)
            {
                downPcheck = false;

                if (HfloorCheck == false) floorDownCheck = true;
            }
            // コライダー(POffsetY, PSizeY, HeadOffsetY, BodyOffsetY, BodySizeY)
            PcolSize(-0.25f, 0.8f, 0f, -0.45f, 0.5f); // 小

            anim.SetBool("Squat", true);
        }
        else
        {
            anim.SetBool("Squat", false);

            downPcheck = true;

            // コライダー(POffsetY, PSizeY, HeadOffsetY, BodyOffsetY, BodySizeY)
            PcolSize(-0.1f, 1.1f, 0.3f, -0.3f, 0.75f); // 大
        }

        // 銃の向きに合わせてプレイヤーの向きも変える
        if (AimObj.transform.localEulerAngles.y >= 80f && AimObj.transform.localEulerAngles.y <= 100f)
        {
            this.transform.localEulerAngles = new Vector3(0, 0f, 0);
            Pright = true;
            Pleft = false;
        }

        if (AimObj.transform.localEulerAngles.y >= 260f && AimObj.transform.localEulerAngles.y <= 280f)
        {
            this.transform.localEulerAngles = new Vector3(0, 180f, 0);
            Pleft = true;
            Pright = false;
        }

        AimObj.transform.position = this.transform.position;

    }

    void WALK(float walkSpeed, float slowWalkSpeed) // 左右移動
    {
        float horizontalkey = Input.GetAxis("Horizontal");

        if (horizontalkey > 0f) //　右に移動
        {
            anim.SetBool("Walk", true);

            // 後ろ歩きorしゃがみ歩きだと移動速度が遅くなる
            if (anim.GetBool("Squat") || anim.GetBool("BackWalk"))
            {
                this.transform.position += new Vector3(slowWalkSpeed, 0, 0);
            }
            else
            {
                this.transform.position += new Vector3(walkSpeed, 0, 0);
            }

            if (Pleft) anim.SetBool("BackWalk", true);
            else
            {
                anim.SetBool("BackWalk", false);
            }
        }
        else if (horizontalkey < 0f) // 左に移動
        {
            anim.SetBool("Walk", true);

            if (anim.GetBool("Squat") || anim.GetBool("BackWalk"))
            {
                this.transform.position += new Vector3(-slowWalkSpeed, 0, 0);
            }
            else
            {
                this.transform.position += new Vector3(-walkSpeed, 0, 0);
            }

            if (Pright) anim.SetBool("BackWalk", true);
            else
            {
                anim.SetBool("BackWalk", false);
            }
        }
        else
        {
            anim.SetBool("Walk", false);
            anim.SetBool("BackWalk", false);
            anim.SetBool("Squat", false);
        }
    }

    void JUMP(int maxJumpCnt)
    {
        // ジャンプ 1回だけ(2段ジャンプ以上なし)
        if (Input.GetKeyDown(KeyCode.W) && jumpCnt < maxJumpCnt || Input.GetKeyDown(KeyCode.Space) && jumpCnt < maxJumpCnt)
        {
            anim.SetBool("Jump", true);

            this.rbody2D.velocity = Vector3.zero;
            this.rbody2D.AddForce(transform.up * jumpForce);
            jumpCnt++;
        }
    }

    IEnumerator triggerF() // 薄い床貫通
    {
        var second = new WaitForSeconds(0.18f);
        yield return second;

        Hfloor.isTrigger = false;
        HfloorCheck = true;
    }

    IEnumerator GUARD(float sec) // 無敵
    {
        Guard = true;
        var second = new WaitForSeconds(sec); // 無敵時間
        yield return second;
        Guard = false;
    }

    public void Pause() // 一時停止
    {
        Text tmpTx = UIsc.pauseTx.GetComponent<Text>();

        if (pause) // 停止
        {
            rbody2D.isKinematic = true;
            rbody2D.velocity = Vector2.zero;
            StopCoroutine("GUARD");
            UIsc.panel.SetActive(true);
            tmpTx.text = "PAUSE";
            pause = !pause;
        }
        else // 再開
        {
            rbody2D.isKinematic = false;
            StartCoroutine("GUARD", 0.0f);
            UIsc.panel.SetActive(false);
            tmpTx.text = "";
            pause = !pause;
        }
    }

    // コライダー大きさ
    void PcolSize(float PlayerOffsetY, float PlayerSizeY, float headOffsetY, float bodyOffsetY, float bodySizeY)
    {
        // プレイヤー
        bcol2D.offset = new Vector2(0, PlayerOffsetY);
        bcol2D.size = new Vector2(0.5f, PlayerSizeY);

        // 頭
        Headbcol2D[0].offset = new Vector2(0, headOffsetY + 0.2f);
        Headbcol2D[1].offset = new Vector2(-0.3f, headOffsetY);
        Headbcol2D[2].offset = new Vector2(0.3f, headOffsetY);

        // 体
        Bodybcol2D[1].offset = new Vector2(-0.3f, bodyOffsetY);
        Bodybcol2D[2].offset = new Vector2(0.3f, bodyOffsetY);
        Bodybcol2D[1].size = new Vector2(0.05f, bodySizeY);
        Bodybcol2D[2].size = new Vector2(0.05f, bodySizeY);
    }

    public void PlayerHitDamage(int hitdamage)　// プレイヤーが受けるダメージ
    {
        if (HP > 0 && !Guard) HP -= hitdamage;
        Lifesc.SendMessage("AllClear");
        Lifesc.SendMessage("LifeSet");
        if (HP <= 0)
        {
            PlayerPrefs.SetInt("SCORE_RESULT", admin.GetComponent<Score>().ScoreCount);
            PlayerPrefs.SetInt("BOSScnt", MapSC.BossClearCnt);
            dead = true;
            SceneManageSC.Invoke("InResult", 4f);
            Destroy(this.gameObject);
        }
    }
}
