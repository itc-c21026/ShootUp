using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class TurretController : MonoBehaviour
{
    public TextAsset csvFile;
    List<string[]> csvDatas = new List<string[]>();
    //���O(0),HP(1),�U����(2),���[�g(3),���ː�(4),�e��(5)

    public string[] TypeA1 = new string[6];
    public string[] TypeA2 = new string[6];

    public string[] TypeB1 = new string[6];
    public string[] TypeB2 = new string[6];

    public string[] TypeC1 = new string[6];
    public string[] TypeC2 = new string[6];
    private void Awake()
    {
        csvFile = Resources.Load("Turret") as TextAsset;
        StringReader reader = new StringReader(csvFile.text);
        while (reader.Peek() != -1) // reader.Peek��0�ɂȂ�܂�
        {
            string line = reader.ReadLine(); // ��s���ǂݍ���
            csvDatas.Add(line.Split(',')); // , ��؂�Ń��X�g�ɒǉ�
        }
        for (int i = 0; i < 6; i++)
        {
            TypeA1[i] = csvDatas[1][i];
        }
        for (int i = 0; i < 6; i++)
        {
            TypeA2[i] = csvDatas[2][i];
        }
        for (int i = 0; i < 6; i++)
        {
            TypeB1[i] = csvDatas[3][i];
        }
        for (int i = 0; i < 6; i++)
        {
            TypeB2[i] = csvDatas[4][i];
        }
        for (int i = 0; i < 6; i++)
        {
            TypeC1[i] = csvDatas[5][i];
        }
        for (int i = 0; i < 6; i++)
        {
            TypeC2[i] = csvDatas[6][i];
        }
    }
}
