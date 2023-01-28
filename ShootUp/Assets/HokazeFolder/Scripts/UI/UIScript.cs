using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*----------------------------------------
 UI全般のスクリプト
----------------------------------------*/

public class UIScript : MonoBehaviour
{
    Camera Camera;

    // スクリプト ************************
    GunController Gunscript;
    CameraScript Cmrscript;
    Score ScoreSC;
    EnemyDrop EnemySC;

    // オブジェクト **********************
    GameObject Player; // プレイヤー
    GameObject Gun; // プレイヤーが持っている銃
    [HideInInspector] public GameObject AimObj; // 照準(カーソル)
    [HideInInspector] public GameObject AimObj2; // 照準2
    [HideInInspector] public GameObject panel; // 一時停止用(ちょっと暗くするため)
    GameObject[] menuButton; // メニューボタン{0:リスタートボタン, 1:タイトルボタン} ※ Resources.Load
    GameObject reStart; // メニュー0
    GameObject title; // メニュー1
    GameObject mainWeapon; // 武器0
    GameObject subWeapon; // 武器1
    GameObject specialWeapon; // 武器2
    GameObject[] LockObj; // 武器使用不可能UI
    [HideInInspector] public GameObject BossTx; // ボスエリア出現テキスト
    GameObject EnemyScoreText; // 敵撃破時スコア表示テキスト
    GameObject[] EnemyHPbar; // 敵HPバー
    [HideInInspector] public GameObject Slider; // スライダーGetComponent用
    [HideInInspector] public GameObject[] EffectObj; // エフェクト

    // スライダー ************************
    Slider slider; // ボスHPスライダー

    // テキスト **************************
    [HideInInspector] public GameObject pauseTx; // 一時停止用
    Text[] BulletTx; // 残弾数テキスト{0:メイン武器用, 1:サブ武器用,
                     // 2:ピストル用, 3:ショットガン用, 4:スナイパー用, 5:マシンガン用,
                     // 6:特殊武器用, 7:グレネード用, 8:投げナイフ用}
    Text SniperText; // スナイパーライフルエイムモード表示

    // string ****************************
    string[] TextSt; // 残弾数テキスト用string
    string[] LockSt; // 武器ロック用string
    
    // Image *****************************
    GameObject[] weaponIMG; // メイン・サブ武器Image{0:メイン武器, 1:サブ武器} ※ Resources.Load
    [HideInInspector] public GameObject[] MenuIMG; // メニュー用UI{0:Up, 1:Down}
    [HideInInspector] public GameObject pauseIMG; // ポーズImage
    Image reStartIMG; // メニュー0Image
    Image titleIMG; // メニュー1Image
    Image mainWeaponBackIMG; // 武器0Image
    Image subWeaponBackIMG; // 武器1Image
    Image specialWeaponBackIMG; // 武器2Image
    Image HPbarCtrl; // ボスHPバーの上に被せるオブジェクト
    Image FadeOutPanel; // 死亡時フェードアウト用

    // Sprite ****************************
    Sprite[] wSp; // メイン・サブ武器用のSprite ※Resources.Load

    // float *****************************
    // 弾数上限
    [HideInInspector] public float MaxPistolBullets; // ピストル
    [HideInInspector] public float MaxSniperBullets; // スナイパー
    [HideInInspector] public float MaxShotGunBullets; // ショットガン
    [HideInInspector] public float MaxMachineGunBullets; // マシンガン
    [HideInInspector] public float MaxGrenades; // グレネード
    [HideInInspector] public float MaxThrowKnifes; // 投げナイフ

    // Color *****************************
    Color colWhite = Color.white; // 白
    Color colYellow = Color.yellow; // 黄
    Color colRed = Color.red; // 赤
    [HideInInspector] public Color pauseOff = new Color(0, 0.49f, 0.64f, 1f); // メニューポーズ用
    [HideInInspector] public Color pauseOn = new Color(0, 0.23f, 0.3f, 1f); // メニューポーズ用

