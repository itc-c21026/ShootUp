using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBullet : MonoBehaviour
{
    GameObject Player;
    PlayerScript Pscript;
    Rigidbody2D Rb;
    public float Damage;
    public float Speed;
    float Critical = 2;
    bool OK;

    bool pose = false;
    float destroySecond;
    void Start()
    {
        Player = GameObject.Find("Player");

        if (Player != null)
        Pscript = Player.GetComponent<PlayerScript>();

        Rb = GetComponent<Rigidbody2D>();
        if (name == "Missile")
            Rb.velocity = transform.right * -Speed*2;
        else
            Rb.velocity = transform.right * -Speed;

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
                if (name == "Missile")
                {
                    transform.rotation = Quaternion.LookRotation(Player.transform.position - transform.position, Vector3.up)
                        * Quaternion.FromToRotation(Vector3.forward, Vector3.right);

                    Vector3 target = Player.transform.position - transform.position;
                    Rb.AddForce(target.normalized * 200);
                    Rb.velocity = Vector3.zero;
                    float speedXtmp = Mathf.Clamp(Rb.velocity.x, -2, 2);
                    float speedYtmp = Mathf.Clamp(Rb.velocity.y, -2, 2);
                    Rb.velocity = new Vector3(speedXtmp, speedYtmp);
                }
                else if (!pose)
                {
                    Rb.velocity = transform.right * -Speed;
                    pose = true;
                }
            }
            else
            {
                Rb.velocity = Vector2.zero;
                if (name != "Missile")
                    pose = false;
            }
        }

        if (transform.position.z != 0)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
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
        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Ground")
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
                        Pscript.PlayerHitDamage((int)Mathf.Floor(tmp * Critical));
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
