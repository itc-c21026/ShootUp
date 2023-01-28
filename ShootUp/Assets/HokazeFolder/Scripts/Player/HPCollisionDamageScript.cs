using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*---------------------------------------
 * �v���C���[���G�ƐڐG�����Ƃ��̃v���O����
 --------------------------------------*/

public class HPCollisionDamageScript : MonoBehaviour
{
    PlayerScript Pscript;

    string targetSt;
    string st;
    string s;
    int i;
    float tmp;

    private void Start()
    {
        Pscript = GameObject.Find("Player").GetComponent<PlayerScript>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MainEnemy")) // "Enemy"�^�O���t�����I�u�W�F�N�g�ɐڐG�����ꍇ
        {
            GameObject enemy = collision.gameObject.transform.Find("Enemy").gameObject;

            // �G�̖��O���琔�����擾���ē��肷��
            targetSt = collision.gameObject.name.Substring(1, 1);

            if (targetSt == "1")
                st = collision.gameObject.name.Substring(3, 1);
            else
                st = "4";

            switch (st)
            {
                case "1":
                    s = enemy.GetComponent<E101>().Mystatus[2];
                    tmp = float.Parse(s) * 2;
                    i = (int)tmp;
                    break;

                case "2":
                    s = enemy.GetComponent<E102>().Mystatus[2];
                    tmp = float.Parse(s) * 2;
                    i = (int)tmp;
                    break;

                case "3":
                    s = enemy.GetComponent<E103>().Mystatus[2];
                    tmp = float.Parse(s) * 2;
                    i = (int)tmp;
                    break;

                case "4":
                    i = 1;
                    break;
            }

            // �v���C���[�����G����Ȃ��ꍇ�_���[�W���󂯂�
            if (Pscript.HP > 0 && !Pscript.Guard)
            {
                Pscript.PlayerHitDamage(i);
                Pscript.StartCoroutine("GUARD", 3.0f);
            }
        }

        // �_���[�W���󂯂�u���b�N
        string s2 = collision.gameObject.name.Substring(0, 1);
        if (s2 == "3" || s2 == "4")
            if (Pscript.HP > 0 && !Pscript.Guard)
            {
                Pscript.PlayerHitDamage(1);
                Pscript.StartCoroutine("GUARD", 3.0f);
            }
    }
}