    // bool ******************************
    [HideInInspector] public bool menuCheck = true;

    // int *******************************
    [HideInInspector] public int BossInt = 99;
    int valueHP;

    private void Start()
    {
        if (Camera == null) Camera = Camera.main;

        // Resources.Load
        wSp = Resources.LoadAll<Sprite>("WeaponSprites");
        menuButton = Resources.LoadAll<GameObject>("Button");
        weaponIMG = Resources.LoadAll<GameObject>("WeaponBackImage");
        EnemyScoreText = Resources.Load<GameObject>("EnemyScoreText");
        EnemyHPbar = Resources.LoadAll<GameObject>("HPbar");
        EffectObj = Resources.LoadAll<GameObject>("Effects");

        // 配列 定義
        BulletTx = new Text[9];
        LockObj = new GameObject[9];
        MenuIMG = new GameObject[2];
        TextSt = new string[9] {"W1", "W2", "PisA", "SniA", "ShotA", "SmgA", "sW1", "Boom", "Knife"};
        LockSt = new string[9] { "Lock", "Lock2", "Lock3", "Lock4", "Lock5", "Lock6", "Lock7", "Lock8", "Lock9" };

        // Script
        Gun = GameObject.Find("Gun");
        Gunscript = Gun.GetComponent<GunController>();

        Cmrscript = GameObject.Find("Main Camera").GetComponent<CameraScript>();

        GameObject obj2 = GameObject.Find("Admin");
        ScoreSC = obj2.GetComponent<Score>();
        EnemySC = obj2.GetComponent<EnemyDrop>();

        // GameObject.Find
        Player = GameObject.Find("Player");
        MenuIMG[0] = GameObject.Find("Up");
        MenuIMG[1] = GameObject.Find("Down");
        AimObj = GameObject.Find("Cursor");
        AimObj2 = GameObject.Find("Cursor2");
        panel = GameObject.Find("PausePanel");
        pauseTx = GameObject.Find("PauseTx");
        pauseIMG = GameObject.Find("PauseImage");
        SniperText = GameObject.Find("SniperText").GetComponent<Text>();
        BossTx = GameObject.Find("BossText");
        HPbarCtrl = GameObject.Find("HPbarC").GetComponent<Image>();
        FadeOutPanel = GameObject.Find("FadeOutPanel").GetComponent<Image>();

        HPbarCtrl.transform.SetAsLastSibling();

        panel.SetActive(false);
        pauseTx.GetComponent<Text>().text = "";

        SniperText.text = "";

        Slider = GameObject.Find("BossHPbar");
        slider = Slider.GetComponent<Slider>();
        Slider.SetActive(false);

        // 読み込み等
        Load();

        pauseIMG.transform.parent = GameObject.Find("MiniMenu").transform;

        BossTx.SetActive(false);
    }

    private void Update()
    {
        // メイン・サブ・スペシャル武器の表示切替(メイン武器背景Image, サブ武器背景Image, スペシャル武器背景Image)
        GunUI(mainWeaponBackIMG, subWeaponBackIMG, specialWeaponBackIMG);

        // 残弾数表示
        BulletCntUI();

        // 残弾数で色変え(右の数値ほど弾数少ない表示)
        BulTxColor(0.4f, 0.1f);

        // メニュー(メニュー切り替え速度), リスタート用UI, タイトルへ用UI
        MENU(4.0f, reStartIMG, titleIMG);

        // スナイパーライフルエイムモード表示
        SNIPERtext();

        // スコア表示
        SCORE();

        // ボスHP更新
        Boss_HPbarReload();

        // スケール初期化
        reStart.transform.localScale = new Vector3(1, 1, 1);
        title.transform.localScale = new Vector3(1, 1, 1);
        mainWeapon.transform.localScale = new Vector3(1, 1, 1);
        subWeapon.transform.localScale = new Vector3(1, 1, 1);
        specialWeapon.transform.localScale = new Vector3(1, 1, 1);

        // ゲーム終了時にフェードアウト
        if (Player == null)
        {
            FadeOutPanel.color += new Color(0, 0, 0, 0.005f);
            FadeOutPanel.transform.SetAsLastSibling();
        }
    }

