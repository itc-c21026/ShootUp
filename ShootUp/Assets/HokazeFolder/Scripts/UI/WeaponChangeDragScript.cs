using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/*--------------------------------------------
 �h���b�O���h���b�v�S�ʂ̃X�N���v�g
--------------------------------------------*/

public class WeaponChangeDragScript : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    // �X�N���v�g **********************
    GunController GUNscript;

    // �I�u�W�F�N�g*********************
    GameObject Weapon; // ���C������IMG
    GameObject Weapon2; // �T�u����IMG
    GameObject sWeapon; // �X�y�V��������IMG

    // Vector3 *************************
    Vector3 PosReset; // ���W���Z�b�g
    Vector3 WeaponPos; // ���C������IMG���W
    Vector3 Weapon2Pos; // �T�u����IMG���W
    Vector3 sWeaponPos; // �X�y�V��������IMG���W

    // Image ***************************
    Image ImgComponent; // �͂񂾃I�u�W�F�N�g�̃R���|�[�l���g�擾

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
        // �h���b�O���h���b�v�ŕ���ύX(�h���b�O���h���b�v�̕���ύX�K�p�͈�[����, �c��])
        WeaponChange(130f, 75f);

        this.transform.position = PosReset;
    }

    // �h���b�O���h���b�v�ŕ���ύX
    void WeaponChange(float ImgPosX, float ImgPosY)
    {
        Vector3 pos = this.transform.position;

        // ���C������
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

        // �T�u����
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

        // �X�y�V��������
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
