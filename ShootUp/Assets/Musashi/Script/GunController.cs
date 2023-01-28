using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class GunController : MonoBehaviour
{
    UIScript UIsc;

    AudioClipScript ACSC;

    public GameObject Player;
    PlayerScript Pscript;
    public TextAsset csvFile;
    List<string[]> csvDatas = new List<string[]>();//威力(0),クリティカル倍率(1),ノックバック(2),クールタイム(3),精度(4),弾薬数(5)

    public float[] Pistol = new float[6];
    public float[] Sniper = new float[6];
    public float[] ShotGun = new float[6];
    public float[] MachineGun = new float[6];

    public GameObject Knife;
    public GameObject Grenade;

    public GameObject Bullet;

    string[] list = { "Pistol", "Sniper", "ShotGun", "MachineGun" , "Throwingknife", "Grenade" };
    //"Pistol", "Sniper","ShotGun", "MachineGun" 
    public string MainWeapon;
    public string SubWeapn;
    public string SpecialWeapon;

    //クールタイム（レートで使用）
    float PistolCount;
    float SniperCount;
    float ShotGunCount;
    float MachineGunCount;
    float ThrowingknifeCount = 0.5f;
    float GrenadeCount;
    //クールタイム（レートで使用）

    //撃っていいかどうか
    bool PistolShot;
    bool SniperShot;
    bool ShotGunShot;
    bool MachineGunShot;
    bool ThrowingknifeShot;
    bool GrenadeShot;
    //撃っていいかどうか

    //残弾数
    public int NowPistolBulletCount;
    public int NowSniperBulletCount;
    public int NowShotGunBulletCount;
    public int NowMachineGunBulletCount;
    public int NowThrowingknifeBulletCount;
    public int NowGrenadeBulletCount;
    //残弾数

    public bool GunShotCheck = false;
    void Start()
    {
        GameObject obj = GameObject.Find("GC");
        UIsc = obj.GetComponent<UIScript>();

        GameObject obj2 = GameObject.Find("Audio");
        ACSC = obj2.GetComponent<AudioClipScript>();

        Pscript = Player.GetComponent<PlayerScript>();

        Application.targetFrameRate = 60;
        Load();
    }
    void Update()
    {

        WeaponCT();

        if (Player != null)
        {
            if (!Pscript.dead && Pscript.pause)
                if (!GunShotCheck) GunShot();

            float MouseWheel = Input.GetAxis("Mouse ScrollWheel");//マウスホイールの入力を取る
            if (MouseWheel != 0)//マウスホイールの入力があるなら武器を変える
            {
                if (SubWeapn != "") WeaponChange();
            }
        }
    }

    void GunShot()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            switch (SpecialWeapon)
            {
                case "ThrowingKnife":
                    if (NowThrowingknifeBulletCount > 0)
                    {
                        if (ThrowingknifeShot)
                        {
                            GameObject knife = Instantiate(Knife, transform.position, Quaternion.identity);
                            //Debug.Log(knife.GetComponent<Special>().name);
                            knife.GetComponent<Special>().name = "ThrowingKnife";
                            knife.GetComponent<Rigidbody2D>().gravityScale = 0.3f;
                            ThrowingknifeShot = false;
                            NowThrowingknifeBulletCount--;
                        }
                    }
                    break;
                case "Grenade":
                    if (NowGrenadeBulletCount > 0)
                    {
                        if (GrenadeShot)
                        {
                            GameObject grenade = Instantiate(Grenade, transform.position, Quaternion.identity);
                            //Debug.Log(grenade.GetComponent<Special>().name);
                            grenade.GetComponent<Special>().name = "Grenade";
                            grenade.GetComponent<Rigidbody2D>().gravityScale = 6f;
                            GrenadeShot = false;
                            NowGrenadeBulletCount--;
                        }
                    }
                    break;
            }
        }
        if (Input.GetKey(KeyCode.Mouse0))
        {
            switch (MainWeapon)
            {
                case "Pistol":
                    if (NowPistolBulletCount > 0)
                    {
                        if (PistolShot)
                        {
                            SoundController.Instance.PlaySE(ACSC.SE[0]);
                            GameObject bullet = Instantiate(Bullet, transform.position, Quaternion.identity);
                            bullet.GetComponent<Bullet>().Name = "Pistol";
                            bullet.GetComponent<Bullet>().Damage = Pistol[0];
                            bullet.GetComponent<Bullet>().Critical = Pistol[1];
                            bullet.GetComponent<Bullet>().Accuracy_Count = Pistol[4];
                            PistolShot = false;
                            NowPistolBulletCount--;
                            if (Pscript.downPcheck) Player.GetComponent<Rigidbody2D>().velocity = transform.right * -Pistol[2] * 4;
                        }
                    }
                    break;
                case "Sniper":
                    if (NowSniperBulletCount > 0)
                    {
                        if (SniperShot)
                        {
                            SoundController.Instance.PlaySE(ACSC.SE[1]);
                            GameObject bullet = Instantiate(Bullet, transform.position, Quaternion.identity);
                            bullet.GetComponent<Bullet>().Name = "Sniper";
                            bullet.GetComponent<Bullet>().Damage = Sniper[0];
                            bullet.GetComponent<Bullet>().Critical = Sniper[1];
                            bullet.GetComponent<Bullet>().Accuracy_Count = Sniper[4];
                            SniperShot = false;
                            NowSniperBulletCount--;
                            if (Pscript.downPcheck) Player.GetComponent<Rigidbody2D>().velocity = transform.right * -Sniper[2] * 4;
                        }
                    }
                    break;
                case "ShotGun":
                    if (NowShotGunBulletCount > 0)
                    {
                        if (ShotGunShot)
                        {
                            SoundController.Instance.PlaySE(ACSC.SE[2]);
                            for (int i = 0; i < 8; i++)
                            {
                                GameObject bullet = Instantiate(Bullet, transform.position, Quaternion.identity);
                                bullet.GetComponent<Bullet>().Name = "ShotGun";
                                bullet.GetComponent<Bullet>().Damage = ShotGun[0];
                                bullet.GetComponent<Bullet>().Critical = ShotGun[1];
                                bullet.GetComponent<Bullet>().Accuracy_Count = ShotGun[4];
                            }
                            ShotGunShot = false;
                            NowShotGunBulletCount--;
                            if (Pscript.downPcheck) Player.GetComponent<Rigidbody2D>().velocity = transform.right * -ShotGun[2] * 4;
                        }
                    }
                    break;
                case "MachineGun":
                    if (NowMachineGunBulletCount > 0)
                    {
                        if (MachineGunShot)
                        {
                            SoundController.Instance.PlaySE(ACSC.SE[3]);
                            GameObject bullet = Instantiate(Bullet, transform.position, Quaternion.identity);
                            bullet.GetComponent<Bullet>().Name = "MachineGun";
                            bullet.GetComponent<Bullet>().Damage = MachineGun[0];
                            bullet.GetComponent<Bullet>().Critical = MachineGun[1];
                            bullet.GetComponent<Bullet>().Accuracy_Count = MachineGun[4];
                            MachineGunShot = false;
                            NowMachineGunBulletCount--;
                            if (Pscript.downPcheck) Player.GetComponent<Rigidbody2D>().velocity = transform.right * -MachineGun[2] * 4;
                        }
                    }
                    break;
            }
        }
    }

    void Load()
    {
        //Player = transform.root.gameObject;
        csvFile = Resources.Load("Weapon") as TextAsset;
        StringReader reader = new StringReader(csvFile.text);
        while (reader.Peek() != -1) // reader.Peekが0になるまで
        {
            string line = reader.ReadLine(); // 一行ずつ読み込み
            csvDatas.Add(line.Split(',')); // , 区切りでリストに追加
        }
        for (int i = 1; i < 7; i++)
        {
            Pistol[i - 1] = float.Parse(csvDatas[1][i]);
        }
        for (int i = 1; i < 7; i++)
        {
            Sniper[i - 1] = float.Parse(csvDatas[2][i]);
        }
        for (int i = 1; i < 7; i++)
        {
            ShotGun[i - 1] = float.Parse(csvDatas[3][i]);
        }
        for (int i = 1; i < 7; i++)
        {
            MachineGun[i - 1] = float.Parse(csvDatas[4][i]);
        }
        NowPistolBulletCount = Mathf.FloorToInt(Pistol[5]);
        NowSniperBulletCount = Mathf.FloorToInt(Sniper[5]);
        NowShotGunBulletCount = Mathf.FloorToInt(ShotGun[5]);
        NowMachineGunBulletCount = Mathf.FloorToInt(MachineGun[5]);
        NowThrowingknifeBulletCount = 5;
        NowGrenadeBulletCount = 3;

        UIsc.MaxPistolBullets = NowPistolBulletCount;
        UIsc.MaxSniperBullets = NowSniperBulletCount;
        UIsc.MaxShotGunBullets = NowShotGunBulletCount;
        UIsc.MaxMachineGunBullets = NowMachineGunBulletCount;
        UIsc.MaxGrenades = NowGrenadeBulletCount;
        UIsc.MaxThrowKnifes = NowThrowingknifeBulletCount;
    }
    void WeaponCT()
    {
        if (!PistolShot)
        {
            PistolCount += Time.deltaTime;
            if (Pistol[3] <= PistolCount)
            {
                PistolCount = 0;
                PistolShot = true;
            }
        }
        if (!SniperShot)
        {
            SniperCount += Time.deltaTime;
            if (Sniper[3] <= SniperCount)
            {
                SniperCount = 0;
                SniperShot = true;
            }
        }
        if (!ShotGunShot)
        {
            ShotGunCount += Time.deltaTime;
            if (ShotGun[3] <= ShotGunCount)
            {
                ShotGunCount = 0;
                ShotGunShot = true;
            }
        }
        if (!MachineGunShot)
        {
            MachineGunCount += Time.deltaTime;
            if (MachineGun[3] <= MachineGunCount)
            {
                MachineGunCount = 0;
                MachineGunShot = true;
            }
        }
        if (!ThrowingknifeShot)
        {
            ThrowingknifeCount += Time.deltaTime;
            if (0.5f <= ThrowingknifeCount)
            {
                ThrowingknifeCount = 0;
                ThrowingknifeShot = true;
            }
        }
        if (!GrenadeShot)
        {
            GrenadeCount += Time.deltaTime;
            if (1 <= GrenadeCount)
            {
                GrenadeCount = 0;
                GrenadeShot = true;
            }
        }
    }
    void WeaponChange()
    {
        string tmp = MainWeapon;
        MainWeapon = SubWeapn;
        SubWeapn = tmp;
    }
}