    // ボスエリア出現時にテキスト表示
    public IEnumerator BossTxChange()
    {
        BossTx.SetActive(true);
        var second = new WaitForSeconds(3.0f);
        yield return second;
        BossTx.SetActive(false);
    }

    // スコアの数に応じて使用可能武器の表示と入手可能な弾の種類を増やす
    void SCORE()
    {
        if (ScoreSC.ScoreCount < 10) {
            LockObj[0].SetActive(false);
            LockObj[3].SetActive(false);
            LockObj[2].SetActive(false);
            LockObj[7].SetActive(false);
            LockObj[8].SetActive(false);
            EnemySC.MaxRando = 1;
        }
        else if (ScoreSC.ScoreCount < 100)
        {
            LockObj[1].SetActive(false);
            LockObj[4].SetActive(false);
            EnemySC.MaxRando = 2;
        }
        else if (ScoreSC.ScoreCount < 200)
        {
            LockObj[5].SetActive(false);
            LockObj[6].SetActive(false);
            EnemySC.MaxRando = 4;
        }
    }

    // スナイパーライフルのスコープオンオフのテキスト表示
    void SNIPERtext()
    {
        // メイン武器がスナイパーライフルの場合
        if (Gunscript.MainWeapon == "Sniper")
        {
            if (Cmrscript.sniperAimCheck) 
                SniperText.text = "[Q]SniperScope ON";

            else 
                SniperText.text = "[Q]SniperScope OFF";
        }
        else 
            SniperText.text = "";
    }

