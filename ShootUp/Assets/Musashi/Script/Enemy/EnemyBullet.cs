using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    GameObject Player;
    PlayerScript Pscript;
    public float Damage;
    public float Accuracy_Count;
    Rigidbody2D Rb;
    float Critical = 2;
    bool OK;
    float Accuracy_;

    bool pose = false;

    float destroySecond;
    void Start()
    {
        Player = GameObject.Find("Player");
        Pscript = Player.GetComponent<PlayerScript>();

        if (name == "EnemyShotGun")
        {
            Accuracy_ = (100 - Accuracy_Count) / 5;
        }
        if (name == "Missile")
        {
            Rb = GetComponent<Rigidbody2D>();
            //Invoke("destroy", 3);
            destroySecond = 3f;
        }
        else
        {
            Vector3 Accuracy = new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z + Random.Range(-Accuracy_, Accuracy_));
            transform.Rotate(Accuracy);
            GetComponent<Rigidbody2D>().velocity = transform.right * -10;
            //Invoke("destroy", 5);
            destroySecond = 5f;
        }
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
                    Rb.AddForce(target.normalized * 10);

                    float speedXtmp = Mathf.Clamp(Rb.velocity.x, -3f, 3f);
                    float speedYtmp = Mathf.Clamp(Rb.velocity.y, -3f, 3f);
                    Rb.velocity = new Vector3(speedXtmp, speedYtmp);
                }
                else if (!pose)
                {
                    GetComponent<Rigidbody2D>().velocity = transform.right * -10;
                    pose = true;
                }
            }
            else
            {
                if (name == "Missile")
                    Rb.velocity = Vector2.zero;
                else
                {
                    GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    pose = false;
                }
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
