using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Enemy : MonoBehaviour
{
    GameObject Player;
    public GameObject EnemyController;
    public GameObject Parent;
    public GameObject Attackobj;
    public GameObject Bullet;
    Rigidbody2D Rb;

    public int ReceiveDamage;
    public bool Select;
    public bool ActiveType;
    public string[] Mystatus = new string[7];
    float CT;

    public GameObject Child1;
    public GameObject Child2;
    public int Dir = -1;

    public bool Attack;
    Animator anim;
    void Start()
    {
        Player = GameObject.Find("Player");
        EnemyController = GameObject.Find("EnemyController");
        if (!Select)
        {
            if (ActiveType)
            {
                //int Ran = Random.Range(0, 4);
                int Ran = 0;
                switch (Ran)
                {
                    case 0:
                        Mystatus = EnemyController.GetComponent<EnemyController>().TypeA1;
                        GameObject Myobj = (GameObject)Resources.Load("Enemy/101");
                        Parent = Instantiate(Myobj,transform.position,Quaternion.identity);
                        this.gameObject.transform.parent = Parent.gameObject.transform;
                        break;
                }
            }
            else
            {
                int Ran = 4;
                //int Ran = Random.Range(4, 11);
                switch (Ran)
                {
                    case 4:
                        Mystatus = EnemyController.GetComponent<EnemyController>().TypeB1;
                        GameObject Myobj = (GameObject)Resources.Load("Enemy/201");
                        Parent = Instantiate(Myobj, transform.position, Quaternion.identity);
                        this.gameObject.transform.parent = Parent.gameObject.transform;
                        break;
                }
            }
        }
        Rb = Parent.GetComponent<Rigidbody2D>();
        anim = Parent.GetComponent<Animator>();
    }
    void Update()
    {
        Vector2 Direction = Player.transform.position - transform.position;
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
        if (!Attack)
        {
            Rb.velocity = transform.right * -float.Parse(Mystatus[3])*2;
        }
        else
        {
            Rb.velocity = transform.right * 0;
            CT += Time.deltaTime;
            switch (Mystatus[5])
            {
                case "Approach":
                    break;
                case "Rush":
                    break;
                case "Pistol":
                    if (CT >= float.Parse(Mystatus[4]))
                    {
                        GameObject Enemy_Bullet = Instantiate(Bullet, Attackobj.transform.position, Attackobj.transform.rotation);
                        Enemy_Bullet.GetComponent<EnemyBullet>().Damage = float.Parse(Mystatus[2]);
                        CT = 0;
                    }
                    break;
                case "Sniper":
                    break;
                case "Shotgun":
                    break;
                case "Tracking":
                    break;
                case "MachineGun":
                    break;
            }
        }
    }
    void AttackOK()
    {
        if (Attack)
        {
            anim.SetBool("Attack", false);
            Attack = false;
        }
        else
        {
            anim.SetBool("Attack", true);
            Attack = true;
        }
    }
    public void HPCheck()
    {
        Debug.Log(ReceiveDamage+"É_ÉÅÅ[ÉWéÛÇØÇΩ");
    }
    public void Reversal()
    {
        Parent.transform.Rotate(0, 180, 0);
        Dir = Dir * -1;
        Child1.GetComponent<GroundChecker>().CheckPos = Dir;
        Child1.GetComponent<JumpGroundCheck>().CheckPos = Dir;
    }
}
