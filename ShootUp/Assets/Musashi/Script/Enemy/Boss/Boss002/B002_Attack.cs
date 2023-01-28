using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B002_Attack : MonoBehaviour
{
    GameObject Root;
    bool PlayerIn;
    void Start()
    {
        Root = transform.root.gameObject;
    }
    void Update()
    {
        Root.GetComponent<Boss002>().Attack = PlayerIn;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerIn = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerIn = false;
        }
    }
}
