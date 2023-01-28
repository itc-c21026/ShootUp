using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*-----------------------------------
 マップ全般のスクリプト
-----------------------------------*/

public class MapScript : MonoBehaviour
{
    // スクリプト *******************
    PlayerScript Pscript;
    CSVDataReader CSVscript;
    UIScript UIsc;
    Score ScoreSC;
    EnemyController EC;
    TurretController TC;

    // オブジェクト *****************
    GameObject[] MapPrefabs; // マップ用オブジェクトを入れる ※Resources.Load
    GameObject[] EnemyPrefabs;
    GameObject Pobj;
    GameObject FloorObj;
    GameObject CameraObj;
    GameObject BackGround;
    GameObject Player;

    // int **************************
    int BossInt; // ボス生成位置
    int bossY;
    int bossX;
    int MapCount; // マップ生成回数
    int PlayerCount; // 次マップ生成までの距離
    int ScoreCheckCount = 1; // ボスマップ生成用カウント
    int BGCount; // 背景画像生成カウント
    int NextBlock; // ブロック生成間隔
    int BlockNum400x32 = 0; // 通常ブロック
    int BlockNum200x32 = 1;
    int BlockNum800x32 = 2;
    int LBlockNum400x400 = 3; // L字
    int CrossBlockNum400x400 = 4; // 十字
    int MoveBlockNum400x32 = 5;
    int RotationCrossBlockNum400x400 = 6;
    int MoveIBlockNum32x400 = 7;
    int HalfBlockNum400x20 = 8; // 下から貫通できるブロック
    int HalfBlockNum200x20 = 9;
    int HalfBlockNum800x20 = 10;
    int DamageBlockNum400x32 = 11; // ダメージを受けるブロック
    int DamageIBlockNum32x400 = 12;
    int DamageMoveIBlockNum32x400 = 13;
    int BossWallNum = 14; // 壁(ボスマップ用)
    int NormalWallNum = 15; // 壁(通常マップ用)
    int BigFloorNum = 16; // ボス倒すまでの通行妨害ブロック

    int MapBlockInt; // マップブロック出現頻度
    int EnemyInt; // 敵出現頻度
    [HideInInspector] public int HP; // 敵HP仮格納
    [HideInInspector] int BossChange = 0; // ボス種類tmp
    [HideInInspector] public int BossClearCnt; // ボス撃破数

    int CrossBlockBoolCnt; // 十字ブロック生成間隔

    // bool *************************
    bool MapCheck = true; // マップ生成時にボスor通常の判定をする
    bool BossCheck = true; // ボスエリア生成間隔を空ける
    [HideInInspector] public bool BossClear = false; // ボス撃破時に通行妨害ブロックを削除するため
    [HideInInspector] public bool cameraBool = false; // ボスエリアカメラ固定
    bool CrossBlockBool;
    [HideInInspector] public bool tutorialClear = false;

    // float ************************
    float bossMapY = 9999; // ボス座標初期化

