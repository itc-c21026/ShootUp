using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeReceive : MonoBehaviour
{
    public bool GreOK = true;
    float CT;
    public GameObject Enemy;
    void Start()
    {

    }
    void Update()
    {
        if (!GreOK)
        {
            CT += Time.deltaTime;
            if (CT >= 1f)
            {
                GreOK = true;
                CT = 0;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GreOK)
        {
            if (collision.gameObject.name == "Grenade1")
            {
                GreOK = false;
                Enemy.SendMessage("Grenade1");
            }
            else if (collision.gameObject.name == "Grenade2")
            {
                GreOK = false;
                Enemy.SendMessage("Grenade2");
            }
        }
    }
}
