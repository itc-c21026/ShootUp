using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E203 : MonoBehaviour
{
    GameObject Player;
    PlayerScript Pscript;
    public GameObject EnemyController;
    public GameObject Parent;
    public GameObject Body;
    public GameObject Critical;
    public GameObject Attackobj;
    public GameObject Bullet;
    GameObject EnemyDrop;

    GameObject DeadDown;

    GameObject GC;

    [HideInInspector] public GameObject ebar;

    Rigidbody2D Rb;

    [HideInInspector] public int HP;
    public int ReceiveDamage;
    public string[] Mystatus = new string[7];
    float CT;
    int Gre1;
    int Gre2;

    public GameObject Child;
    public int Dir = -1;

    public bool Attack;
    bool Ground;
    bool Dead;
    public Animator Left;
    public Animator Right;
    private void Awake()
    {
        EnemyController = GameObject.Find("Admin");
        Mystatus = EnemyController.GetComponent<EnemyController>().TypeB3;
    }
    void Start()
    {
        DeadDown = Resources.Load<GameObject>("EnemyDown");
        GC = GameObject.Find("GC");
        Player = GameObject.Find("Player");
        Pscript = Player.GetComponent<PlayerScript>();
        this.gameObject.transform.parent = Parent.gameObject.transform;
        Rb = Parent.GetComponent<Rigidbody2D>();
        Gre1 = 20;
        Gre2 = 15;
        EnemyDrop = GameObject.Find("Admin");
        HP = int.Parse(Mystatus[1]);
    }
    void Update()
    {
        if (Player != null)
        {
            if (!Pscript.dead && Pscript.pause)
            {
                Rb.isKinematic = false;

                Attack = Attackobj.GetComponent<Attack>().enemy;

                Vector2 Direction = Vector2.zero;
                Direction = Player.transform.position - transform.position;

                Ground = Child.GetComponent<GroundChecker>().Ground;
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
                if (Attack)
                {
                    CT += Time.deltaTime;
                    Left.SetBool("Attack", false);
                    Right.SetBool("Attack", false);
                }
                else
                {
                    Left.SetBool("Attack", true);
                    Right.SetBool("Attack", true);
                }
                if (Ground)
                {
                    Rb.velocity = transform.right * -float.Parse(Mystatus[3]) * 2;
                    if (Attack)
                    {
                        Rb.velocity = transform.right * 0;
                    }
                }
                else
                {
                    Rb.velocity = transform.right * 0;
                }
                if (CT >= float.Parse(Mystatus[4]))
                {
                    for (int i = 0; i < 3; i++)
                    {
                        GameObject Enemy_Bullet = Instantiate(Bullet, Attackobj.transform.position, Attackobj.transform.rotation);
                        Enemy_Bullet.name = "EnemyShotGun";
                        Enemy_Bullet.GetComponent<EnemyBullet>().Damage = float.Parse(Mystatus[2]);
                        Enemy_Bullet.GetComponent<EnemyBullet>().Accuracy_Count = 20;
                    }
                    CT = 0;
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
                EnemyDrop.SendMessage("ShotGunDrop");
                EnemyDrop.GetComponent<EnemyDrop>().BulletUIinst(this.transform, "WeaponAmmo");
                if (EnemyDrop.GetComponent<EnemyDrop>().Ran == 0 || EnemyDrop.GetComponent<EnemyDrop>().Ran == 1)
                    EnemyDrop.GetComponent<EnemyDrop>().BulletUIinst(this.transform, "SpecialAmmo");
                Destroy(ebar);
                GameObject score = GameObject.Find("Admin").gameObject;
                score.GetComponent<Score>().ReceiveScore = 10;
                GC.GetComponent<UIScript>().E_ScoreTxInst(this.transform, score.GetComponent<Score>().ReceiveScore);
                score.GetComponent<Score>().Score_Plus();
                Destroy(Parent.gameObject);
                Dead = true;
            }
        }
    }
    public void Reversal()
    {
        Parent.transform.Rotate(0, 180, 0);
        Dir = Dir * -1;
        Child.GetComponent<GroundChecker>().CheckPos = Dir;
    }
}
