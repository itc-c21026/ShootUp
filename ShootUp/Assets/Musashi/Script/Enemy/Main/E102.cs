using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E102 : MonoBehaviour
{
    GameObject Player;
    PlayerScript Pscript;
    public GameObject EnemyController;
    public GameObject Parent;
    public GameObject Body;
    public GameObject Critical;
    GameObject EnemyDrop;

    GameObject GC;
    GameObject DeadDown;

    [HideInInspector] public GameObject ebar;

    Rigidbody2D Rb;

    [HideInInspector] public int HP;
    public int ReceiveDamage;
    float JumpCt;
    public float JumpTime = 5;
    public float JumpPower = 1500;
    public string[] Mystatus = new string[7];
    int Gre1;
    int Gre2;

    public GameObject Child1;
    public int Dir = -1;

    bool Ground;
    bool GroundCheck = true;
    bool NowJump;
    bool Dead;
    Animator anim;
    private void Awake()
    {
        EnemyController = GameObject.Find("Admin");
        Mystatus = EnemyController.GetComponent<EnemyController>().TypeA2;
    }
    void Start()
    {
        DeadDown = Resources.Load<GameObject>("EnemyDown");
        GC = GameObject.Find("GC");
        Player = GameObject.Find("Player");
        Pscript = Player.GetComponent<PlayerScript>();
        this.gameObject.transform.parent = Parent.gameObject.transform;
        Rb = Parent.GetComponent<Rigidbody2D>();
        anim = Parent.GetComponent<Animator>();
        Gre1 = 20;
        Gre2 = 15;
        EnemyDrop = GameObject.Find("Admin");
        HP = int.Parse(Mystatus[1]);
        anim.SetBool("Walk", true);
    }
    void Update()
    {
        if (Player != null)
        {
            if (!Pscript.dead && Pscript.pause)
            {
                Rb.isKinematic = false;

                JumpCt += Time.deltaTime;
                if (JumpCt >= JumpTime)
                {
                    Jump();
                    JumpCt = 0;
                }

                Vector2 Direction = Vector2.zero;
                Direction = Player.transform.position - transform.position;

                if (GroundCheck)
                {
                    Ground = Child1.GetComponent<GroundChecker>().Ground;
                }
                if (Direction.x >= 0.1f)
                {
                    if (Dir == -1)
                    {
                        Reversal();
                    }
                }
                else if (Direction.x <= -0.1f)
                {
                    if (Dir == 1)
                    {
                        Reversal();
                    }
                }
                if (!NowJump)
                {
                    Rb.velocity = transform.right * -float.Parse(Mystatus[3]) * 2;
                    if (Ground)
                    {
                        Rb.velocity = transform.right * -float.Parse(Mystatus[3]) * 2;
                    }
                    else
                    {
                        Rb.velocity = transform.right * 0;
                    }
                }
                else
                {
                    Parent.transform.position = new Vector3(Parent.transform.position.x, Parent.transform.position.y, 0);
                }
            }
            else
            {
                Rb.isKinematic = true;
                Rb.velocity = Vector2.zero;
            }
        }
    }
    void BCheck()
    {
        ReceiveDamage = Body.GetComponent<ReceiveDamage>().ReceiveCount;
    }
    void CCheck()
    {
        ReceiveDamage = Critical.GetComponent<ReceiveDamage>().ReceiveCount;
    }
    void Grenade1()
    {
        ReceiveDamage = Gre1;
        HPCheck();
    }
    void Grenade2()
    {
        ReceiveDamage = Gre2;
        HPCheck();
    }
    public void HPCheck()
    {
        HP -= ReceiveDamage;
        if (!Dead)
        {
            if (HP <= 0)
            {
                GameObject downObj = Instantiate(DeadDown, this.transform.position, Quaternion.identity);
                Destroy(downObj, 5f);
                EnemyDrop.SendMessage("Drop");
                EnemyDrop.GetComponent<EnemyDrop>().BulletUIinst(this.transform, "WeaponAmmo");
                if (EnemyDrop.GetComponent<EnemyDrop>().Ran == 0 || EnemyDrop.GetComponent<EnemyDrop>().Ran == 1)
                    EnemyDrop.GetComponent<EnemyDrop>().BulletUIinst(this.transform, "SpecialAmmo");
                Destroy(ebar);
                //Debug.Log("Kill");
                GameObject score = GameObject.Find("Admin").gameObject;
                score.GetComponent<Score>().ReceiveScore = 10;
                GC.GetComponent<UIScript>().E_ScoreTxInst(this.transform, score.GetComponent<Score>().ReceiveScore);
                score.GetComponent<Score>().Score_Plus();
                Destroy(Parent.gameObject);
                Dead = true;
            }
        }
    }

    void Jump()
    {
        anim.SetBool("Jump", true);
        NowJump = true;
        Invoke("JumpOn", 0.2f);
    }
    void JumpOn()
    {
        //Rb.AddForce(transform.up * JumpPower);
        GroundCheck = false;
        //Rb.velocity = transform.right * -10;

        Vector3 PlayerPos = Player.transform.position;
        if (PlayerPos.x <= -5)
            transform.root.gameObject.transform.position = new Vector3(PlayerPos.x + 2, PlayerPos.y + 5f, 0);
        else if (PlayerPos.x > -5)
            transform.root.gameObject.transform.position = new Vector3(PlayerPos.x - 2, PlayerPos.y + 5f, 0);

        GameObject obj = Instantiate(GC.GetComponent<UIScript>().EffectObj[2],
            this.transform.position, Quaternion.identity);
        Destroy(obj, 1f);

        Invoke("WalkOn", .4f);
    }
    void WalkOn()
    {
        anim.SetBool("Jump", false);
        anim.SetBool("Walk", true);
        GroundCheck = true;
        NowJump = false;
    }
    public void Reversal()
    {
        Parent.transform.Rotate(0, 180, 0);
        Dir = Dir * -1;
        Child1.GetComponent<GroundChecker>().CheckPos = Dir;
    }
}
