using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    GameObject parent;
    public int CheckPos =-1;
    public bool Ground;
    void Start()
    {
        parent = transform.parent.gameObject;
    }
    void Update()
    {
        if (transform.root.name.Substring(1,1) == "2")
        {
            transform.position = new Vector3(parent.transform.position.x + CheckPos, parent.transform.position.y - 1, 0);
        }
        else
        {
            transform.position = parent.transform.position;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            Ground = true;
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            Ground = false;
            if (!Ground)
            {
                if (transform.root.name.Substring(0, 1) == "B")
                {
                    transform.root.SendMessage("Reversal");
                }
                else
                {
                    parent.SendMessage("Reversal");
                }
            }
        }
    }
}