    private void Start()
    {
        GameObject obj = GameObject.Find("GC");
        CSVscript = obj.GetComponent<CSVDataReader>();
        UIsc = obj.GetComponent<UIScript>();

        GameObject obj2 = GameObject.Find("Admin");
        ScoreSC = obj2.GetComponent<Score>();
        EC = obj2.GetComponent<EnemyController>();
        TC = obj2.GetComponent<TurretController>();

        Player = GameObject.Find("Player");
        Pscript = Player.GetComponent<PlayerScript>();

        BackGround = Resources.Load<GameObject>("background02");
        MapPrefabs = Resources.LoadAll<GameObject>("Map");
        EnemyPrefabs = Resources.LoadAll<GameObject>("Enemy");
        Pobj = GameObject.Find("Player");
        CameraObj = GameObject.Find("Main Camera");

        MapBlockInt = 150;
        EnemyInt = 200;

        CrossBlockBool = true;

        // マップ生成
        //for (int i = 0; i < 3; i++)
        //{
        //    CreateMap();
        //}
        TutorialMap(12.0f);
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.I)) ScoreSC.ScoreCount += 10;
#endif

        if (Player != null)
        {
            if (!Pscript.dead)
            {
                if (MapCount > 4) tutorialClear = true;

                // マップ生成
                if (Pobj.transform.position.y >= (float)5 + PlayerCount * 16)
                {
                    CreateMap();
                    PlayerCount++;
                }

                // 背景生成
                if (Pobj.transform.position.y >= -30 + BGCount * 51)
                {
                    BGinst();
                    BGCount++;
                }

                // ノーマルマップ・ボスマップ生成切替
                if (ScoreSC.ScoreCount >= 200 * ScoreCheckCount)
                {
                    if (MapCheck && BossCheck)
                    {
                        MapCheck = !MapCheck;
                        BossCheck = !BossCheck;
                    }

                    float m = MapBlockInt;
                    if (MapBlockInt > 30)
                    {
                        m *= 0.8f;
                    }

                    float e = EnemyInt;
                    if (EnemyInt > 30)
                    {
                        e *= 0.8f;
                    }

                    MapBlockInt = (int)m;
                    EnemyInt = (int)e;

                    ScoreCheckCount++;
                }

                // ボス撃破時に進行妨害オブジェクト削除
                if (BossClear)
                {
                    BossClearCnt++;
                    Destroy(FloorObj);
                    cameraBool = !cameraBool;
                    BossCheck = !BossCheck;
                    BossClear = !BossClear;
                };

                // ボスエリア進入時カメラ固定&地面床生成
                if (Pobj.transform.position.y >= bossMapY - 6)
                {
                    Instantiate(MapPrefabs[BigFloorNum],
                new Vector3(-5, bossMapY - 7, 0),
                Quaternion.identity);

                    switch (BossChange)
                    {
                        case 0:
                            // ボス生成0
                            EnemyInst(0, bossX, bossY - 48, 12, CSVscript.csvDatasInt4.Count);

                            GameObject boss = GameObject.Find("Boss001(Clone)");
                            Boss001 script = boss.GetComponent<Boss001>();

                            HP = script.HP;
                            UIsc.Boss_HPbarInitialize(0, HP);
                            BossChange = 1;
                            break;
                        case 1:
                            // ボス生成1
                            EnemyInst(1, bossX, bossY - 48, 12, CSVscript.csvDatasInt4.Count);

                            GameObject boss2 = GameObject.Find("Boss002(Clone)");
                            Boss002 script2 = boss2.GetComponent<Boss002>();


                            UIsc.Boss_HPbarInitialize(1, script2.HP);
                            BossChange = 0;
                            break;
                    }

                    cameraBool = !cameraBool;
                    CameraObj.transform.position = new Vector3(0, bossMapY, -10);
                    bossMapY = 9999;
                }
            }
        }
    }

    void BGinst()
    {
        Instantiate(BackGround,
            new Vector3(-4.75f, 22.5f + BGCount * 51, 0),
            Quaternion.identity);
    }

    void CreateMap()
    {
        if (MapCheck)
            // ノーマルマップ(オブジェクト生成座標調整)
            NormalMap(12.0f, MapBlockInt);

        else
        {
            // ボスマップ(オブジェクト生成座標調整)
            BossMap(12.0f);
            StartCoroutine(UIsc.BossTxChange());
            MapCheck = !MapCheck;
        }
    }

    void NormalMap(float T, int InstLevel)
    {
        // 壁生成(ノーマルマップ用)
        GameObject obj;

        obj = Instantiate(MapPrefabs[NormalWallNum],
                            new Vector3(-14.0f, 5.0f + MapCount * 16, 0),
                            Quaternion.identity);
        obj.transform.parent = GameObject.Find("Stage").transform;

        obj = Instantiate(MapPrefabs[NormalWallNum],
                            new Vector3(4.5f, 5.0f + MapCount * 16, 0),
                            Quaternion.identity);
        obj.transform.parent = GameObject.Find("Stage").transform;

        for (int x = 0; x < 12; x++) // マップの高さ
        {
            if (CrossBlockBoolCnt >= 1)
                CrossBlockBool = true;
            else
                CrossBlockBoolCnt++;

            for (int y = 0; y <= 15; y++) // マップの横幅
            {
                if (x % 2 == 0 && NextBlock == 0 && CrossBlockBool) // プレイヤー・敵が通れる隙間を作るため
                {
                    if (x != 10)
                    {
                        int mapNum = UnityEngine.Random.Range(0, InstLevel);

                        switch (mapNum)
                        {
                            case 0: // 400px薄い床生成
                                MapBlockInst(HalfBlockNum400x20, y, x, T, 12);
                                NextBlock += 7;
                                break;

                            case 1: // 400px薄い床生成
                                MapBlockInst(HalfBlockNum400x20, y, x, T, 12);
                                NextBlock += 7;
                                break;

                            case 2: // 200px薄い床生成
                                MapBlockInst(HalfBlockNum200x20, y, x, T, 12);
                                NextBlock += 6;
                                break;

                            case 3: // 200px薄い床生成
                                MapBlockInst(HalfBlockNum200x20, y, x, T, 12);
                                NextBlock += 6;
                                break;

                            case 4: // 800px薄い床生成
                                if (y > 2 && y < 10)
                                {
                                    MapBlockInst(HalfBlockNum800x20, y, x, T, 12);
                                    NextBlock += 9;
                                }
                                break;

                            case 5: // 800px薄い床生成
                                if (y > 2 && y < 10)
                                {
                                    MapBlockInst(HalfBlockNum800x20, y, x, T, 12);
                                    NextBlock += 9;
                                }
                                break;

                            case 6: // 400px床生成
                                MapBlockInst(BlockNum400x32, y, x, T, 12);
                                NextBlock += 7;
                                break;

                            case 7: // 200px床生成
                                MapBlockInst(BlockNum200x32, y, x, T, 12);
                                NextBlock += 6;
                                break;

                            case 8: // 800px床生成
                                if (y > 2 && y < 10)
                                {
                                    MapBlockInst(BlockNum800x32, y, x, T, 12);
                                    NextBlock += 9;
                                }
                                break;

                            case 9: // 400pxダメージ床生成
                                if (ScoreCheckCount >= 2)
                                {
                                    MapBlockInst(DamageBlockNum400x32, y, x, T, 12);
                                    NextBlock += 7;
                                }
                                else
                                {
                                    MapBlockInst(BlockNum400x32, y, x, T, 12);
                                    NextBlock += 7;
                                }
                                break;

                            case 10: // 32pxダメージ壁生成
                                if (ScoreCheckCount >= 2)
                                    MapBlockInstRotate(DamageIBlockNum32x400, y, x, T, 12);
                                else
                                    MapBlockInstRotate(BlockNum400x32, y, x, T, 12);
                                break;

                            case 11: // 400px動く床生成
                                MapBlockInst(MoveBlockNum400x32, y, x, T, 12);
                                NextBlock += 7;
                                break;

                            //case 12: // 400pxL字ブロック生成
                            //    MapBlockInst(LBlockNum400x400, y, x, T, 12);
                            //    NextBlock += 7;
                            //    break;

                            case 13: // 400px動く壁ブロック生成
                                MapBlockInstRotate(MoveIBlockNum32x400, y, x, T, 12);
                                break;

                            case 14: // 32px動くダメージ壁生成
                                if (ScoreCheckCount >= 2)
                                    MapBlockInstRotate(DamageMoveIBlockNum32x400, y, x, T, 12);
                                else
                                {
                                    MapBlockInst(BlockNum400x32, y, x, T, 12);
                                    NextBlock += 7;
                                }
                                break;

                            case 15: // 400px十字ブロック生成
                                MapBlockInst(CrossBlockNum400x400, y, x, T, 12);
                                NextBlock += 7;
                                CrossBlockBool = false;
                                break;

                            case 16: // 400px回転十字ブロック生成
                                MapBlockInst(RotationCrossBlockNum400x400, y, x, T, 12);
                                NextBlock += 7;
                                CrossBlockBool = false;
                                break;
                        }
                    }
                    else
                    {
                        int mapNum = UnityEngine.Random.Range(0, InstLevel);

                        switch (mapNum)
                        {
                            case 0: // 400px薄い床生成
                                MapBlockInst(HalfBlockNum400x20, y, x, T, 12);
                                NextBlock += 7;
                                break;

                            case 1: // 400px薄い床生成
                                MapBlockInst(HalfBlockNum400x20, y, x, T, 12);
                                NextBlock += 7;
                                break;

                            case 2: // 200px薄い床生成
                                MapBlockInst(HalfBlockNum200x20, y, x, T, 12);
                                NextBlock += 6;
                                break;

                            case 3: // 200px薄い床生成
                                MapBlockInst(HalfBlockNum200x20, y, x, T, 12);
                                NextBlock += 6;
                                break;

                            case 4: // 800px薄い床生成
                                if (y > 2 && y < 10)
                                {
                                    MapBlockInst(HalfBlockNum800x20, y, x, T, 12);
                                    NextBlock += 9;
                                }
                                break;

                            case 5: // 800px薄い床生成
                                if (y > 2 && y < 10)
                                {
                                    MapBlockInst(HalfBlockNum800x20, y, x, T, 12);
                                    NextBlock += 9;
                                }
                                break;

                            case 6: // 400px床生成
                                MapBlockInst(BlockNum400x32, y, x, T, 12);
                                NextBlock += 7;
                                break;

                            case 7: // 200px床生成
                                MapBlockInst(BlockNum200x32, y, x, T, 12);
                                NextBlock += 6;
                                break;

                            case 8: // 800px床生成
                                if (y > 2 && y < 10)
                                {
                                    MapBlockInst(BlockNum800x32, y, x, T, 12);
                                    NextBlock += 9;
                                }
                                break;

                            case 9: // 400pxダメージ床生成
                                if (ScoreCheckCount >= 2)
                                {
                                    MapBlockInst(DamageBlockNum400x32, y, x, T, 12);
                                    NextBlock += 7;
                                }
                                else
                                {
                                    MapBlockInst(BlockNum400x32, y, x, T, 12);
                                    NextBlock += 7;
                                }
                                break;

                            case 10: // 32pxダメージ壁生成
                                if (ScoreCheckCount >= 2)
                                    MapBlockInstRotate(DamageIBlockNum32x400, y, x, T, 12);
                                else
                                    MapBlockInstRotate(BlockNum400x32, y, x, T, 12);
                                break;

                            case 11: // 400px動く床生成
                                MapBlockInst(MoveBlockNum400x32, y, x, T, 12);
                                NextBlock += 7;
                                break;

                            //case 12: // 400pxL字ブロック生成
                            //    MapBlockInst(LBlockNum400x400, y, x, T, 12);
                            //    NextBlock += 7;
                            //    break;

                            case 13: // 400px動く壁ブロック生成
                                MapBlockInstRotate(MoveIBlockNum32x400, y, x, T, 12);
                                break;

                            case 14: // 32px動くダメージ壁生成
                                if (ScoreCheckCount >= 2)
                                    MapBlockInstRotate(DamageMoveIBlockNum32x400, y, x, T, 12);
                                else
                                {
                                    MapBlockInst(BlockNum400x32, y, x, T, 12);
                                    NextBlock += 7;
                                }
                                break;

                            case 15: // 400px十字ブロック生成
                                MapBlockInst(CrossBlockNum400x400, y, x, T, 12);
                                NextBlock += 7;
                                CrossBlockBool = false;
                                break;

                            case 16: // 400px回転十字ブロック生成
                                MapBlockInst(RotationCrossBlockNum400x400, y, x, T, 12);
                                NextBlock += 7;
                                CrossBlockBool = false;
                                break;
                        }
                    }

                    if (MapCount >= 4)
                    {
                        float e = EnemyInt;
                        e *= 1.5f;

                        int enemyRandomHigh = Random.Range(0, EnemyInt);
                        int enemyRandomLow = Random.Range(0, (int)e);
                        int TurretRandom = Random.Range(0, EnemyInt * 5);

                        if (enemyRandomHigh >= 2 && enemyRandomHigh <= 3) // 近距離系エネミー
                            EnemyInst(enemyRandomHigh, y, x + 1, T, 12);

                        else if (enemyRandomLow >= 4 && enemyRandomLow <= 11) // 遠距離系エネミー
                            EnemyInst(enemyRandomLow, y, x + 1, T, 12);

                        else if (TurretRandom >= 12 && TurretRandom <= 17) // タレット
                        {
                            int ran = Random.Range(0, 2);
                            if (ran == 0)
                                EnemyInst(TurretRandom, 15, x + Random.Range(-1, 2), T, 12);
                            else
                                EnemyInst(TurretRandom, -1, x + Random.Range(-1, 2), T, 12);
                        }
                    }
                }

                if (NextBlock > 0) 
                    NextBlock--;
            }
        }
        MapCount++;
    }

    void BossMap(float T)
    {
        bossMapY = 5.0f + MapCount * 16;

        // 壁生成(ボスマップ用)
        GameObject obj;

        obj = Instantiate(MapPrefabs[BossWallNum],
                            new Vector3(-14.0f, 5.0f + MapCount * 16, 0),
                            Quaternion.identity);
        obj.transform.parent = GameObject.Find("Stage").transform;

        obj = Instantiate(MapPrefabs[BossWallNum],
                            new Vector3(4.5f, 5.0f + MapCount * 16, 0),
                            Quaternion.identity);
        obj.transform.parent = GameObject.Find("Stage").transform;

        // ボスマップブロック&敵生成
        for (int x = 0; x < CSVscript.csvDatasInt4.Count; x++)
        {
            for (int y = 0; y <= 15; y++)
            {
                switch (CSVscript.csvDatasInt4[x][y])
                {
                    case 1: // 薄い床生成
                            MapBlockInst(HalfBlockNum400x20, y, x, T, CSVscript.csvDatasInt4.Count);
                    break;

                    case 2: // 床生成
                            MapBlockInst(BlockNum400x32, y, x, T, CSVscript.csvDatasInt4.Count);
                    break;

                    case 4: // ボス生成位置保存
                        BossInt = CSVscript.csvDatasInt4[x][y];
                        bossX = y;
                        bossY = x;
                        break;
                }
            }
        }

        FloorObj = Instantiate(MapPrefabs[BigFloorNum],
                    new Vector3(7 - T, bossMapY + 8, 0),
                    Quaternion.identity);

        MapCount++;
    }

    void TutorialMap(float T)
    {
        // 壁生成(ノーマルマップ用)
        GameObject obj;

        obj = Instantiate(MapPrefabs[NormalWallNum],
                            new Vector3(-14.0f, 5.0f + MapCount * 16, 0),
                            Quaternion.identity);
        obj.transform.parent = GameObject.Find("Stage").transform;

        obj = Instantiate(MapPrefabs[NormalWallNum],
                            new Vector3(4.5f, 5.0f + MapCount * 16, 0),
                            Quaternion.identity);
        obj.transform.parent = GameObject.Find("Stage").transform;

        // マップブロック&敵生成
        for (int x = 0; x < CSVscript.csvDatasInt.Count; x++)
        {
            for (int y = 0; y <= 15; y++)
            {
                switch (CSVscript.csvDatasInt[x][y])
                {
                    case 1: // 薄い床生成
                        MapBlockInst(HalfBlockNum400x20, y, x, T, CSVscript.csvDatasInt.Count);
                        break;

                    case 2: // 床生成
                        MapBlockInst(BlockNum400x32, y, x, T, CSVscript.csvDatasInt.Count);
                        break;

                    case 3: // 敵生成
                        EnemyInst(12, y, x + 2, T, CSVscript.csvDatasInt.Count);
                        break;
                }
            }
        }

        MapCount++;

        // 壁生成(ノーマルマップ用)
        GameObject obj2;

        obj2 = Instantiate(MapPrefabs[NormalWallNum],
                            new Vector3(-14.0f, 5.0f + MapCount * 16, 0),
                            Quaternion.identity);
        obj2.transform.parent = GameObject.Find("Stage").transform;

        obj2 = Instantiate(MapPrefabs[NormalWallNum],
                            new Vector3(4.5f, 5.0f + MapCount * 16, 0),
                            Quaternion.identity);
        obj2.transform.parent = GameObject.Find("Stage").transform;

        // マップブロック生成
        for (int x = 0; x < CSVscript.csvDatasInt2.Count; x++)
        {
            for (int y = 0; y <= 15; y++)
            {
                switch (CSVscript.csvDatasInt2[x][y])
                {
                    case 1: // 薄い床生成
                        MapBlockInst(HalfBlockNum400x20, y, x, T, CSVscript.csvDatasInt2.Count);
                        break;

                    case 2: // 床生成
                        MapBlockInst(BlockNum400x32, y, x, T, CSVscript.csvDatasInt2.Count);
                        break;
                }
            }
        }

        MapCount++;

        // 壁生成(ノーマルマップ用)
        GameObject obj3;

        obj3 = Instantiate(MapPrefabs[NormalWallNum],
                            new Vector3(-14.0f, 5.0f + MapCount * 16, 0),
                            Quaternion.identity);
        obj3.transform.parent = GameObject.Find("Stage").transform;

        obj3 = Instantiate(MapPrefabs[NormalWallNum],
                            new Vector3(4.5f, 5.0f + MapCount * 16, 0),
                            Quaternion.identity);
        obj3.transform.parent = GameObject.Find("Stage").transform;

        // マップブロック生成
        for (int x = 0; x < CSVscript.csvDatasInt2.Count; x++)
        {
            for (int y = 0; y <= 15; y++)
            {
                switch (CSVscript.csvDatasInt2[x][y])
                {
                    case 1: // 薄い床生成
                        MapBlockInst(HalfBlockNum400x20, y, x, T, CSVscript.csvDatasInt2.Count);
                        break;

                    case 2: // 床生成
                        MapBlockInst(BlockNum400x32, y, x, T, CSVscript.csvDatasInt2.Count);
                        break;
                }
            }
        }

        MapCount++;
    }

    // マップブロック生成(種類, x座標, y座標, x座標補正, y座標生成幅)
    void MapBlockInst(int MapTmp, int forY, int forX, float T, float PosY)
    {
        Instantiate(MapPrefabs[MapTmp],
                            new Vector3(forY - T, forX + MapCount * (PosY * 1.33f), 0),
                            Quaternion.identity);
    }

    // マップブロック生成(回転付)(種類, x座標, y座標, x座標補正, y座標生成幅)
    void MapBlockInstRotate(int MapTmp, int forY, int forX, float T, float PosY)
    {
        GameObject obj = Instantiate(MapPrefabs[MapTmp],
                            new Vector3(forY - T, forX + MapCount * (PosY * 1.33f), 0),
                            Quaternion.identity);

        obj.transform.localEulerAngles = new Vector3(0, 0, 90);
    }

    // 敵生成(種類, x座標, y座標, x座標補正, y座標生成幅)
    public void EnemyInst(int EnemyTmp, int forY, int forX, float T, float PosY)
    {
        GameObject enemy = Instantiate(EnemyPrefabs[EnemyTmp],
                            new Vector3(forY - T, forX + MapCount * (PosY * 1.33f) + 0.1f, 0),
                            Quaternion.identity);

        bool a = false;

        switch (EnemyTmp)
        {
            case 2:
                a = true;
                HP = int.Parse(EC.TypeA1[1]);
                break;

            case 3:
                HP = int.Parse(EC.TypeA2[1]);
                a = true;
                break;

            case 4:
                HP = int.Parse(EC.TypeA3[1]);
                a = true;
                break;

            case 5:
                HP = int.Parse(EC.TypeB1[1]);
                a = true;
                break;

            case 6:
                HP = int.Parse(EC.TypeB2[1]);
                a = true;
                break;

            case 7:
                HP = int.Parse(EC.TypeB3[1]);
                a = true;
                break;

            case 8:
                HP = int.Parse(EC.TypeB4[1]);
                a = true;
                break;

            case 9:
                HP = int.Parse(EC.TypeB5[1]);
                a = true;
                break;

            case 10:
                HP = int.Parse(EC.TypeB6[1]);
                a = true;
                break;

            case 11:
                HP = int.Parse(EC.TypeB7[1]);
                a = true;
                break;

            case 12:
                HP = int.Parse(TC.TypeA1[1]);
                a = false;
                break;

            case 13:
                HP = int.Parse(TC.TypeA2[1]);
                a = false;
                break;

            case 14:
                HP = int.Parse(TC.TypeB1[1]);
                a = false;
                break;

            case 15:
                HP = int.Parse(TC.TypeB2[1]);
                a = false;
                break;

            case 16:
                HP = int.Parse(TC.TypeC1[1]);
                a = false;
                break;

            case 17:
                HP = int.Parse(TC.TypeC2[1]);
                a = false;
                break;
        }

        if (a)
        {
            GameObject e = enemy.transform.Find("Enemy").gameObject;
            UIsc.E_HPbarInst(e, HP, EnemyTmp);
        }
        else if (!a && EnemyTmp >= 12)
            UIsc.E_HPbarInst(enemy, HP, EnemyTmp);
    }
}
