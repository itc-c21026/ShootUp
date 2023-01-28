using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public int Damage;
    public string MyName;
    GameObject Enemy;
    private void Start()
    {
        MyName = transform.gameObject.name;
        Invoke("Destroy",0.3f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "MainEnemy")
        {
            Enemy = collision.gameObject;
            var dir = (Enemy.transform.position - transform.position).normalized;
            if (dir.y <= 0)
            {
                dir.y = 0.3f;
            }
            dir.y *= 3;
            Enemy.GetComponent<Rigidbody2D>().AddForce(dir * 3000);
        }
    }
    void Destroy()
    {
        Destroy(this.gameObject);
    }
}
