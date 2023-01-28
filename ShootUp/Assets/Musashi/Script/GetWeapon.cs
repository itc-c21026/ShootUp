using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetWeapon : MonoBehaviour
{
    string[] list = { "Pistol", "Sniper", "ShotGun", "MachineGun" };
    public GameObject Player;
    public GameObject Muzzule;
    void Start()
    {
        Player = transform.root.gameObject;
        Muzzule = GameObject.Find("Muzzule"); ;
    }
    void Update()
    {
        transform.position = Player.transform.position;
        transform.rotation = Player.transform.rotation;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (collision.gameObject.tag == "Drop_Pistol")
            {
                Muzzule.GetComponent<GunController>().SubWeapn = Muzzule.GetComponent<GunController>().MainWeapon;
                Muzzule.GetComponent<GunController>().MainWeapon = list[0];
                Destroy(collision.gameObject);
            }
            else if (collision.gameObject.tag == "Drop_Sniper")
            {
                Muzzule.GetComponent<GunController>().SubWeapn = Muzzule.GetComponent<GunController>().MainWeapon;
                Muzzule.GetComponent<GunController>().MainWeapon = list[1];
                Destroy(collision.gameObject);
            }
            else if (collision.gameObject.tag == "Drop_ShotGun")
            {
                Muzzule.GetComponent<GunController>().SubWeapn = Muzzule.GetComponent<GunController>().MainWeapon;
                Muzzule.GetComponent<GunController>().MainWeapon = list[2];
                Destroy(collision.gameObject);
            }
            else if (collision.gameObject.tag == "Drop_MachineGun")
            {
                Muzzule.GetComponent<GunController>().SubWeapn = Muzzule.GetComponent<GunController>().MainWeapon;
                Muzzule.GetComponent<GunController>().MainWeapon = list[3];
                Destroy(collision.gameObject);
            }
        }
    }
}
