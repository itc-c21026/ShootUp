using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    GameObject Player;
    PlayerScript Pscript;

    UIScript UIsc;

    public string Name;
    public float Damage;
    public float Critical;
    public float Accuracy_Count;
    public bool OK;
    float Accuracy_;
    Vector3 pos;
    Rigidbody2D rb;

    bool pose = false;

    float destroySecond;
    void Start()
    {
        Player = GameObject.Find("Player");
        Pscript = Player.GetComponent<PlayerScript>();

        UIsc = GameObject.Find("GC").GetComponent<UIScript>();

        //Invoke("destroy", 5);
        destroySecond = 5f;
        this.gameObject.name = Name;
        pos = Camera.main.WorldToScreenPoint(transform.localPosition);
        var rotation = Quaternion.LookRotation(Vector3.forward, Input.mousePosition - pos);
        transform.localRotation = rotation;
        if (Name == "ShotGun") 
        {
            Accuracy_ = (100 - Accuracy_Count) / 5;
        }
        else
        {
            Accuracy_ = (100 - Accuracy_Count) / 10;
        }
        Vector3 Accuracy = new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z + Random.Range(-Accuracy_, Accuracy_));
        transform.Rotate(Accuracy);
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.up * 50;

    }

    void Update()
    {
        if (Pscript.pause)
        {
            if (pose)
            {
                rb.velocity = transform.up * 50;
                pose = false;
            }
        }
        else
        {
            if (!pose)
            {
                rb.velocity = Vector2.zero;
                pose = true;
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
        string groundSt = collision.gameObject.name;
        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Ground")
        {
            if (!groundSt.Contains("Harf"))
            {
                GameObject obj = Instantiate(UIsc.EffectObj[1],
                        this.transform.position,
                        Quaternion.identity);
                Destroy(obj, 1f);
                Destroy(this.gameObject);
            }
        }
        if (collision.gameObject.tag == "Turret")
        {
            collision.SendMessage("HPCheck");
        }
        if (collision.gameObject.tag == "Enemy")
        {
            if (collision.gameObject.name == "Critical")
            {
                if (!OK)
                {
                    GameObject obj = Instantiate(UIsc.EffectObj[0],
                        this.transform.position,
                        Quaternion.identity);
                    Destroy(obj, 1f);
                    OK = true;
                    Destroy(this.gameObject);
                    collision.GetComponent<ReceiveDamage>().ReceiveCount = (int)Mathf.Floor(Damage * Critical);
                    collision.GetComponent<ReceiveDamage>().Receive();
                }
            }
            if (collision.gameObject.name == "Body")
            {
                if (!OK)
                {
                    GameObject obj = Instantiate(UIsc.EffectObj[0],
                        this.transform.position,
                        Quaternion.identity);
                    Destroy(obj, 1f);
                    OK = true;
                    Destroy(this.gameObject);
                    collision.GetComponent<ReceiveDamage>().ReceiveCount = (int)Damage;
                    collision.GetComponent<ReceiveDamage>().Receive();
                }
            }
        }
    }
}
