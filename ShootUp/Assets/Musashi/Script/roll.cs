using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roll : MonoBehaviour
{
    public GameObject Mouse;
    void Start()
    {
        
    }
    private void Update()
    {
        transform.LookAt(Mouse.transform);
    }
}
