using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class TurretController : MonoBehaviour
{
    public TextAsset csvFile;
    List<string[]> csvDatas = new List<string[]>();
    //名前(0),HP(1),攻撃力(2),レート(3),発射数(4),弾速(5)

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
        while (reader.Peek() != -1) // reader.Peekが0になるまで
        {
            string line = reader.ReadLine(); // 一行ずつ読み込み
            csvDatas.Add(line.Split(',')); // , 区切りでリストに追加
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
