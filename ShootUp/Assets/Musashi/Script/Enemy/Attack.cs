using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public GameObject root;
    public GameObject Enemy;

    public bool enemy;

    void Start()
    {
        root = transform.root.gameObject;
        name = "Attackobj";
    }
    void Update()
    {
        transform.position = root.transform.position;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            enemy = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            enemy = false;
        }
    }
}
