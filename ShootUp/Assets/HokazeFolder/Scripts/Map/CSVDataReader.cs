using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

/*-----------------------------------------
 CSVデータを読み込むプログラム
-----------------------------------------*/

public class CSVDataReader : MonoBehaviour
{
    TextAsset[] csvFile; // CSVファイル

    public List<string[]> csvDatas = new List<string[]>(); // CSVの中身を入れるリスト;
    public List<string[]> csvDatas2 = new List<string[]>();
    //public List<string[]> csvDatas3 = new List<string[]>();
    public List<string[]> csvDatas4 = new List<string[]>();

    public List<int[]> csvDatasInt = new List<int[]>();
    public List<int[]> csvDatasInt2 = new List<int[]>();// CSVをint型に変換
    //public List<int[]> csvDatasInt3 = new List<int[]>();
    public List<int[]> csvDatasInt4 = new List<int[]>();

    string[] fileName; // マップの名前

    private void Awake()
    {
        fileName = new string[4];
        csvFile = new TextAsset[4];

        fileName[0] = "Map1";
        fileName[1] = "Map2";
        //fileName[2] = "Map3";
        fileName[3] = "MapBoss";

        csvFile[0] = Resources.Load(fileName[0]) as TextAsset; // Resouces下のCSV読み込み
        csvFile[1] = Resources.Load(fileName[1]) as TextAsset;
        //csvFile[2] = Resources.Load(fileName[2]) as TextAsset;
        csvFile[3] = Resources.Load(fileName[3]) as TextAsset;

        StringReader reader = new StringReader(csvFile[0].text);
        StringReader reader2 = new StringReader(csvFile[1].text);
        //StringReader reader3 = new StringReader(csvFile[2].text);
        StringReader reader4 = new StringReader(csvFile[3].text);

        // , で分割しつつ一行ずつ読み込み
        // リストに追加していく
        while (reader.Peek() != -1) // reader.Peaekが-1になるまで
        {
            string line = reader.ReadLine(); // 一行ずつ読み込み
            csvDatas.Add(line.Split(',')); // , 区切りでリストに追加
        }
        while (reader2.Peek() != -1) // reader.Peaekが-1になるまで
        {
            string line = reader2.ReadLine(); // 一行ずつ読み込み
            csvDatas2.Add(line.Split(',')); // , 区切りでリストに追加
        }
        //while (reader3.Peek() != -1) // reader.Peaekが-1になるまで
        //{
        //    string line = reader3.ReadLine(); // 一行ずつ読み込み
        //    csvDatas3.Add(line.Split(',')); // , 区切りでリストに追加
        //}
        while (reader4.Peek() != -1) // reader.Peaekが-1になるまで
        {
            string line = reader4.ReadLine(); // 一行ずつ読み込み
            csvDatas4.Add(line.Split(',')); // , 区切りでリストに追加
        }

        csvDatasInt = csvDatas.Select(x => x.Select(y => int.Parse(y)).ToArray()).ToList();
        csvDatasInt2 = csvDatas2.Select(x => x.Select(y => int.Parse(y)).ToArray()).ToList();
        //csvDatasInt3 = csvDatas3.Select(x => x.Select(y => int.Parse(y)).ToArray()).ToList();
        csvDatasInt4 = csvDatas4.Select(x => x.Select(y => int.Parse(y)).ToArray()).ToList();
    }
}


