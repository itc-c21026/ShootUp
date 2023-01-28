using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotBall : MonoBehaviour
{
    GameObject Player;
    PlayerScript Pscript;

    Rigidbody2D rb;

    public float Damage;
    bool OK;

    bool pose = false;

    float destroySecond;
    Vector3 targetPos;
    Vector3 velocity;

    string s;
    int i;

    public Transform root;
    void Start()
    {
        Player = GameObject.Find("Player");
        if (Player != null)
            Pscript = Player.GetComponent<PlayerScript>();

        rb = GetComponent<Rigidbody2D>();

        s = this.name.Substring(0, 1);
        i = int.Parse(s);
        targetPos = new Vector3((Player.transform.position.x + (-4 + (i * 2))),
            Player.transform.position.y,
            Player.transform.position.z);

        //Invoke("destroy", 5);
        destroySecond = 5f;
    }
    void Update()
    {
        if (Player != null)
        {
            destroy();
            if (Pscript.pause)
            {
                if (!pose)
                {
                    rb.isKinematic = false;
                    rb.AddForce(velocity * rb.mass, ForceMode2D.Impulse);
                    pose = true;
                }
            }
            else
            {
                if (pose)
                {
                    rb.isKinematic = true;
                    velocity = rb.velocity;
                    rb.velocity = Vector2.zero;
                    pose = false;
                }
            }
        }
    }
    void destroy()
    {
        if (Pscript.pause)
            destroySecond -= Time.deltaTime;

        if (destroySecond <= 0)
            Destroy(this.gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Wall" )
        {
            Destroy(this.gameObject);
        }
        if (collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.name == "Head")
            {
                if (!OK)
                {
                    float tmp = Damage * 2;
                    if (!Pscript.Guard)
                    {
                        Pscript.PlayerHitDamage((int)Mathf.Floor(tmp * 2));
                        Pscript.StartCoroutine("GUARD", 3.0f);
                    }
                    OK = true;
                    Destroy(this.gameObject);
                }
            }
            if (collision.gameObject.name == "Body")
            {
                if (!OK)
                {
                    float tmp = Damage * 2;
                    if (!Pscript.Guard)
                    {
                        Pscript.PlayerHitDamage((int)tmp);
                        Pscript.StartCoroutine("GUARD", 3.0f);
                    }
                    OK = true;
                    Destroy(this.gameObject);
                }
            }
        }
    }
}
