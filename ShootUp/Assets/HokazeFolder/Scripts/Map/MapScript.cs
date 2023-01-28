using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*-----------------------------------
 �}�b�v�S�ʂ̃X�N���v�g
-----------------------------------*/

public class MapScript : MonoBehaviour
{
    // �X�N���v�g *******************
    PlayerScript Pscript;
    CSVDataReader CSVscript;
    UIScript UIsc;
    Score ScoreSC;
    EnemyController EC;
    TurretController TC;

    // �I�u�W�F�N�g *****************
    GameObject[] MapPrefabs; // �}�b�v�p�I�u�W�F�N�g������ ��Resources.Load
    GameObject[] EnemyPrefabs;
    GameObject Pobj;
    GameObject FloorObj;
    GameObject CameraObj;
    GameObject BackGround;
    GameObject Player;

    // int **************************
    int BossInt; // �{�X�����ʒu
    int bossY;
    int bossX;
    int MapCount; // �}�b�v������
    int PlayerCount; // ���}�b�v�����܂ł̋���
    int ScoreCheckCount = 1; // �{�X�}�b�v�����p�J�E���g
    int BGCount; // �w�i�摜�����J�E���g
    int NextBlock; // �u���b�N�����Ԋu
    int BlockNum400x32 = 0; // �ʏ�u���b�N
    int BlockNum200x32 = 1;
    int BlockNum800x32 = 2;
    int LBlockNum400x400 = 3; // L��
    int CrossBlockNum400x400 = 4; // �\��
    int MoveBlockNum400x32 = 5;
    int RotationCrossBlockNum400x400 = 6;
    int MoveIBlockNum32x400 = 7;
    int HalfBlockNum400x20 = 8; // ������ђʂł���u���b�N
    int HalfBlockNum200x20 = 9;
    int HalfBlockNum800x20 = 10;
    int DamageBlockNum400x32 = 11; // �_���[�W���󂯂�u���b�N
    int DamageIBlockNum32x400 = 12;
    int DamageMoveIBlockNum32x400 = 13;
    int BossWallNum = 14; // ��(�{�X�}�b�v�p)
    int NormalWallNum = 15; // ��(�ʏ�}�b�v�p)
    int BigFloorNum = 16; // �{�X�|���܂ł̒ʍs�W�Q�u���b�N

    int MapBlockInt; // �}�b�v�u���b�N�o���p�x
    int EnemyInt; // �G�o���p�x
    [HideInInspector] public int HP; // �GHP���i�[
    [HideInInspector] int BossChange = 0; // �{�X���tmp
    [HideInInspector] public int BossClearCnt; // �{�X���j��

    int CrossBlockBoolCnt; // �\���u���b�N�����Ԋu

    // bool *************************
    bool MapCheck = true; // �}�b�v�������Ƀ{�Xor�ʏ�̔��������
    bool BossCheck = true; // �{�X�G���A�����Ԋu���󂯂�
    [HideInInspector] public bool BossClear = false; // �{�X���j���ɒʍs�W�Q�u���b�N���폜���邽��
    [HideInInspector] public bool cameraBool = false; // �{�X�G���A�J�����Œ�
    bool CrossBlockBool;
    [HideInInspector] public bool tutorialClear = false;

    // float ************************
    float bossMapY = 9999; // �{�X���W������

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

        // �}�b�v����
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

                // �}�b�v����
                if (Pobj.transform.position.y >= (float)5 + PlayerCount * 16)
                {
                    CreateMap();
                    PlayerCount++;
                }

                // �w�i����
                if (Pobj.transform.position.y >= -30 + BGCount * 51)
                {
                    BGinst();
                    BGCount++;
                }

                // �m�[�}���}�b�v�E�{�X�}�b�v�����ؑ�
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

                // �{�X���j���ɐi�s�W�Q�I�u�W�F�N�g�폜
                if (BossClear)
                {
                    BossClearCnt++;
                    Destroy(FloorObj);
                    cameraBool = !cameraBool;
                    BossCheck = !BossCheck;
                    BossClear = !BossClear;
                };

