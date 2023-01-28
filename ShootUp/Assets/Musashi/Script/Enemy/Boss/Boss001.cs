using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Boss001 : MonoBehaviour
{
    public TextAsset csvFile;
    public List<string[]> csvDatas = new List<string[]>();

    public GameObject RushDamage;

    GameObject GC;

    GameObject Player;
    PlayerScript Pscript;
    GameObject EnemyDrop;
    public GameObject Body;
    public GameObject Critical;
    public GameObject Bulletobj;
    public GameObject RushObj;

    public int HP;
    public int ReceiveDamage;
    float ShotCoolTime;
    int Gre1;
    int Gre2;
    Rigidbody2D Rb;

    public GameObject Child1;
    public int Dir = -1;

    bool Ground;
    bool GroundCheck = true;
    bool Dead;
    public bool Attack1;
    bool tackle;
    Animator anim;

private void Awake()
    {
        csvFile = Resources.Load("Boss001") as TextAsset;
        StringReader reader = new StringReader(csvFile.text);
        while (reader.Peek() != -1)
        {
            string line = reader.ReadLine();
            csvDatas.Add(line.Split(',')); 
        }

        HP = int.Parse(csvDatas[1][1]);
        GC = GameObject.Find("GC");
        GC.GetComponent<MapScript>().HP = HP;
    }

    void Start()
    {
        Player = GameObject.Find("Player");
        Pscript = Player.GetComponent<PlayerScript>();
        Rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        Gre1 = 20;
        Gre2 = 15;
        EnemyDrop = GameObject.Find("Admin");
    }
    void Update()
    {
        if (Player != null)
        {
            if (!Pscript.dead && Pscript.pause)
            {
                if (Player.transform.position.y + 9 <= this.transform.position.y)
                    this.transform.position += new Vector3(0, -9, 0);

                Basic();
                Rb.isKinematic = false;
            }
            else
            {
                Rb.isKinematic = true;
                Rb.velocity = Vector2.zero;
            }
        }
    }
    void Basic()
    {
        ShotCoolTime += Time.deltaTime;

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
                if (!Attack1) Reversal();
            }
        }
        else if (Direction.x <= -0.1f)
        {
            if (Dir == 1)
            {
                if (!Attack1) Reversal();
            }
        }
        if (Ground)
        {
            if (!Attack1)
            {
                Rb.velocity = transform.right * -float.Parse(csvDatas[1][3]) * 17;
                anim.SetBool("tackle", false);
                anim.SetBool("walk", true);
                if (ShotCoolTime >= Random.Range(5, 15))
                {
                    if (!tackle)
                    {
                        StartCoroutine("Shot");
                        ShotCoolTime = 0;
                    }
                }
            }
            if (tackle)
            {
                Rb.velocity = transform.right * -float.Parse(csvDatas[1][5]) * 10;
                anim.SetBool("tackle", true);
                anim.SetBool("walk", false);
                RushDamage.GetComponent<RushDamage>().OK = true;
            }
        }
        else
        {
            Rb.velocity = transform.right * 0;
            anim.SetBool("walk", false);
        }
    }
    
    void GroudCheck()
    {
        anim.SetBool("spin", false);
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
    void Rush()
    {
        Attack1 = true;
        StartCoroutine("Tackle");
    }
    IEnumerator Tackle()
    {
        anim.SetBool("walk", false);
        anim.SetBool("spin", false);
        yield return new WaitForSeconds(1f);
        tackle = true;
    }
    IEnumerator Stop()
    {
        yield return new WaitForSeconds(1);
        Attack1 = false;
        tackle = false;
    }
    private Vector3 CalculateVelocity(Vector3 pointA, Vector3 pointB, float angle)
    {
        float rad = angle * Mathf.PI / 180;

        float x = Vector2.Distance(new Vector2(pointA.x, pointA.z), new Vector2(pointB.x, pointB.z));

        float y = pointA.y - pointB.y;

        float speed = Mathf.Sqrt(-Physics.gravity.y * Mathf.Pow(x, 2) / (2 * Mathf.Pow(Mathf.Cos(rad), 2) * (x * Mathf.Tan(rad) + y)));

        if (float.IsNaN(speed))
        {
            return Vector3.zero;
        }
        else
        {
            return (new Vector3(pointB.x - pointA.x, x * Mathf.Tan(rad), pointB.z - pointA.z).normalized * speed);
        }
    }
    IEnumerator Shot()
    {
        RushObj.SetActive(false);
            anim.SetBool("walk", false);
            anim.SetBool("spin", true);
            for (int i = 0; i < 80; i++)
            {
                GetComponent<Rigidbody2D>().AddForce(transform.up * 250);
            }
            yield return new WaitForSeconds(0.3f);
        for (int i = 0; i < 5; i++)
        {
            GameObject Bullet = Instantiate(Bulletobj, transform.position, Quaternion.identity);
            Bullet.name = i + name;
            Bullet.GetComponent<ShotBall>().Damage = float.Parse(csvDatas[1][6]);
            Bullet.GetComponent<ShotBall>().root = this.transform;
            Vector3 Targetpos = new Vector3((Player.transform.position.x + (-4+(i*2))),
                    Player.transform.position.y,
                    Player.transform.position.z);
            Vector3 velocity = CalculateVelocity(this.transform.position, Targetpos, 40);
            Rigidbody2D RigidBody = Bullet.GetComponent<Rigidbody2D>();
            RigidBody.AddForce(velocity * RigidBody.mass, ForceMode2D.Impulse);
        }
        RushObj.SetActive(true);

        //完全ランダム

        /*GameObject Bullet = Instantiate(Bulletobj, transform.position, Quaternion.identity);
        Bullet.GetComponent<ShotBall>().Damage = float.Parse(csvDatas[1][6]);
        Vector3 Targetpos = Player.transform.position;
        Vector3 velocity = CalculateVelocity(this.transform.position, Targetpos, 70);
        Rigidbody2D RigidBody = Bullet.GetComponent<Rigidbody2D>();
        RigidBody.AddForce(velocity * RigidBody.mass, ForceMode2D.Impulse);

        for (int i = 0; i < 4; i++)
        {
            Bullet = Instantiate(Bulletobj, transform.position, Quaternion.identity);
            Bullet.GetComponent<ShotBall>().Damage = float.Parse(csvDatas[1][6]);
            Targetpos = new Vector3((Player.transform.position.x + Random.Range(-3.5f, 3.5f)),
                Player.transform.position.y,
                Player.transform.position.z);
            velocity = CalculateVelocity(this.transform.position, Targetpos, 70);
            RigidBody = Bullet.GetComponent<Rigidbody2D>();
            RigidBody.AddForce(velocity * RigidBody.mass, ForceMode2D.Impulse);
        }*/

        //完全ランダム
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
                //Debug.Log("Kill");
                Destroy(this.gameObject);
                Dead = true;
            }
        }
    }
    public void Reversal()
    {
        transform.Rotate(0, 180, 0);
        Dir = Dir * -1;
        Child1.GetComponent<GroundChecker>().CheckPos = Dir;
    }
}
