using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss002Hp : MonoBehaviour
{
    public float HP;
    bool Dead;
    public float ReceiveDamage;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            HP -= ReceiveDamage;
            if (!Dead)
            {
                if (HP <= 0)
                {
                    //EnemyDrop.SendMessage("Drop");
                    Destroy(transform.root.gameObject);
                    GameObject score = GameObject.Find("Admin").gameObject;
                    score.GetComponent<Score>().ReceiveScore = 50;
                    score.GetComponent<Score>().Score_Plus();
                    Dead = true;
                }
            }
        }
    }
}