                // �{�X�G���A�i�����J�����Œ�&�n�ʏ�����
                if (Pobj.transform.position.y >= bossMapY - 6)
                {
                    Instantiate(MapPrefabs[BigFloorNum],
                new Vector3(-5, bossMapY - 7, 0),
                Quaternion.identity);

                    switch (BossChange)
                    {
                        case 0:
                            // �{�X����0
                            EnemyInst(0, bossX, bossY - 48, 12, CSVscript.csvDatasInt4.Count);

                            GameObject boss = GameObject.Find("Boss001(Clone)");
                            Boss001 script = boss.GetComponent<Boss001>();

                            HP = script.HP;
                            UIsc.Boss_HPbarInitialize(0, HP);
                            BossChange = 1;
                            break;
                        case 1:
                            // �{�X����1
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
            // �m�[�}���}�b�v(�I�u�W�F�N�g�������W����)
            NormalMap(12.0f, MapBlockInt);

        else
        {
            // �{�X�}�b�v(�I�u�W�F�N�g�������W����)
            BossMap(12.0f);
            StartCoroutine(UIsc.BossTxChange());
            MapCheck = !MapCheck;
        }
    }

    void NormalMap(float T, int InstLevel)
    {
        // �ǐ���(�m�[�}���}�b�v�p)
        GameObject obj;

        obj = Instantiate(MapPrefabs[NormalWallNum],
                            new Vector3(-14.0f, 5.0f + MapCount * 16, 0),
                            Quaternion.identity);
        obj.transform.parent = GameObject.Find("Stage").transform;

        obj = Instantiate(MapPrefabs[NormalWallNum],
                            new Vector3(4.5f, 5.0f + MapCount * 16, 0),
                            Quaternion.identity);
        obj.transform.parent = GameObject.Find("Stage").transform;

        for (int x = 0; x < 12; x++) // �}�b�v�̍���
        {
            if (CrossBlockBoolCnt >= 1)
                CrossBlockBool = true;
            else
                CrossBlockBoolCnt++;

            for (int y = 0; y <= 15; y++) // �}�b�v�̉���
            {
                if (x % 2 == 0 && NextBlock == 0 && CrossBlockBool) // �v���C���[�E�G���ʂ�錄�Ԃ���邽��
                {
                    if (x != 10)
                    {
                        int mapNum = UnityEngine.Random.Range(0, InstLevel);

                        switch (mapNum)
                        {
                            case 0: // 400px����������
                                MapBlockInst(HalfBlockNum400x20, y, x, T, 12);
                                NextBlock += 7;
                                break;

                            case 1: // 400px����������
                                MapBlockInst(HalfBlockNum400x20, y, x, T, 12);
                                NextBlock += 7;
                                break;

                            case 2: // 200px����������
                                MapBlockInst(HalfBlockNum200x20, y, x, T, 12);
                                NextBlock += 6;
                                break;

                            case 3: // 200px����������
                                MapBlockInst(HalfBlockNum200x20, y, x, T, 12);
                                NextBlock += 6;
                                break;

                            case 4: // 800px����������
                                if (y > 2 && y < 10)
                                {
                                    MapBlockInst(HalfBlockNum800x20, y, x, T, 12);
                                    NextBlock += 9;
                                }
                                break;

                            case 5: // 800px����������
                                if (y > 2 && y < 10)
                                {
                                    MapBlockInst(HalfBlockNum800x20, y, x, T, 12);
                                    NextBlock += 9;
                                }
                                break;

                            case 6: // 400px������
                                MapBlockInst(BlockNum400x32, y, x, T, 12);
                                NextBlock += 7;
                                break;

                            case 7: // 200px������
                                MapBlockInst(BlockNum200x32, y, x, T, 12);
                                NextBlock += 6;
                                break;

                            case 8: // 800px������
                                if (y > 2 && y < 10)
                                {
                                    MapBlockInst(BlockNum800x32, y, x, T, 12);
                                    NextBlock += 9;
                                }
                                break;

                            case 9: // 400px�_���[�W������
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

                            case 10: // 32px�_���[�W�ǐ���
                                if (ScoreCheckCount >= 2)
                                    MapBlockInstRotate(DamageIBlockNum32x400, y, x, T, 12);
                                else
                                    MapBlockInstRotate(BlockNum400x32, y, x, T, 12);
                                break;

                            case 11: // 400px����������
                                MapBlockInst(MoveBlockNum400x32, y, x, T, 12);
                                NextBlock += 7;
                                break;

                            //case 12: // 400pxL���u���b�N����
                            //    MapBlockInst(LBlockNum400x400, y, x, T, 12);
                            //    NextBlock += 7;
                            //    break;

                            case 13: // 400px�����ǃu���b�N����
                                MapBlockInstRotate(MoveIBlockNum32x400, y, x, T, 12);
                                break;

                            case 14: // 32px�����_���[�W�ǐ���
                                if (ScoreCheckCount >= 2)
                                    MapBlockInstRotate(DamageMoveIBlockNum32x400, y, x, T, 12);
                                else
                                {
                                    MapBlockInst(BlockNum400x32, y, x, T, 12);
                                    NextBlock += 7;
                                }
                                break;

                            case 15: // 400px�\���u���b�N����
                                MapBlockInst(CrossBlockNum400x400, y, x, T, 12);
                                NextBlock += 7;
                                CrossBlockBool = false;
                                break;

                            case 16: // 400px��]�\���u���b�N����
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
                            case 0: // 400px����������
                                MapBlockInst(HalfBlockNum400x20, y, x, T, 12);
                                NextBlock += 7;
                                break;

                            case 1: // 400px����������
                                MapBlockInst(HalfBlockNum400x20, y, x, T, 12);
                                NextBlock += 7;
                                break;

                            case 2: // 200px����������
                                MapBlockInst(HalfBlockNum200x20, y, x, T, 12);
                                NextBlock += 6;
                                break;

                            case 3: // 200px����������
                                MapBlockInst(HalfBlockNum200x20, y, x, T, 12);
                                NextBlock += 6;
                                break;

                            case 4: // 800px����������
                                if (y > 2 && y < 10)
                                {
                                    MapBlockInst(HalfBlockNum800x20, y, x, T, 12);
                                    NextBlock += 9;
                                }
                                break;

                            case 5: // 800px����������
                                if (y > 2 && y < 10)
                                {
                                    MapBlockInst(HalfBlockNum800x20, y, x, T, 12);
                                    NextBlock += 9;
                                }
                                break;

                            case 6: // 400px������
                                MapBlockInst(BlockNum400x32, y, x, T, 12);
                                NextBlock += 7;
                                break;

                            case 7: // 200px������
                                MapBlockInst(BlockNum200x32, y, x, T, 12);
                                NextBlock += 6;
                                break;

                            case 8: // 800px������
                                if (y > 2 && y < 10)
                                {
                                    MapBlockInst(BlockNum800x32, y, x, T, 12);
                                    NextBlock += 9;
                                }
                                break;

                            case 9: // 400px�_���[�W������
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

                            case 10: // 32px�_���[�W�ǐ���
                                if (ScoreCheckCount >= 2)
                                    MapBlockInstRotate(DamageIBlockNum32x400, y, x, T, 12);
                                else
                                    MapBlockInstRotate(BlockNum400x32, y, x, T, 12);
                                break;

                            case 11: // 400px����������
                                MapBlockInst(MoveBlockNum400x32, y, x, T, 12);
                                NextBlock += 7;
                                break;

                            //case 12: // 400pxL���u���b�N����
                            //    MapBlockInst(LBlockNum400x400, y, x, T, 12);
                            //    NextBlock += 7;
                            //    break;

                            case 13: // 400px�����ǃu���b�N����
                                MapBlockInstRotate(MoveIBlockNum32x400, y, x, T, 12);
                                break;

                            case 14: // 32px�����_���[�W�ǐ���
                                if (ScoreCheckCount >= 2)
                                    MapBlockInstRotate(DamageMoveIBlockNum32x400, y, x, T, 12);
                                else
                                {
                                    MapBlockInst(BlockNum400x32, y, x, T, 12);
                                    NextBlock += 7;
                                }
                                break;

                            case 15: // 400px�\���u���b�N����
                                MapBlockInst(CrossBlockNum400x400, y, x, T, 12);
                                NextBlock += 7;
                                CrossBlockBool = false;
                                break;

                            case 16: // 400px��]�\���u���b�N����
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

                        if (enemyRandomHigh >= 2 && enemyRandomHigh <= 3) // �ߋ����n�G�l�~�[
                            EnemyInst(enemyRandomHigh, y, x + 1, T, 12);

                        else if (enemyRandomLow >= 4 && enemyRandomLow <= 11) // �������n�G�l�~�[
                            EnemyInst(enemyRandomLow, y, x + 1, T, 12);

                        else if (TurretRandom >= 12 && TurretRandom <= 17) // �^���b�g
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

        // �ǐ���(�{�X�}�b�v�p)
        GameObject obj;

        obj = Instantiate(MapPrefabs[BossWallNum],
                            new Vector3(-14.0f, 5.0f + MapCount * 16, 0),
                            Quaternion.identity);
        obj.transform.parent = GameObject.Find("Stage").transform;

        obj = Instantiate(MapPrefabs[BossWallNum],
                            new Vector3(4.5f, 5.0f + MapCount * 16, 0),
                            Quaternion.identity);
        obj.transform.parent = GameObject.Find("Stage").transform;

        // �{�X�}�b�v�u���b�N&�G����
        for (int x = 0; x < CSVscript.csvDatasInt4.Count; x++)
        {
            for (int y = 0; y <= 15; y++)
            {
                switch (CSVscript.csvDatasInt4[x][y])
                {
                    case 1: // ����������
                            MapBlockInst(HalfBlockNum400x20, y, x, T, CSVscript.csvDatasInt4.Count);
                    break;

                    case 2: // ������
                            MapBlockInst(BlockNum400x32, y, x, T, CSVscript.csvDatasInt4.Count);
                    break;

                    case 4: // �{�X�����ʒu�ۑ�
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
        // �ǐ���(�m�[�}���}�b�v�p)
        GameObject obj;

        obj = Instantiate(MapPrefabs[NormalWallNum],
                            new Vector3(-14.0f, 5.0f + MapCount * 16, 0),
                            Quaternion.identity);
        obj.transform.parent = GameObject.Find("Stage").transform;

        obj = Instantiate(MapPrefabs[NormalWallNum],
                            new Vector3(4.5f, 5.0f + MapCount * 16, 0),
                            Quaternion.identity);
        obj.transform.parent = GameObject.Find("Stage").transform;

        // �}�b�v�u���b�N&�G����
        for (int x = 0; x < CSVscript.csvDatasInt.Count; x++)
        {
            for (int y = 0; y <= 15; y++)
            {
                switch (CSVscript.csvDatasInt[x][y])
                {
                    case 1: // ����������
                        MapBlockInst(HalfBlockNum400x20, y, x, T, CSVscript.csvDatasInt.Count);
                        break;

                    case 2: // ������
                        MapBlockInst(BlockNum400x32, y, x, T, CSVscript.csvDatasInt.Count);
                        break;

                    case 3: // �G����
                        EnemyInst(12, y, x + 2, T, CSVscript.csvDatasInt.Count);
                        break;
                }
            }
        }

        MapCount++;

        // �ǐ���(�m�[�}���}�b�v�p)
        GameObject obj2;

        obj2 = Instantiate(MapPrefabs[NormalWallNum],
                            new Vector3(-14.0f, 5.0f + MapCount * 16, 0),
                            Quaternion.identity);
        obj2.transform.parent = GameObject.Find("Stage").transform;

        obj2 = Instantiate(MapPrefabs[NormalWallNum],
                            new Vector3(4.5f, 5.0f + MapCount * 16, 0),
                            Quaternion.identity);
        obj2.transform.parent = GameObject.Find("Stage").transform;

        // �}�b�v�u���b�N����
        for (int x = 0; x < CSVscript.csvDatasInt2.Count; x++)
        {
            for (int y = 0; y <= 15; y++)
            {
                switch (CSVscript.csvDatasInt2[x][y])
                {
                    case 1: // ����������
                        MapBlockInst(HalfBlockNum400x20, y, x, T, CSVscript.csvDatasInt2.Count);
                        break;

                    case 2: // ������
                        MapBlockInst(BlockNum400x32, y, x, T, CSVscript.csvDatasInt2.Count);
                        break;
                }
            }
        }

        MapCount++;

        // �ǐ���(�m�[�}���}�b�v�p)
        GameObject obj3;

        obj3 = Instantiate(MapPrefabs[NormalWallNum],
                            new Vector3(-14.0f, 5.0f + MapCount * 16, 0),
                            Quaternion.identity);
        obj3.transform.parent = GameObject.Find("Stage").transform;

        obj3 = Instantiate(MapPrefabs[NormalWallNum],
                            new Vector3(4.5f, 5.0f + MapCount * 16, 0),
                            Quaternion.identity);
        obj3.transform.parent = GameObject.Find("Stage").transform;

        // �}�b�v�u���b�N����
        for (int x = 0; x < CSVscript.csvDatasInt2.Count; x++)
        {
            for (int y = 0; y <= 15; y++)
            {
                switch (CSVscript.csvDatasInt2[x][y])
                {
                    case 1: // ����������
                        MapBlockInst(HalfBlockNum400x20, y, x, T, CSVscript.csvDatasInt2.Count);
                        break;

                    case 2: // ������
                        MapBlockInst(BlockNum400x32, y, x, T, CSVscript.csvDatasInt2.Count);
                        break;
                }
            }
        }

        MapCount++;
    }

    // �}�b�v�u���b�N����(���, x���W, y���W, x���W�␳, y���W������)
    void MapBlockInst(int MapTmp, int forY, int forX, float T, float PosY)
    {
        Instantiate(MapPrefabs[MapTmp],
                            new Vector3(forY - T, forX + MapCount * (PosY * 1.33f), 0),
                            Quaternion.identity);
    }

    // �}�b�v�u���b�N����(��]�t)(���, x���W, y���W, x���W�␳, y���W������)
    void MapBlockInstRotate(int MapTmp, int forY, int forX, float T, float PosY)
    {
        GameObject obj = Instantiate(MapPrefabs[MapTmp],
                            new Vector3(forY - T, forX + MapCount * (PosY * 1.33f), 0),
                            Quaternion.identity);

        obj.transform.localEulerAngles = new Vector3(0, 0, 90);
    }

    // �G����(���, x���W, y���W, x���W�␳, y���W������)
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
