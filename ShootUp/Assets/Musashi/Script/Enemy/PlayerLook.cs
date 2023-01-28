using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    GameObject player;
    PlayerScript Pscript;
    void Start()
    {
        player = GameObject.Find("Player");
        Pscript = player.GetComponent<PlayerScript>();
    }
    void Update()
    {
        if (player != null)
        transform.LookAt(player.transform);
    }
}
