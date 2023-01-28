using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*-----------------------------------------
 �e����莞�̋����̃v���O����
-----------------------------------------*/

public class BulletUIScript : MonoBehaviour
{
    [HideInInspector] public GameObject target;
    [HideInInspector] public GameObject targetS;

    EnemyDrop EDscript;
    GunController Gunscript;

    int i = 0;
    private void Start()
    {
        EDscript = GameObject.Find("Admin").GetComponent<EnemyDrop>();
        Gunscript = GameObject.Find("Gun").GetComponent<GunController>();

        // �e�̎�ނŃ^�[�Q�b�g��ς���
        switch (EDscript.Rando)
        {
            case 0:
                target = GameObject.Find("PisAmmo");
                break;
            case 1:
                target = GameObject.Find("SniAmmo");
                break;
            case 2:
                target = GameObject.Find("ShotAmmo");
                break;
            case 3:
                target = GameObject.Find("SmgAmmo");
                break;
        }

        // �e�̎�ނŃ^�[�Q�b�g��ς���
        switch (EDscript.Ran)
        {
            case 0:
                targetS = GameObject.Find("Granade");
                break;
            case 1:
                targetS = GameObject.Find("ThrowKnife");
                break;
        }
    }

    void Update()
    {
        Target();
    }

    // �^�[�Q�b�g�Ɍ������Ēe����΂�
    void Target()
    {
        if (this.gameObject.name == "WeaponAmmo")
        {
            // �^�[�Q�b�g�ɕ�����������
            Vector3 diff = (target.transform.position - this.transform.position).normalized;
            this.transform.rotation = Quaternion.FromToRotation(Vector3.up, diff);

            // y���Ɉړ�������
            var velocity = Vector3.zero;
            velocity.y = 30.0f;
            this.transform.position += transform.rotation * velocity;

            i = Screen.width - 200;

            // �e�򂪃^�[�Q�b�g�̍��W�ɓ��B�����ꍇ
            if (this.transform.position.x >= i)
            {
                switch (target.name)
                {
                    case "PisAmmo":
                        Gunscript.NowPistolBulletCount += EDscript.Pistol;
                        break;

                    case "SniAmmo":
                        Gunscript.NowSniperBulletCount += EDscript.Sniper;
                        break;

                    case "ShotAmmo":
                        Gunscript.NowShotGunBulletCount += EDscript.ShotGun;
                        break;

                    case "SmgAmmo":
                        Gunscript.NowMachineGunBulletCount += EDscript.MachineGun;
                        break;
                }

                for (int i = 0; i < 10; i++)
                {
                    if (EDscript.BulletUIs[i] == this.gameObject)
                        EDscript.BulletUIs[i] = null;
                }

                Destroy(this.gameObject);
            }
        }
        else if (this.gameObject.name == "SpecialAmmo")
        {
            if (targetS == null) Destroy(this.gameObject);
            Vector3 diff = (targetS.transform.position - this.transform.position).normalized;
            this.transform.rotation = Quaternion.FromToRotation(Vector3.up, diff);

            var velocity = Vector3.zero;
            velocity.y = 30.0f;
            this.transform.position += transform.rotation * velocity;

            i = Screen.width - 200;

            if (this.transform.position.x >= i)
            {
                switch (targetS.name)
                {
                    case "Granade":
                        Gunscript.NowGrenadeBulletCount += EDscript.Grenade;
                        break;

                    case "ThrowKnife":
                        Gunscript.NowThrowingknifeBulletCount += EDscript.Throwingknife;
                        break;
                }

                for (int i = 0; i < 10; i++)
                {
                    if (EDscript.BulletUIs[i] == this.gameObject)
                        EDscript.BulletUIs[i] = null;
                }

                Destroy(this.gameObject);
            }
        }
    }
}
