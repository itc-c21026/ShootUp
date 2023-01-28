using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E103_Attack : MonoBehaviour
{
    GameObject Player;
    PlayerScript Pscript;

    GameObject DeadDown;

    Rigidbody2D rb;

    GameObject EnemyDrop;
    public float Damage;
    public int HP;
    public int ReceiveDamage;
    bool OK;
    int Gre1;
    int Gre2;
    bool Dead;

    bool pose = false;
    void Start()
    {
        DeadDown = Resources.Load<GameObject>("EnemyDown");
        Player = GameObject.Find("Player");
        Pscript = Player.GetComponent<PlayerScript>();

        rb = this.GetComponent<Rigidbody2D>();

        EnemyDrop = GameObject.Find("Admin");
        Invoke("destroy", 5);
        Invoke("ok", 0.1f);
        Gre1 = 20;
        Gre2 = 15;
    }
    void Update()
    {
        if (Pscript.pause)
        {
            if (!pose)
            {
                rb.velocity = transform.right * -20;
                pose = true;
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
            pose = false;
        }

    }
    void ok()
    {
        OK = true;
    }
    void destroy()
    {
        Destroy(this.gameObject);
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
    void CCheck()
    {
        ReceiveDamage = GetComponent<ReceiveDamage>().ReceiveCount;
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
                //Debug.Log("Kill");
                Destroy(this.gameObject);
                GameObject score = GameObject.Find("Admin").gameObject;
                score.GetComponent<Score>().ReceiveScore = 10;
                score.GetComponent<Score>().Score_Plus();
                Dead = true;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (OK)
        {
            if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Ground")
            {
                GameObject downObj = Instantiate(DeadDown, this.transform.position, Quaternion.identity);
                Destroy(downObj, 5f);
                Destroy(this.gameObject);
            }
        }
        if (collision.gameObject.tag == "Player")
        {
            if (!Pscript.Guard)
            {
                GameObject downObj = Instantiate(DeadDown, this.transform.position, Quaternion.identity);
                Destroy(downObj, 5f);
                collision.transform.root.gameObject.GetComponent<PlayerScript>().PlayerHitDamage((int)Damage * 2);
                Pscript.StartCoroutine("GUARD", 3.0f);
            }
            Destroy(this.gameObject);
        }
    }
}
