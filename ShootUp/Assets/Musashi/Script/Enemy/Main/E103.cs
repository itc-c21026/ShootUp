using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E103 : MonoBehaviour
{
    GameObject Player;
    PlayerScript Pscript;

    GameObject DeadDown;

    public GameObject EnemyController;
    public GameObject Parent;
    public GameObject Body;
    public GameObject Critical;
    public GameObject AttackBullet;
    public GameObject AttackObj;
    GameObject EnemyDrop;

    GameObject GC;

    [HideInInspector] public GameObject ebar;

    [HideInInspector] public int HP;
    public int ReceiveDamage;
    public string[] Mystatus = new string[7];
    int Gre1;
    int Gre2;

    public int Dir = -1;

    public bool Attack;
    bool Dead;
    bool Instantiate_;
    private void Awake()
    {
        EnemyController = GameObject.Find("Admin");
        Mystatus = EnemyController.GetComponent<EnemyController>().TypeA3;
    }
    void Start()
    {
        DeadDown = Resources.Load<GameObject>("EnemyDown");
        GC = GameObject.Find("GC");
        Player = GameObject.Find("Player");
        Pscript = Player.GetComponent<PlayerScript>();
        this.gameObject.transform.parent = Parent.gameObject.transform;
        Gre1 = 20;
        Gre2 = 15;
        EnemyDrop = GameObject.Find("Admin");
        HP = int.Parse(Mystatus[1]);
    }
    void AttackOK()
    {
        if (Player != null)
        {
            if (!Pscript.dead && Pscript.pause)
            {
                if (!Instantiate_)
                {
                    GameObject E = Instantiate(AttackBullet, transform.position, AttackObj.transform.rotation);
                    E.GetComponent<E103_Attack>().HP = HP;
                    E.GetComponent<E103_Attack>().Damage = float.Parse(Mystatus[2]);
                    E.name = "Critical";
                    Destroy(ebar);
                    Destroy(Parent.gameObject);
                    Instantiate_ = true;
                }
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
}
