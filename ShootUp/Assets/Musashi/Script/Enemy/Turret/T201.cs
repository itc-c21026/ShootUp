using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T201 : MonoBehaviour
{
    GameObject Player;
    PlayerScript Pscript;

    GameObject GC;

    int Gre1;
    int Gre2;
    [HideInInspector] public float HP;
    bool Dead;
    public float ReceiveDamage;
    GameObject EnemyDrop;
    GameObject TurretController;
    public GameObject My_Muzzle1;
    public GameObject My_Muzzle2;
    public GameObject My_Muzzle3;
    public GameObject TurretBullet;
    public GameObject body;

    [HideInInspector] public GameObject ebar;

    public string[] Mystatus = new string[6];
    float CT;
    //ñºëO(0),HP(1),çUåÇóÕ(2),ÉåÅ[Ég(3),î≠éÀêî(4),íeë¨(5)
    void Start()
    {
        Player = GameObject.Find("Player");
        Pscript = Player.GetComponent<PlayerScript>();
        GC = GameObject.Find("GC");

        TurretController = GameObject.Find("Admin");
        Mystatus = TurretController.GetComponent<TurretController>().TypeC1;
        EnemyDrop = GameObject.Find("Admin");
        HP = float.Parse(Mystatus[1]);
        Gre1 = 20;
        Gre2 = 15;
    }
    void Update()
    {
        if (Pscript.pause)
        {
            CT += Time.deltaTime;
            if (CT >= float.Parse(Mystatus[3]))
            {
                GameObject T_Bullet = Instantiate(TurretBullet, My_Muzzle1.transform.position, My_Muzzle1.transform.rotation);
                T_Bullet.GetComponent<TurretBullet>().Damage = float.Parse(Mystatus[2]);
                T_Bullet.GetComponent<TurretBullet>().Speed = float.Parse(Mystatus[5]);
                T_Bullet.name = "Missile";
                T_Bullet = Instantiate(TurretBullet, My_Muzzle2.transform.position, My_Muzzle2.transform.rotation);
                T_Bullet.GetComponent<TurretBullet>().Damage = float.Parse(Mystatus[2]);
                T_Bullet.GetComponent<TurretBullet>().Speed = float.Parse(Mystatus[5]);
                T_Bullet.name = "Missile";
                T_Bullet = Instantiate(TurretBullet, My_Muzzle3.transform.position, My_Muzzle3.transform.rotation);
                T_Bullet.GetComponent<TurretBullet>().Damage = float.Parse(Mystatus[2]);
                T_Bullet.GetComponent<TurretBullet>().Speed = float.Parse(Mystatus[5]);
                T_Bullet.name = "Missile";
                CT = 0;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            ReceiveDamage = collision.GetComponent<Bullet>().Damage;
        }
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
    void HPCheck()
    {
        ReceiveDamage = body.GetComponent<ReceiveDamage>().ReceiveCount;
        HP -= ReceiveDamage;
        if (!Dead)
        {
            if (HP <= 0)
            {
                EnemyDrop.SendMessage("Drop");
                EnemyDrop.GetComponent<EnemyDrop>().BulletUIinst(this.transform, "WeaponAmmo");
                if (EnemyDrop.GetComponent<EnemyDrop>().Ran == 0 || EnemyDrop.GetComponent<EnemyDrop>().Ran == 1)
                    EnemyDrop.GetComponent<EnemyDrop>().BulletUIinst(this.transform, "SpecialAmmo");
                Destroy(ebar);
                //Debug.Log("Kill");
                Destroy(this.gameObject);
                GameObject score = GameObject.Find("Admin").gameObject;
                score.GetComponent<Score>().ReceiveScore = 20;
                GC.GetComponent<UIScript>().E_ScoreTxInst(this.transform, score.GetComponent<Score>().ReceiveScore);
                score.GetComponent<Score>().Score_Plus();
                Dead = true;
            }
        }
    }
}
