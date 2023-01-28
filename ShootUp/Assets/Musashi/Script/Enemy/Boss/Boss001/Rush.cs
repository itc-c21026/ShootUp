using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rush : MonoBehaviour
{
    bool Attack;
    public bool Type;
    void Start()
    {
        if (name == "Rush") Type = true;
    }
    void Update()
    {
        Attack = transform.root.GetComponent<Boss001>().Attack1;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Type)
        {
            if (collision.tag == "Player")
            {
                transform.root.SendMessage("Rush");
            }
        }
        else
        {
            if (Attack)
            {
                if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "StealthWall")
                {
                    transform.root.GetComponent<Boss001>().StartCoroutine("Stop");
                }
            }
        }
    }

}