    // 残弾数に応じてテキストの色を変える
    void BulTxColor(float hazardLv1, float hazardLv2)
    {
        // メイン武器
        switch (Gunscript.MainWeapon)
        {
            case "Pistol":
                if (Gunscript.NowPistolBulletCount <= MaxPistolBullets * hazardLv2) 
                    BulletTx[0].color = colRed;

                else if (Gunscript.NowPistolBulletCount <= MaxPistolBullets * hazardLv1) 
                    BulletTx[0].color = colYellow;

                else 
                    BulletTx[0].color = colWhite;

                break;

            case "Sniper":
                if (Gunscript.NowSniperBulletCount <= MaxSniperBullets * hazardLv2) 
                    BulletTx[0].color = colRed;

                else if (Gunscript.NowSniperBulletCount <= MaxSniperBullets * hazardLv1) 
                    BulletTx[0].color = colYellow;

                else
                    BulletTx[0].color = colWhite;

                break;

            case "ShotGun":
                if (Gunscript.NowShotGunBulletCount <= MaxShotGunBullets * hazardLv2) 
                    BulletTx[0].color = colRed;

                else if (Gunscript.NowShotGunBulletCount <= MaxShotGunBullets * hazardLv1) 
                    BulletTx[0].color = colYellow;

                else
                    BulletTx[0].color = colWhite;

                break;

            case "MachineGun":
                if (Gunscript.NowMachineGunBulletCount <= MaxMachineGunBullets * hazardLv2) 
                    BulletTx[0].color = colRed;

                else if (Gunscript.NowMachineGunBulletCount <= MaxMachineGunBullets * hazardLv1) 
                    BulletTx[0].color = colYellow;

                else
                    BulletTx[0].color = colWhite;

                break;
        }

        // サブ武器
        switch (Gunscript.SubWeapn)
        {
            case "Pistol":
                if (Gunscript.NowPistolBulletCount <= MaxPistolBullets * hazardLv2) 
                    BulletTx[1].color = colRed;

                else if (Gunscript.NowPistolBulletCount <= MaxPistolBullets * hazardLv1)
                    BulletTx[1].color = colYellow;

                else 
                    BulletTx[1].color = colWhite;

                break;

            case "Sniper":
                if (Gunscript.NowSniperBulletCount <= MaxSniperBullets * hazardLv2) 
                    BulletTx[1].color = colRed;

                else if (Gunscript.NowSniperBulletCount <= MaxSniperBullets * hazardLv1)
                    BulletTx[1].color = colYellow;

                else
                    BulletTx[1].color = colWhite;

                break;

            case "ShotGun":
                if (Gunscript.NowShotGunBulletCount <= MaxShotGunBullets * hazardLv2) 
                    BulletTx[1].color = colRed;

                else if (Gunscript.NowShotGunBulletCount <= MaxShotGunBullets * hazardLv1)
                    BulletTx[1].color = colYellow;

                else
                    BulletTx[1].color = colWhite;

                break;

            case "MachineGun":
                if (Gunscript.NowMachineGunBulletCount <= MaxMachineGunBullets * hazardLv2)
                    BulletTx[1].color = colRed;

                else if (Gunscript.NowMachineGunBulletCount <= MaxMachineGunBullets * hazardLv1)
                    BulletTx[1].color = colYellow;

                else
                    BulletTx[1].color = colWhite;

                break;
        }

        // 装備表示一覧のピストル
        if (Gunscript.NowPistolBulletCount <= MaxPistolBullets * hazardLv2)
            BulletTx[2].color = colRed;

        else if (Gunscript.NowPistolBulletCount <= MaxPistolBullets * hazardLv1)
            BulletTx[2].color = colYellow;

        else
            BulletTx[2].color = colWhite;

        // 装備表示一覧のスナイパー
        if (Gunscript.NowSniperBulletCount <= MaxSniperBullets * hazardLv2)
            BulletTx[3].color = colRed;

        else if (Gunscript.NowSniperBulletCount <= MaxSniperBullets * hazardLv1)
            BulletTx[3].color = colYellow;

        else
            BulletTx[3].color = colWhite;

        // 装備表示一覧のショットガン
        if (Gunscript.NowShotGunBulletCount <= MaxShotGunBullets * hazardLv2)
            BulletTx[4].color = colRed;

        else if (Gunscript.NowShotGunBulletCount <= MaxShotGunBullets * hazardLv1)
            BulletTx[4].color = colYellow;

        else
            BulletTx[4].color = colWhite;

        // 装備表示一覧のマシンガン
        if (Gunscript.NowMachineGunBulletCount <= MaxMachineGunBullets * hazardLv2)
            BulletTx[5].color = colRed;

        else if (Gunscript.NowMachineGunBulletCount <= MaxMachineGunBullets * hazardLv1)
            BulletTx[5].color = colYellow;

        else
            BulletTx[5].color = colWhite;
    }

