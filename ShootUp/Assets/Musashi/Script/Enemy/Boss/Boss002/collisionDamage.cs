using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collisionDamage : MonoBehaviour
{
    PlayerScript Pscript;
    public bool OK;
    public float Damage;
    void Start()
    {
        Pscript = GameObject.Find("Player").GetComponent<PlayerScript>();
        Damage = 0.5f;
    }

    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.name == "Body")
            {
                if (OK)
                {
                    float tmp = Damage * 2;
                    if (!Pscript.Guard)
                    {
                        Pscript.PlayerHitDamage((int)tmp);
                        Pscript.StartCoroutine("GUARD", 3.0f);
                    }
                    OK = false;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.name == "Body")
            {
                if (!OK)
                {
                    OK = true;
                }
            }
        }
    }
}
