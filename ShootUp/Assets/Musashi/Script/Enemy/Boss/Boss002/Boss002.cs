using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss002 : MonoBehaviour
{
    public GameObject Body;
    public GameObject Critical;
    GameObject Beam;
    GameObject beam;
    GameObject Charge;
    GameObject Player;
    GameObject EnemyDrop;

    GameObject GC;

    PlayerScript Pscript;

    AudioClipScript ACSC;

    Rigidbody2D RB;

    public int HP;
    public int ReceiveDamage;
    int Gre1;
    int Gre2;
    float SummonCT;
    float AttackCT;
    float CT;
    float AttackSec;
    float MoveSec;

    public bool Attack;
    bool Dead;
    bool NowAttack;
    bool Summon;
    bool Lookat;
    bool Stop;

    bool CTcheck = false;
    bool beamCheck1 = false;
    bool beamCheck2 = false;
    bool moveCheck = false;
    bool summon = false;

    Animator Anim;
    void Start()
    {
        HP = 500;

        Player = GameObject.Find("Player");
        Pscript = Player.GetComponent<PlayerScript>();
        ACSC = GameObject.Find("Audio").GetComponent<AudioClipScript>();
        EnemyDrop = GameObject.Find("Admin");
        GC = GameObject.Find("GC");
        Beam = transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.gameObject;
        beam = Beam.transform.GetChild(0).transform.gameObject;
        Charge = transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).transform.gameObject;
        RB = GetComponent<Rigidbody2D>();
        Gre1 = 20;
        Gre2 = 15;
        Critical.SetActive(false);
        Anim = transform.GetChild(0).GetComponent<Animator>();
    }
    void Update()
    {
        if (Player != null)
        {
            if (Beam != null)
            {
                if (!Pscript.dead && Pscript.pause)
                {
                    Beam.transform.localPosition = new Vector3(-0.11f, 
                                                          0, 
                                                          0);
                    Beam.transform.localEulerAngles = Vector3.zero;
                    Beam.transform.rotation = default;
                    beam.transform.localEulerAngles = Vector3.zero;
                    beam.transform.rotation = default;
                    if (!Lookat)
                    {
                        transform.LookAt(Player.transform);
                    }

                    Vector2 Dif = Vector2.zero;
                    if (!Pscript.dead)
                        Dif = Player.transform.position - transform.position;

                    SummonCT += Time.deltaTime;
                    if (Attack)
                    {
                        AttackCT += Time.deltaTime;
                        if (!Summon)
                        {
                            if (AttackCT >= 3)
                            {
                                Stop = true;
                                NowAttack = true;
                                beamCheck1 = false;
                                beamCheck2 = false;
                                AttackCT = 0;
                                //StartCoroutine("BeamAttack");
                                CTcheck = true;
                            }
                        }
                    }
                    if (CTcheck)
                        BeamAttack();
                    if (SummonCT >= 15)//何秒おきに召喚するか
                    {
                        Summon = true;
                        Stop = true;
                        SummonCT = 0;
                    }
                    if (!Stop)
                    {
                        if (Dif.x + Dif.y >= 10 || Dif.x + Dif.y <= -10)
                        {
                            tackle();
                            CT = 0;
                        }
                        else
                        {
                            CT += Time.deltaTime;
                            if (CT >= .5f)
                            {
                                //StartCoroutine("Move");
                                Move();
                                CT = 0;
                            }
                        }
                    }
                    else
                    {
                        RB.velocity = Vector3.zero;
                        if (!NowAttack)
                        {
                            if (Summon)
                            {
                                if (!summon)
                                {
                                    //タレットを召喚
                                    int random = Random.Range(12, 18);
                                    int x = Random.Range(5, 8);
                                    GC.GetComponent<MapScript>().EnemyInst(random, 15, x + Random.Range(-1, 2) - 60, 12.0f, 12);
                                    random = Random.Range(12, 18);
                                    x = Random.Range(5, 8);
                                    GC.GetComponent<MapScript>().EnemyInst(random, -1, x + Random.Range(-1, 2) - 60, 12.0f, 12);
                                    summon = true;
                                    SoundController.Instance.PlaySE(ACSC.SE[4], 3f);
                                }

                                Anim.SetBool("Call", true);
                                //タレットを召喚中に止まる時間
                                Invoke("Normal", 3f);
                            }
                        }
                    }
                }
                else
                {
                    if (moveCheck)
                    {
                        RB.velocity = Vector3.zero;
                        RB.isKinematic = true;
                        moveCheck = false;
                    }
                }
            }
        }
    }
    void Normal()
    {
        Anim.SetBool("Call", false);
        Summon = false;
        Stop = false;
        summon = false;
    }
    void tackle()
    {
        RB.AddForce((Player.transform.position - transform.position)*20);
        RB.velocity = Vector3.zero;
    }
    //IEnumerator BeamAttack()
    //{
    //    Stop = true;
    //    NowAttack = true;
    //    yield return new WaitForSeconds(1f);
    //    Anim.SetBool("Open", true);
    //    Lookat = true;
    //    Body.SetActive(false);
    //    Critical.SetActive(true);
    //    Beam.SetActive(true);
    //    Charge.SetActive(true);
    //    yield return new WaitForSeconds(2f);
    //    BeamAttackFalse();
    //}

    void BeamAttack()
    {
        AttackSec += Time.deltaTime;
        //yield return new WaitForSeconds(1f);
        for (int i = 0; i <= 1; i++)
        {
            SoundController.Instance.PlaySE(ACSC.SE[5], 2f);
        }
        if (!beamCheck1 && AttackSec >= 0.5f)
        {
            Anim.SetBool("Open", true);
            Lookat = true;
            Body.SetActive(false);
            Critical.SetActive(true);
            Beam.SetActive(true);
            Charge.SetActive(true);
            beamCheck1 = true;
        }
        //yield return new WaitForSeconds(2f);
        if (!beamCheck2 && AttackSec >= 2.5f)
        {
            BeamAttackFalse();
            CTcheck = false;
            AttackSec = 0;
            beamCheck2 = true;
        }
    }

    void BeamAttackFalse()
    {
        Body.SetActive(true);
        Critical.SetActive(false);
        Stop = false;
        NowAttack = false;
        Anim.SetBool("Open",false);
        Lookat = false;
    }
    //IEnumerator Move()
    //{
    //    RB.AddForce(Random.insideUnitCircle.normalized*350);
    //    yield return new WaitForSeconds(0.3f);
    //    RB.velocity = Vector3.zero;
    //    yield return new WaitForSeconds(0.1f);
    //}
    void Move()
    {
        if (Pscript.pause)
        {
            if (!moveCheck)
            {
                RB.isKinematic = false;
                moveCheck = true;
            }

            RB.AddForce(Random.insideUnitCircle.normalized * 350);

            MoveSec += 0.1f;
            //yield return new WaitForSeconds(0.3f);
            if (MoveSec >= 0.3f)
                RB.velocity = Vector3.zero;
            //yield return new WaitForSeconds(0.1f);
            if (MoveSec >= 0.4f)
                MoveSec = 0;
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
                GC.GetComponent<UIScript>().Slider.SetActive(false);
                GC.GetComponent<UIScript>().BossInt = 99;
                GC.GetComponent<MapScript>().BossClear = true;
                EnemyDrop.SendMessage("BossDrop");
                EnemyDrop.GetComponent<EnemyDrop>().BulletUIinst(this.transform, "WeaponAmmo");
                if (EnemyDrop.GetComponent<EnemyDrop>().Ran == 0 || EnemyDrop.GetComponent<EnemyDrop>().Ran == 1)
                    EnemyDrop.GetComponent<EnemyDrop>().BulletUIinst(this.transform, "SpecialAmmo");
                Destroy(this.gameObject);
                Dead = true;
            }
        }
    }
}
