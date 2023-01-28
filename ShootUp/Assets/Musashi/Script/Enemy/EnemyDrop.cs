using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDrop : MonoBehaviour
{
    PlayerScript Pscript;

    Camera TargetCamera; //
    public GameObject[] BulletUIs; //

    GameObject BulletUI; //
    GameObject GunController;
    GameObject LifeSC;
    public string DropName;

    public int Pistol = 50;
    public int Sniper = 10;
    public int ShotGun = 15;
    public int MachineGun = 150;
    public int Throwingknife = 5;
    public int Grenade = 3;

    public int Rando; //
    [HideInInspector] public int MaxRando; //
    [HideInInspector] public int Ran;
    void Start()
    {
        if (TargetCamera == null) TargetCamera = Camera.main;

        LifeSC = GameObject.Find("GC");
        GameObject obj = GameObject.Find("Player");
        Pscript = obj.GetComponent<PlayerScript>();

        BulletUIs = new GameObject[20];

        BulletUI = (GameObject)Resources.Load("BulletUIAnim");
        GunController = GameObject.Find("Gun");
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U)) StartCoroutine("LifeSetClear2");
    }
    void PistolDrop()
    {
        //GunController.GetComponent<GunController>().NowPistolBulletCount += Pistol;
        //Debug.Log("ピストル");
        Rando = 0;
    }
    void SniperDrop()
    {
        //GunController.GetComponent<GunController>().NowSniperBulletCount += Sniper;
        //Debug.Log("スナイパー");
        Rando = 1;
    }
    void ShotGunDrop()
    {
        //GunController.GetComponent<GunController>().NowShotGunBulletCount += ShotGun;
        //Debug.Log("ショットガン");
        Rando = 2;
    }
    void MachineGunDrop()
    {
        //GunController.GetComponent<GunController>().NowMachineGunBulletCount += MachineGun;
        //Debug.Log("マシンガン");
        Rando = 3;
    }
    public void Drop()
    {
        Rando = Random.Range(0, MaxRando); //
        switch (Rando)
        {
            case 0:
                //GunController.GetComponent<GunController>().NowPistolBulletCount += Pistol;
                //.Log("ピストル");
                break;
            case 1:
                //GunController.GetComponent<GunController>().NowSniperBulletCount += Sniper;
                //Debug.Log("スナイパー");
                break;
            case 2:
                //GunController.GetComponent<GunController>().NowShotGunBulletCount += ShotGun;
                //Debug.Log("ショットガン");
                break;
            case 3:
                //GunController.GetComponent<GunController>().NowMachineGunBulletCount += MachineGun;
                //Debug.Log("マシンガン");
                break;
        }
        Ran = Random.Range(0, 100);
        if (Ran <= 20)
        {
            Ran = Random.Range(0, 2);
            if (Ran == 0)
            {
                //GunController.GetComponent<GunController>().NowGrenadeBulletCount += Grenade;
                //Debug.Log("グレ");
                Pscript.HP += 3;
                if (Pscript.HP > 10) Pscript.HP = 10;
                StartCoroutine("LifeSetClear2");
                LifeSC.SendMessage("AllClear");
                LifeSC.SendMessage("LifeSet");
            }
            else if (Ran == 1)
            {
                //GunController.GetComponent<GunController>().NowThrowingknifeBulletCount += Throwingknife;
                //Debug.Log("ナイフ");
                Pscript.HP += 4;
                if (Pscript.HP > 10) Pscript.HP = 10;
                StartCoroutine("LifeSetClear2");
                LifeSC.SendMessage("AllClear");
                LifeSC.SendMessage("LifeSet");
            }
        }
    }

    public void BossDrop()
    {
        Rando = Random.Range(0, MaxRando);
        Ran = Random.Range(0, 2);

        Pscript.HP += 8;
        if (Pscript.HP > 10) Pscript.HP = 10;
        StartCoroutine("LifeSetClear2");
        LifeSC.SendMessage("AllClear");
        LifeSC.SendMessage("LifeSet");
    }

    IEnumerator LifeSetClear2()
    {
        var second = new WaitForSeconds(0.5f);
        LifeSC.SendMessage("LifeSet2");
        yield return second;
        LifeSC.SendMessage("AllClear2");
    }

    public void BulletUIinst(Transform ObjT, string ObjName) //////////////
    {
        for (int i = 0; i < 10; i++)
        {
            if (BulletUIs[i] == null)
            {
                BulletUIs[i] = Instantiate(BulletUI, new Vector3(0, 0, 0), Quaternion.identity);

                BulletUIs[i].name = ObjName;

                BulletUIs[i].transform.parent = GameObject.Find("UI").transform;

                BulletUIs[i].transform.position = RectTransformUtility.WorldToScreenPoint(TargetCamera, ObjT.transform.position);

                i = 10;
            }
        }
    }
}
