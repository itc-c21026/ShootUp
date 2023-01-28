using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpGroundCheck : MonoBehaviour
{
    GameObject parent;
    public float PosY;
    public int CheckPos = -1;
    public float TargetY;
    void Start()
    {
        parent = transform.parent.gameObject;
    }
    void Update()
    {
        transform.position = new Vector3(parent.transform.position.x + CheckPos, parent.transform.position.y - 1, 0);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            PosY = collision.transform.position.y - parent.transform.position.y;
            TargetY = collision.transform.position.y;
            parent.SendMessage("Jump");
        }
    }
}
