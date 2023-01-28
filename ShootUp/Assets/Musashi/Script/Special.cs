using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Special : MonoBehaviour
{

    GameObject Player;
    PlayerScript Pscript;

    public GameObject Grenade1;
    public GameObject Grenade2;
    Vector3 pos;
    bool OK;
    float CT;

    bool pose = false;

    float destroySecond;

    Rigidbody2D rb;
    void Start()
    {
        Player = GameObject.Find("Player");
        if (Player != null)
            Pscript = Player.GetComponent<PlayerScript>();

        if (name == "ThrowingKnife")
        {
            pos = Camera.main.WorldToScreenPoint(transform.localPosition);
            var rotation = Quaternion.LookRotation(Vector3.forward, Input.mousePosition - pos);
            transform.localRotation = rotation;
            rb = GetComponent<Rigidbody2D>();
            rb.velocity = transform.up * 30;
            //Invoke("destroy", 3);
            destroySecond = 3f;
        }
        else if (name == "Grenade")
        {
            pos = Camera.main.WorldToScreenPoint(transform.localPosition);
            var rotation = Quaternion.LookRotation(Vector3.forward, Input.mousePosition - pos);
            transform.localRotation = rotation;
            rb = GetComponent<Rigidbody2D>();
            rb.velocity = transform.up * 40;
            //Invoke("Explosion", 3f);
            destroySecond = 3f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (name == "ThrowingKnife")
            destroy();
        else if (name == "Grenade")
            Explosion();

        if (Pscript.pause)
        {
            if (!pose)
            {
                if (name == "ThrowingKnife")
                {
                    rb.velocity = transform.up * 30;
                    rb.isKinematic = false;
                    pose = true;
                }
                else if (name == "Grenade")
                {
                    rb.velocity = transform.up * 40;
                    rb.isKinematic = false;
                    pose = true;
                }
            }
        }
        else
        {
            if (pose)
            {
                rb.velocity = Vector2.zero;
                rb.isKinematic = true;
                pose = false;
            }
        }

        if (transform.position.z != 0)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }
    }
    public void Explosion()
    {
        if (Pscript.pause) destroySecond -= Time.deltaTime;

        if (destroySecond <= 0)
        {
            Vector3 HitPosition = transform.position;
            GameObject Gre = Instantiate(Grenade1, HitPosition, Quaternion.identity);
            Gre.name = "Grenade1";
            Gre = Instantiate(Grenade2, HitPosition, Quaternion.identity);
            Gre.name = "Grenade2";
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (name == "Grenade")
        {
            if (collision.gameObject.tag == "Enemy"|| collision.gameObject.tag == "TurretGrenade")
            {
                Gre();
            }
        }
        else if (name == "ThrowingKnife")
        {
            if (collision.gameObject.tag == "Enemy")
            {
                if (collision.gameObject.name == "Critical"|| collision.gameObject.name == "Body")
                {
                    if (!OK)
                    {
                        OK = true;
                        Destroy(this.gameObject);
                        collision.GetComponent<ReceiveDamage>().ReceiveCount = 8;
                        collision.GetComponent<ReceiveDamage>().Receive();
                    }
                }
            }
            if (collision.gameObject.tag == "Ground")
            {
                destroy();
            }
        }
    }

    void Gre()
    {
        Vector3 HitPosition = transform.position;
        GameObject Gre = Instantiate(Grenade1, HitPosition, Quaternion.identity);
        Gre.name = "Grenade1";
        Gre = Instantiate(Grenade2, HitPosition, Quaternion.identity);
        Gre.name = "Grenade2";
        Destroy(this.gameObject);
    }

    void destroy()
    {
        if (Pscript.pause)
            destroySecond -= Time.deltaTime;

        if (destroySecond <= 0)
            Destroy(this.gameObject);
    }
}