    void BulletCntUI() // 残弾数表示
    {
        if (Gunscript.NowPistolBulletCount > MaxPistolBullets) Gunscript.NowPistolBulletCount = (int)MaxPistolBullets;
        if (Gunscript.NowSniperBulletCount > MaxSniperBullets) Gunscript.NowSniperBulletCount = (int)MaxSniperBullets;
        if (Gunscript.NowShotGunBulletCount > MaxShotGunBullets) Gunscript.NowShotGunBulletCount = (int)MaxShotGunBullets;
        if (Gunscript.NowMachineGunBulletCount > MaxMachineGunBullets) Gunscript.NowMachineGunBulletCount = (int)MaxMachineGunBullets;
        if (Gunscript.NowGrenadeBulletCount > MaxGrenades) Gunscript.NowGrenadeBulletCount = (int)MaxGrenades;
        if (Gunscript.NowThrowingknifeBulletCount > MaxThrowKnifes) Gunscript.NowThrowingknifeBulletCount = (int)MaxThrowKnifes;

        switch (Gunscript.MainWeapon) // メイン武器残弾数
        {
            case "Pistol":
                BulletTx[0].text = Gunscript.NowPistolBulletCount + "/" + MaxPistolBullets;
                break;

            case "Sniper":
                BulletTx[0].text = Gunscript.NowSniperBulletCount + "/" + MaxSniperBullets;
                break;

            case "ShotGun":
                BulletTx[0].text = Gunscript.NowShotGunBulletCount + "/" + MaxShotGunBullets;
                break;

            case "MachineGun":
                BulletTx[0].text = Gunscript.NowMachineGunBulletCount + "/" + MaxMachineGunBullets;
                break;
        }

        switch (Gunscript.SubWeapn) // サブ武器残弾数
        {
            case "Pistol":
                BulletTx[1].text = Gunscript.NowPistolBulletCount + "/" + MaxPistolBullets;
                break;

            case "Sniper":
                BulletTx[1].text = Gunscript.NowSniperBulletCount + "/" + MaxSniperBullets;
                break;

            case "ShotGun":
                BulletTx[1].text = Gunscript.NowShotGunBulletCount + "/" + MaxShotGunBullets;
                break;

            case "MachineGun":
                BulletTx[1].text = Gunscript.NowMachineGunBulletCount + "/" + MaxMachineGunBullets;
                break;

            case "":
                BulletTx[1].text = "0/0";
                break;
        }

        switch (Gunscript.SpecialWeapon) // スペシャル武器残弾数
        {
            case "Grenade":
                BulletTx[6].text = Gunscript.NowGrenadeBulletCount + "/" + MaxGrenades;
                break;

            case "ThrowingKnife":
                BulletTx[6].text = Gunscript.NowThrowingknifeBulletCount + "/" + MaxThrowKnifes;
                break;
        }

        BulletTx[2].text = Gunscript.NowPistolBulletCount + "/" + MaxPistolBullets; // ピストル残弾数
        BulletTx[3].text = Gunscript.NowSniperBulletCount + "/" + MaxSniperBullets; // スナイパー残弾数
        BulletTx[4].text = Gunscript.NowShotGunBulletCount + "/" + MaxShotGunBullets; // ショットガン残弾数
        BulletTx[5].text = Gunscript.NowMachineGunBulletCount + "/" + MaxMachineGunBullets; // マシンガン残弾数
        BulletTx[7].text = Gunscript.NowGrenadeBulletCount + "/" + MaxGrenades; // グレネード残数
        BulletTx[8].text = Gunscript.NowThrowingknifeBulletCount + "/" + MaxThrowKnifes; // 投げナイフ残数
    }

    // メイン・サブ・スペシャル武器の表示切替
    void GunUI(Image tmpImg, Image tmpImg2, Image tmpImg3)
    {
        SpriteRenderer GunSpr = Gun.GetComponent<SpriteRenderer>();

        switch (Gunscript.MainWeapon) // メイン武器切り替え
        {
            case "Pistol":
                tmpImg.sprite = wSp[0];
                GunSpr.sprite = wSp[4];
                break;

            case "Sniper":
                tmpImg.sprite = wSp[1];
                GunSpr.sprite = wSp[5];
                break;

            case "ShotGun":
                tmpImg.sprite = wSp[2];
                GunSpr.sprite = wSp[6];
                break;

            case "MachineGun":
                tmpImg.sprite = wSp[3];
                GunSpr.sprite = wSp[7];
                break;
        }

        switch (Gunscript.SubWeapn) // サブ武器切り替え
        {
            case "Pistol":
                tmpImg2.sprite = wSp[0];
                break;

            case "Sniper":
                tmpImg2.sprite = wSp[1];
                break;

            case "ShotGun":
                tmpImg2.sprite = wSp[2];
                break;

            case "MachineGun":
                tmpImg2.sprite = wSp[3];
                break;
        }

        switch (Gunscript.SpecialWeapon) // スペシャル武器切り替え
        {
            case "Grenade":
                tmpImg3.sprite = wSp[8];
                break;

            case "ThrowingKnife":
                tmpImg3.sprite = wSp[9];
                break;
        }
    }

