using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class EnemyController : MonoBehaviour
{
    public TextAsset csvFile;
    List<string[]> csvDatas = new List<string[]>();
    //���O(0),HP(1),�U����(2),�ړ����x(3),���[�g/����(4),�^�C�v(5),���ː�(6)

    public string[] TypeA1 = new string[7];
    public string[] TypeA2 = new string[7];
    public string[] TypeA3 = new string[7];

    public string[] TypeB1 = new string[7];
    public string[] TypeB2 = new string[7];
    public string[] TypeB3 = new string[7];
    public string[] TypeB4 = new string[7];
    public string[] TypeB5 = new string[7];
    public string[] TypeB6 = new string[7];
    public string[] TypeB7 = new string[7];

    private void Awake()
    {
        csvFile = Resources.Load("Enemy") as TextAsset;
        StringReader reader = new StringReader(csvFile.text);
        while (reader.Peek() != -1) // reader.Peek��0�ɂȂ�܂�
        {
            string line = reader.ReadLine(); // ��s���ǂݍ���
            csvDatas.Add(line.Split(',')); // , ��؂�Ń��X�g�ɒǉ�
        }
        for (int i = 0; i < 7; i++)
        {
            TypeA1[i] = csvDatas[1][i];
        }
        for (int i = 0; i < 7; i++)
        {
            TypeA2[i] = csvDatas[2][i];
        }
        for (int i = 0; i < 7; i++)
        {
            TypeA3[i] = csvDatas[3][i];
        }
        for (int i = 0; i < 7; i++)
        {
            TypeB1[i] = csvDatas[4][i];
        }
        for (int i = 0; i < 7; i++)
        {
            TypeB2[i] = csvDatas[5][i];
        }
        for (int i = 0; i < 7; i++)
        {
            TypeB3[i] = csvDatas[6][i];
        }
        for (int i = 0; i < 7; i++)
        {
            TypeB4[i] = csvDatas[7][i];
        }
        for (int i = 0; i < 7; i++)
        {
            TypeB5[i] = csvDatas[8][i];
        }
        for (int i = 0; i < 7; i++)
        {
            TypeB6[i] = csvDatas[9][i];
        }
        for (int i = 0; i < 7; i++)
        {
            TypeB7[i] = csvDatas[10][i];
        }
    }
}
