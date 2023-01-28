using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*---------------------------------------
 �J�����S�ʂ̃X�N���v�g
---------------------------------------*/

public class CameraScript : MonoBehaviour
{
    // �X�N���v�g ***********************
    GunController Gunscript;
    UIScript UIsc;
    PlayerScript Pscript;
    MapScript MapSC;

    // �I�u�W�F�N�g *********************
    GameObject Player;

    // Vector3 **************************
    Vector3 defaultPos = new Vector3(0, 4.2f, -10);

    // bool *****************************
    [HideInInspector] public bool sniperAimCheck = false;

    private void Start()
    {
        Player = GameObject.Find("Player");
        Pscript = Player.GetComponent<PlayerScript>();

        Gunscript = GameObject.Find("Gun").GetComponent<GunController>();

        GameObject obj = GameObject.Find("GC");
        UIsc = obj.GetComponent<UIScript>();
        MapSC = obj.GetComponent<MapScript>();
    }

    private void Update()
    {
        // �Q�[���\����ʂ�UI��ʂŏƏ���ς���(�Ə���ς�����W[����])
        AimImageChange();

        if (Player != null)
        {
            if (Pscript.pause && !Pscript.dead)
                // �X�i�C�p�[���C�t���������̉�ʑ���(�G�C���ő勗���␳[���l�������قǋ������������Ȃ�])
                SniperAim(120f);
        }

        if (Gunscript.MainWeapon == "Sniper")
        // �X�i�C�p�[���C�t���̃G�C�������␳�I���I�t
        if (Input.GetKeyDown(KeyCode.Q)) 
            sniperAimCheck = !sniperAimCheck;
    }

    void AimImageChange()
    {
        Vector3 mousePos = Input.mousePosition;
        float border = Screen.width / 3 * 2;

        // �Ə�(�J�[�\��)�̍��W����
        if (mousePos.x < border) // x���W1280f����
        {
            UIsc.AimObj.SetActive(true);
            UIsc.AimObj.transform.position = mousePos;
            UIsc.AimObj2.SetActive(false);
            Gunscript.GunShotCheck = false;
        }
        else // x���W1280f�ȏ�
        {
            UIsc.AimObj2.SetActive(true);
            UIsc.AimObj2.transform.position = mousePos;
            UIsc.AimObj.SetActive(false);
            Gunscript.GunShotCheck = true;
        }
    }

    void SniperAim(float AimRangeMax)
    {
        Vector3 mousePos = Input.mousePosition;
        if (!MapSC.cameraBool)
        {
            if (mousePos.y > 540) // ���_�̍��W���グ�� ���X�i�C�p�[���C�t�������̎��̂�
            {
                if (Gunscript.MainWeapon == "Sniper")
                {
                    if (sniperAimCheck) // �I��
                    {
                        float pos = mousePos.y - 540;

                        this.transform.position = new Vector3(0, Player.transform.position.y + pos / AimRangeMax, -10);
                    }
                    else // �I�t
                         //�@�v���C���[���S
                        this.transform.position = new Vector3(0, Player.transform.position.y, -10);
                }
                else
                    //�@�v���C���[���S
                    this.transform.position = new Vector3(0, Player.transform.position.y, -10);

                if (this.transform.position.y < 4.2) this.transform.position = defaultPos;
            }
            else if (mousePos.y < 540) // ���_�̍��W�������� ���X�i�C�p�[���C�t�������̎��̂�
            {
                if (Gunscript.MainWeapon == "Sniper")
                {
                    if (sniperAimCheck) // �I��
                    {
                        float pos = mousePos.y - 540;

                        this.transform.position = new Vector3(0, Player.transform.position.y + pos / AimRangeMax, -10);
                    }
                    else // �I�t
                         //�@�v���C���[���S
                        this.transform.position = new Vector3(0, Player.transform.position.y, -10);
                }
                else
                    //�@�v���C���[���S
                    this.transform.position = new Vector3(0, Player.transform.position.y, -10);

                // �n�ʂ�艺�ɂ͎��_�������Ȃ�
                if (this.transform.position.y < 4.2) this.transform.position = defaultPos;
            }
            else // ���_�𒆐S�v���C���[�ɖ߂�
            {
                this.transform.position = new Vector3(0, Player.transform.position.y, -10);

                // �n�ʂ�艺�ɂ͎��_�������Ȃ�
                if (this.transform.position.y < 4.2) this.transform.position = defaultPos;
            }
        }
    }
}