    // メニュー
    void MENU(float Speed, Image buttonImg, Image buttonImg2)
    {
        Image tmpImg = MenuIMG[0].GetComponent<Image>();
        Image tmpImg2 = MenuIMG[1].GetComponent<Image>();

        if (menuCheck) // 閉じる
        {
            // [UpImg] 回転が0.0fになるまで
            if (tmpImg.transform.localEulerAngles.z > 1.0f)
            {
                if (Screen.width > 1300)
                {
                    // [UpImg] 回転 zマイナス
                    tmpImg.transform.localEulerAngles += new Vector3(0, 0, -Speed);

                    // [UpImg] サイズ widthプラス
                    tmpImg.rectTransform.sizeDelta += new Vector2(Speed / 2.0f, 0);

                    // [UpImg] 座標 xプラス,yプラス
                    tmpImg.transform.position += new Vector3(Speed / 4.0f, Speed / 5.0f, 0);

                    // [DownImg] 回転 zプラス
                    tmpImg2.transform.localEulerAngles += new Vector3(0, 0, Speed);

                    // [DownImg] サイズ widthプラス
                    tmpImg2.rectTransform.sizeDelta += new Vector2(Speed / 2.0f, 0);

                    // [DownImg] 座標 xプラス,yマイナス
                    tmpImg2.transform.position += new Vector3(Speed / 4.0f, -Speed / 5.0f, 0);
                }
                else
                {
                    // [UpImg] 回転 zマイナス
                    tmpImg.transform.localEulerAngles += new Vector3(0, 0, -Speed / 2.0f);

                    // [UpImg] サイズ widthプラス
                    tmpImg.rectTransform.sizeDelta += new Vector2(Speed / 4.0f, 0);

                    // [UpImg] 座標 xプラス,yプラス
                    tmpImg.transform.position += new Vector3(Speed / 12.0f, Speed / 15.0f, 0);

                    // [DownImg] 回転 zプラス
                    tmpImg2.transform.localEulerAngles += new Vector3(0, 0, Speed / 2.0f);

                    // [DownImg] サイズ widthプラス
                    tmpImg2.rectTransform.sizeDelta += new Vector2(Speed / 4.0f, 0);

                    // [DownImg] 座標 xプラス,yマイナス
                    tmpImg2.transform.position += new Vector3(Speed / 12.0f, -Speed / 15.0f, 0);
                }

                // メニューボタン
                if (Screen.width > 1300)
                {
                    buttonImg.transform.position += new Vector3(Speed * 4.75f, 0, 0);
                    buttonImg2.transform.position += new Vector3(Speed * 9.25f, 0, 0);
                }
                else
                {
                    buttonImg.transform.position += new Vector3(Speed * (4.75f / 3.0f), 0, 0);
                    buttonImg2.transform.position += new Vector3(Speed * (9.25f / 3.0f), 0, 0);
                }
            }
        }
        else // 開ける
        {
            // [UpImg] 回転が40.0fになるまで
            if (tmpImg.transform.localEulerAngles.z < 40.0f)
            {
                if (Screen.width > 1300)
                {
                    // [UpImg] 回転 zプラス
                    tmpImg.transform.localEulerAngles += new Vector3(0, 0, Speed);

                    // [UpImg] サイズ widthマイナス
                    tmpImg.rectTransform.sizeDelta += new Vector2(-Speed / 2.0f, 0);

                    // [UpImg] 座標 xマイナス,yマイナス
                    tmpImg.transform.position += new Vector3(-Speed / 4.0f, -Speed / 5.0f, 0);

                    // [DownImg] 回転 zマイナス
                    tmpImg2.transform.localEulerAngles += new Vector3(0, 0, -Speed);

                    // [DownImg] サイズ widthマイナス
                    tmpImg2.rectTransform.sizeDelta += new Vector2(-Speed / 2.0f, 0);

                    // [DownImg] 座標 xマイナス,yプラス
                    tmpImg2.transform.position += new Vector3(-Speed / 4.0f, Speed / 5.0f, 0);
                }
                else
                {
                    // [UpImg] 回転 zプラス
                    tmpImg.transform.localEulerAngles += new Vector3(0, 0, Speed / 2.0f);

                    // [UpImg] サイズ widthマイナス
                    tmpImg.rectTransform.sizeDelta += new Vector2(-Speed / 4.0f, 0);

                    // [UpImg] 座標 xマイナス,yマイナス
                    tmpImg.transform.position += new Vector3(-Speed / 12.0f, -Speed / 15.0f, 0);

                    // [DownImg] 回転 zマイナス
                    tmpImg2.transform.localEulerAngles += new Vector3(0, 0, -Speed / 2.0f);

                    // [DownImg] サイズ widthマイナス
                    tmpImg2.rectTransform.sizeDelta += new Vector2(-Speed / 4.0f, 0);

                    // [DownImg] 座標 xマイナス,yプラス
                    tmpImg2.transform.position += new Vector3(-Speed / 12.0f, Speed / 15.0f, 0);
                }

                // メニューボタン
                if (Screen.width > 1300)
                {
                    buttonImg.transform.position += new Vector3(-Speed * 4.75f, 0, 0);
                    buttonImg2.transform.position += new Vector3(-Speed * 9.25f, 0, 0);
                }
                else
                {
                    buttonImg.transform.position += new Vector3(-Speed * (4.75f / 3.0f), 0, 0);
                    buttonImg2.transform.position += new Vector3(-Speed * (9.25f / 3.0f), 0, 0);
                }
            }
        }

    }

