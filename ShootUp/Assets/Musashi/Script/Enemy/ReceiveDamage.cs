using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceiveDamage : MonoBehaviour
{
    public GameObject root;
    public GameObject Enemy;
    public int ReceiveCount;
    public bool Critical;
    public int HP;
    void Start()
    {
        this.gameObject.tag = "Enemy";
        if (!Critical)
        {
            this.gameObject.name = "Body";
        }
        else
        {
            this.gameObject.name = "Critical";
        }
    }
    void Update()
    {
        if (transform.root.name.Substring(0, 1) != "B")
        {
            transform.position = root.transform.position;
        }
    }
    public void Receive()
    {
        //if (HP >= 0)
        //{
        if (name == "Body")
        {
            if (root.name.Substring(0, 1) != "T")
            {
                Enemy.SendMessage("BCheck");
            }
            Enemy.SendMessage("HPCheck");
        }
        else if (name == "Critical")
        {
            Enemy.SendMessage("CCheck");
            Enemy.SendMessage("HPCheck");
        }
        //}
    }
}
