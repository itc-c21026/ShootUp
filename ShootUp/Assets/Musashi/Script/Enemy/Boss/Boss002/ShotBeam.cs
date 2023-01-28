using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotBeam : MonoBehaviour
{
    GameObject Player;
    PlayerScript Pscript;

    public float Damage;
    bool OK;

    private void OnEnable()
    {
        OK = false;
    }

    void Start()
    {
        Player = GameObject.Find("Player");
        Pscript = Player.GetComponent<PlayerScript>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
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
            }
        }
    }
}