    // ゲーム開始時UI
    void Load()
    {
        // 残弾数テキスト
        for (int i = 0; i < BulletTx.Length; i++)
        {
            BulletTx[i] = GameObject.Find(TextSt[i]).GetComponent<Text>(); // コンポーネント取得
        }

        // ロックオブジェクト(Image)
        for (int j = 0; j < LockObj.Length; j++)
        {
            LockObj[j] = GameObject.Find(LockSt[j]); // Find
        }

        // 武器背景Image
        if (Screen.width > 1300)
        {
            mainWeapon = Instantiate(weaponIMG[0], new Vector3(1462, 871, 0), Quaternion.identity); // 生成
            subWeapon = Instantiate(weaponIMG[0], new Vector3(1462, 648, 0), Quaternion.identity);
            specialWeapon = Instantiate(weaponIMG[0], new Vector3(1462, 323, 0), Quaternion.identity);
        }
        else
        {
            mainWeapon = Instantiate(weaponIMG[0], new Vector3(975, 585, 0), Quaternion.identity); // 生成
            subWeapon = Instantiate(weaponIMG[0], new Vector3(975, 434, 0), Quaternion.identity);
            specialWeapon = Instantiate(weaponIMG[0], new Vector3(975, 211, 0), Quaternion.identity);
        }

        mainWeapon.transform.parent = GameObject.Find("Weapon1").transform; // 親子付け
        subWeapon.transform.parent = GameObject.Find("Weapon2").transform;
        specialWeapon.transform.parent = GameObject.Find("sWeapon1").transform;
        mainWeapon.transform.SetAsFirstSibling();
        subWeapon.transform.SetAsFirstSibling();
        specialWeapon.transform.SetAsFirstSibling();
        mainWeaponBackIMG = mainWeapon.GetComponent<Image>(); // コンポーネント取得
        subWeaponBackIMG = subWeapon.GetComponent<Image>();
        specialWeaponBackIMG = specialWeapon.GetComponent<Image>();

        // メニューImage(リスタート, タイトルへ)
        if (Screen.width > 1300)
        {
            reStart = Instantiate(menuButton[0], new Vector3(1870, 70, 0), Quaternion.identity); // 生成
            title = Instantiate(menuButton[1], new Vector3(1870, 70, 0), Quaternion.identity);
        }
        else
        {
            reStart = Instantiate(menuButton[0], new Vector3(1260, 49, 0), Quaternion.identity); // 生成
            title = Instantiate(menuButton[1], new Vector3(1260, 49, 0), Quaternion.identity);
        }
        reStart.transform.parent = GameObject.Find("MiniMenu").transform; // 親子付け
        title.transform.parent = GameObject.Find("MiniMenu").transform;
        reStartIMG = reStart.GetComponent<Image>(); // コンポーネント取得
        titleIMG = title.GetComponent<Image>();
    }

