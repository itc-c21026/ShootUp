using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/*--------------------------------------------
 ドラッグ＆ドロップ全般のスクリプト
--------------------------------------------*/

public class WeaponChangeDragScript : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    // スクリプト **********************
    GunController GUNscript;

    // オブジェクト*********************
    GameObject Weapon; // メイン武器IMG
    GameObject Weapon2; // サブ武器IMG
    GameObject sWeapon; // スペシャル武器IMG

    // Vector3 *************************
    Vector3 PosReset; // 座標リセット
    Vector3 WeaponPos; // メイン武器IMG座標
    Vector3 Weapon2Pos; // サブ武器IMG座標
    Vector3 sWeaponPos; // スペシャル武器IMG座標

    // Image ***************************
    Image ImgComponent; // 掴んだオブジェクトのコンポーネント取得

    private void Start()
    {
        GUNscript = GameObject.Find("Gun").GetComponent<GunController>();

        Weapon = GameObject.Find("Weapon1");
        Weapon2 = GameObject.Find("Weapon2");
        sWeapon = GameObject.Find("sWeapon1");
    }

    private void Update()
    {
        WeaponPos = Weapon.transform.position;
        Weapon2Pos = Weapon2.transform.position;
        sWeaponPos = sWeapon.transform.position;
    }

    public void OnPointerDown(PointerEventData eventdata)
    {
        ImgComponent = GetComponent<Image>();

        PosReset = this.transform.position;
    }

    public void OnDrag(PointerEventData eventdata)
    {
        this.transform.position = eventdata.position;
    }

    public void OnPointerUp(PointerEventData eventdata)
    {
        // ドラッグ＆ドロップで武器変更(ドラッグ＆ドロップの武器変更適用範囲[横幅, 縦幅])
        WeaponChange(130f, 75f);

        this.transform.position = PosReset;
    }

    // ドラッグ＆ドロップで武器変更
    void WeaponChange(float ImgPosX, float ImgPosY)
    {
        Vector3 pos = this.transform.position;

        // メイン武器
        if (pos.x >= WeaponPos.x - ImgPosX && pos.x <= WeaponPos.x + ImgPosX 
            && pos.y >= WeaponPos.y - ImgPosY && pos.y <= WeaponPos.y + ImgPosY)
            switch (ImgComponent.tag)
            {
                case "IMGPistol":
                    if (GUNscript.SubWeapn != "Pistol") GUNscript.MainWeapon = "Pistol";
                    break;

                case "IMGSniper":
                    if (GUNscript.SubWeapn != "Sniper") GUNscript.MainWeapon = "Sniper";
                    break;

                case "IMGShotGun":
                    if (GUNscript.SubWeapn != "ShotGun") GUNscript.MainWeapon = "ShotGun";
                    break;

                case "IMGMachineGun":
                    if (GUNscript.SubWeapn != "MachineGun") GUNscript.MainWeapon = "MachineGun";
                    break;
            }

        // サブ武器
        if (pos.x >= Weapon2Pos.x - ImgPosX && pos.x <= Weapon2Pos.x + ImgPosX 
            && pos.y >= Weapon2Pos.y - ImgPosY && pos.y <= Weapon2Pos.y + ImgPosY)
            switch (ImgComponent.tag)
            {
                case "IMGPistol":
                    if (GUNscript.MainWeapon != "Pistol") GUNscript.SubWeapn = "Pistol";
                    break;

                case "IMGSniper":
                    if (GUNscript.MainWeapon != "Sniper") GUNscript.SubWeapn = "Sniper";
                    break;

                case "IMGShotGun":
                    if (GUNscript.MainWeapon != "ShotGun") GUNscript.SubWeapn = "ShotGun";
                    break;

                case "IMGMachineGun":
                    if (GUNscript.MainWeapon != "MachineGun") GUNscript.SubWeapn = "MachineGun";
                    break;
            }

        // スペシャル武器
        if (pos.x >= sWeaponPos.x - ImgPosX && pos.x <= sWeaponPos.x + ImgPosX 
            && pos.y >= sWeaponPos.y - ImgPosY + 10f && pos.y <= sWeaponPos.y + ImgPosY + 10f)
            switch (ImgComponent.tag)
            {
                case "IMGGrenade":
                    if (GUNscript.SpecialWeapon != "Grenade") GUNscript.SpecialWeapon = "Grenade";
                    break;

                case "IMGKnife":
                    if (GUNscript.SpecialWeapon != "ThrowingKnife") GUNscript.SpecialWeapon = "ThrowingKnife";
                    break;
            }
    }
}
