using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*-------------------------------------------
 �v���C���[�S�ʂ̃X�N���v�g
-------------------------------------------*/

public class PlayerScript : MonoBehaviour
{
    // �R���|�[�l���g ***********************
    Rigidbody2D rbody2D; // �v���C���[
    BoxCollider2D bcol2D; // �v���C���[
    [HideInInspector] public Animator anim; // �v���C���[
    [HideInInspector] public Collider2D Hfloor; // ������
    private BoxCollider2D[] Headbcol2D; // ��
    private BoxCollider2D[] Bodybcol2D; // ��

    // �X�N���v�g ***************************
    UIScript UIsc;
    SceneScript SceneManageSC;
    MapScript MapSC;

    // �I�u�W�F�N�g *************************
    GameObject AimObj; // ����(�G�C���p)
    GameObject Lifesc; // ���C�t�X�N���v�g(�֐��Ăяo���p)
    GameObject admin; 

    // �W�����v *****************************
    float jumpForce = 700f; // �W�����v��

    [HideInInspector] public int jumpCnt = 0; // �W�����v�̉�

    // bool *********************************
    [HideInInspector] public bool downCheck = false; // ���Ⴊ�݃`�F�b�N
    [HideInInspector] public bool downPcheck = true; // ���Ⴊ�݃`�F�b�N2
    [HideInInspector] public bool floorDownCheck = false; // ���������~���`�F�b�N
    [HideInInspector] public bool HfloorCheck = true; // �������̃`�F�b�N
    [HideInInspector] public bool pause = true; // �ꎞ��~�p
    bool Pleft = false; // �v���C���[�̌���(��)
    bool Pright = false; // �v���C���[�̌���(�E)
    [HideInInspector] public bool Guard; // ���G�̗L��
    [HideInInspector] public bool dead = false; // �����̔���

    // float ********************************
    float nextDownTime = 0.3f; // ���Ⴊ�݂Ɣ��������~��锻��̎���
    float nowTime = 0f; // ��L�̂��߂̌o�ߎ���
    float guardTimeSR;

    // int **********************************
    public int HP; // �v���C���[�̃q�b�g�|�C���g

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
        // �v���C���[�̓���
        if (pause) Move();
    }

    void Move()
    {
        // �ړ����x(�ʏ�, ������or���Ⴊ�ݕ���)
        WALK(0.1f, 0.06f);

        // �W�����v(�W�����v�񐔏��)
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

        // ���������~���
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

        // ���Ⴊ��
        if (Input.GetKey(KeyCode.S) && downCheck)
        {
            if (downPcheck)
            {
                downPcheck = false;

                if (HfloorCheck == false) floorDownCheck = true;
            }
            // �R���C�_�[(POffsetY, PSizeY, HeadOffsetY, BodyOffsetY, BodySizeY)
            PcolSize(-0.25f, 0.8f, 0f, -0.45f, 0.5f); // ��

            anim.SetBool("Squat", true);
        }
        else
        {
            anim.SetBool("Squat", false);

            downPcheck = true;

            // �R���C�_�[(POffsetY, PSizeY, HeadOffsetY, BodyOffsetY, BodySizeY)
            PcolSize(-0.1f, 1.1f, 0.3f, -0.3f, 0.75f); // ��
        }

        // �e�̌����ɍ��킹�ăv���C���[�̌������ς���
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

    void WALK(float walkSpeed, float slowWalkSpeed) // ���E�ړ�
    {
        float horizontalkey = Input.GetAxis("Horizontal");

        if (horizontalkey > 0f) //�@�E�Ɉړ�
        {
            anim.SetBool("Walk", true);

            // ������or���Ⴊ�ݕ������ƈړ����x���x���Ȃ�
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
        else if (horizontalkey < 0f) // ���Ɉړ�
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
        // �W�����v 1�񂾂�(2�i�W�����v�ȏ�Ȃ�)
        if (Input.GetKeyDown(KeyCode.W) && jumpCnt < maxJumpCnt || Input.GetKeyDown(KeyCode.Space) && jumpCnt < maxJumpCnt)
        {
            anim.SetBool("Jump", true);

            this.rbody2D.velocity = Vector3.zero;
            this.rbody2D.AddForce(transform.up * jumpForce);
            jumpCnt++;
        }
    }

    IEnumerator triggerF() // �������ђ�
    {
        var second = new WaitForSeconds(0.18f);
        yield return second;

        Hfloor.isTrigger = false;
        HfloorCheck = true;
    }

    IEnumerator GUARD(float sec) // ���G
    {
        Guard = true;
        var second = new WaitForSeconds(sec); // ���G����
        yield return second;
        Guard = false;
    }

    public void Pause() // �ꎞ��~
    {
        Text tmpTx = UIsc.pauseTx.GetComponent<Text>();

        if (pause) // ��~
        {
            rbody2D.isKinematic = true;
            rbody2D.velocity = Vector2.zero;
            StopCoroutine("GUARD");
            UIsc.panel.SetActive(true);
            tmpTx.text = "PAUSE";
            pause = !pause;
        }
        else // �ĊJ
        {
            rbody2D.isKinematic = false;
            StartCoroutine("GUARD", 0.0f);
            UIsc.panel.SetActive(false);
            tmpTx.text = "";
            pause = !pause;
        }
    }

    // �R���C�_�[�傫��
    void PcolSize(float PlayerOffsetY, float PlayerSizeY, float headOffsetY, float bodyOffsetY, float bodySizeY)
    {
        // �v���C���[
        bcol2D.offset = new Vector2(0, PlayerOffsetY);
        bcol2D.size = new Vector2(0.5f, PlayerSizeY);

        // ��
        Headbcol2D[0].offset = new Vector2(0, headOffsetY + 0.2f);
        Headbcol2D[1].offset = new Vector2(-0.3f, headOffsetY);
        Headbcol2D[2].offset = new Vector2(0.3f, headOffsetY);

        // ��
        Bodybcol2D[1].offset = new Vector2(-0.3f, bodyOffsetY);
        Bodybcol2D[2].offset = new Vector2(0.3f, bodyOffsetY);
        Bodybcol2D[1].size = new Vector2(0.05f, bodySizeY);
        Bodybcol2D[2].size = new Vector2(0.05f, bodySizeY);
    }

    public void PlayerHitDamage(int hitdamage)�@// �v���C���[���󂯂�_���[�W
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