    // 敵撃破時の簡易スコア表示
    public void E_ScoreTxInst(Transform Tf, int score)
    {
        // スコア生成
        GameObject tx = Instantiate(EnemyScoreText, new Vector3(0, 0, 0), Quaternion.identity);
        tx.GetComponent<Text>().text = "+" + score;

        tx.transform.parent = GameObject.Find("Canvas").transform;

        // 敵の座標をUI座標に変換
        var targetPos = Camera.WorldToScreenPoint(Tf.transform.position);

        RectTransform rect = tx.GetComponent<RectTransform>();

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rect,
            targetPos,
            null,
            out var uiLocalPos
            );

        tx.transform.position = uiLocalPos;

        Destroy(tx, 1f);
    }

    // 敵のHPバー生成
    public void E_HPbarInst(GameObject target, int targetHP, int enemyTmp)
    {
        GameObject Ebar = Instantiate(EnemyHPbar[1],
            new Vector3(0, 0, 0),
            Quaternion.identity);

        switch (enemyTmp)
        {
            case 2:
                target.GetComponent<E101>().ebar = Ebar;
                break;

            case 3:
                target.GetComponent<E102>().ebar = Ebar;
                break;

            case 4:
                target.GetComponent<E103>().ebar = Ebar;
                break;

            case 5:
                target.GetComponent<E201>().ebar = Ebar;
                break;

            case 6:
                target.GetComponent<E202>().ebar = Ebar;
                break;

            case 7:
                target.GetComponent<E203>().ebar = Ebar;
                break;

            case 8:
                target.GetComponent<E204>().ebar = Ebar;
                break;

            case 9:
                target.GetComponent<E205>().ebar = Ebar;
                break;

            case 10:
                target.GetComponent<E206>().ebar = Ebar;
                break;

            case 11:
                target.GetComponent<E207>().ebar = Ebar;
                break;

            case 12:
                target.GetComponent<T001>().ebar = Ebar;
                break;

            case 13:
                target.GetComponent<T002>().ebar = Ebar;
                break;

            case 14:
                target.GetComponent<T101>().ebar = Ebar;
                break;

            case 15:
                target.GetComponent<T102>().ebar = Ebar;
                break;

            case 16:
                target.GetComponent<T201>().ebar = Ebar;
                break;

            case 17:
                target.GetComponent<T202>().ebar = Ebar;
                break;
        }

        Ebar.transform.parent = GameObject.Find("Canvas").transform;

        Ebar.GetComponent<EnemyHPbarScript>().Initialize(target, targetHP);
    }

    // ボスHPバー表示
    public void Boss_HPbarInitialize(int Boss, int HP)
    {
        // ボスの種類によりHPバーの数値調整
        BossInt = Boss;
        Slider.SetActive(true);
        slider.maxValue = HP;
        valueHP = HP;
        slider.value = HP;
    }

    // ボスHPバー数値更新
    void Boss_HPbarReload()
    {
        switch (BossInt)
        {
            case 0:
                GameObject obj = GameObject.Find("Boss001(Clone)");
                Boss001 script = null;
                if (obj != null)
                {
                    script = obj.GetComponent<Boss001>();

                    if (script.HP < valueHP)
                    {
                        valueHP = script.HP;
                        slider.value = valueHP;
                    }
                }
                break;

            case 1:
                GameObject obj2 = GameObject.Find("Boss002(Clone)");
                Boss002 script2 = null;
                if (obj2 != null)
                {
                    script2 = obj2.GetComponent<Boss002>();

                    if (script2.HP < valueHP)
                    {
                        valueHP = script2.HP;
                        slider.value = valueHP;
                    }
                }
                break;
        }
    }
}