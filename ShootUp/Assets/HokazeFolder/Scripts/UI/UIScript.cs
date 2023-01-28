using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*----------------------------------------
 UI�S�ʂ̃X�N���v�g
----------------------------------------*/

public class UIScript : MonoBehaviour
{
    Camera Camera;

    // �X�N���v�g ************************
    GunController Gunscript;
    CameraScript Cmrscript;
    Score ScoreSC;
    EnemyDrop EnemySC;

    // �I�u�W�F�N�g **********************
    GameObject Player; // �v���C���[
    GameObject Gun; // �v���C���[�������Ă���e
    [HideInInspector] public GameObject AimObj; // �Ə�(�J�[�\��)
    [HideInInspector] public GameObject AimObj2; // �Ə�2
    [HideInInspector] public GameObject panel; // �ꎞ��~�p(������ƈÂ����邽��)
    GameObject[] menuButton; // ���j���[�{�^��{0:���X�^�[�g�{�^��, 1:�^�C�g���{�^��} �� Resources.Load
    GameObject reStart; // ���j���[0
    GameObject title; // ���j���[1
    GameObject mainWeapon; // ����0
    GameObject subWeapon; // ����1
    GameObject specialWeapon; // ����2
    GameObject[] LockObj; // ����g�p�s�\UI
    [HideInInspector] public GameObject BossTx; // �{�X�G���A�o���e�L�X�g
    GameObject EnemyScoreText; // �G���j���X�R�A�\���e�L�X�g
    GameObject[] EnemyHPbar; // �GHP�o�[
    [HideInInspector] public GameObject Slider; // �X���C�_�[GetComponent�p
    [HideInInspector] public GameObject[] EffectObj; // �G�t�F�N�g

    // �X���C�_�[ ************************
    Slider slider; // �{�XHP�X���C�_�[

    // �e�L�X�g **************************
    [HideInInspector] public GameObject pauseTx; // �ꎞ��~�p
    Text[] BulletTx; // �c�e���e�L�X�g{0:���C������p, 1:�T�u����p,
                     // 2:�s�X�g���p, 3:�V���b�g�K���p, 4:�X�i�C�p�[�p, 5:�}�V���K���p,
                     // 6:���ꕐ��p, 7:�O���l�[�h�p, 8:�����i�C�t�p}
    Text SniperText; // �X�i�C�p�[���C�t���G�C�����[�h�\��

    // string ****************************
    string[] TextSt; // �c�e���e�L�X�g�pstring
    string[] LockSt; // ���탍�b�N�pstring
    
    // Image *****************************
    GameObject[] weaponIMG; // ���C���E�T�u����Image{0:���C������, 1:�T�u����} �� Resources.Load
    [HideInInspector] public GameObject[] MenuIMG; // ���j���[�pUI{0:Up, 1:Down}
    [HideInInspector] public GameObject pauseIMG; // �|�[�YImage
    Image reStartIMG; // ���j���[0Image
    Image titleIMG; // ���j���[1Image
    Image mainWeaponBackIMG; // ����0Image
    Image subWeaponBackIMG; // ����1Image
    Image specialWeaponBackIMG; // ����2Image
    Image HPbarCtrl; // �{�XHP�o�[�̏�ɔ킹��I�u�W�F�N�g
    Image FadeOutPanel; // ���S���t�F�[�h�A�E�g�p

    // Sprite ****************************
    Sprite[] wSp; // ���C���E�T�u����p��Sprite ��Resources.Load

    // float *****************************
    // �e�����
    [HideInInspector] public float MaxPistolBullets; // �s�X�g��
    [HideInInspector] public float MaxSniperBullets; // �X�i�C�p�[
    [HideInInspector] public float MaxShotGunBullets; // �V���b�g�K��
    [HideInInspector] public float MaxMachineGunBullets; // �}�V���K��
    [HideInInspector] public float MaxGrenades; // �O���l�[�h
    [HideInInspector] public float MaxThrowKnifes; // �����i�C�t

    // Color *****************************
    Color colWhite = Color.white; // ��
    Color colYellow = Color.yellow; // ��
    Color colRed = Color.red; // ��
    [HideInInspector] public Color pauseOff = new Color(0, 0.49f, 0.64f, 1f); // ���j���[�|�[�Y�p
    [HideInInspector] public Color pauseOn = new Color(0, 0.23f, 0.3f, 1f); // ���j���[�|�[�Y�p

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

        // �z�� ��`
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

        // �ǂݍ��ݓ�
        Load();

        pauseIMG.transform.parent = GameObject.Find("MiniMenu").transform;

        BossTx.SetActive(false);
    }

    private void Update()
    {
        // ���C���E�T�u�E�X�y�V��������̕\���ؑ�(���C������w�iImage, �T�u����w�iImage, �X�y�V��������w�iImage)
        GunUI(mainWeaponBackIMG, subWeaponBackIMG, specialWeaponBackIMG);

        // �c�e���\��
        BulletCntUI();

        // �c�e���ŐF�ς�(�E�̐��l�قǒe�����Ȃ��\��)
        BulTxColor(0.4f, 0.1f);

        // ���j���[(���j���[�؂�ւ����x), ���X�^�[�g�pUI, �^�C�g���֗pUI
        MENU(4.0f, reStartIMG, titleIMG);

        // �X�i�C�p�[���C�t���G�C�����[�h�\��
        SNIPERtext();

        // �X�R�A�\��
        SCORE();

        // �{�XHP�X�V
        Boss_HPbarReload();

        // �X�P�[��������
        reStart.transform.localScale = new Vector3(1, 1, 1);
        title.transform.localScale = new Vector3(1, 1, 1);
        mainWeapon.transform.localScale = new Vector3(1, 1, 1);
        subWeapon.transform.localScale = new Vector3(1, 1, 1);
        specialWeapon.transform.localScale = new Vector3(1, 1, 1);

        // �Q�[���I�����Ƀt�F�[�h�A�E�g
        if (Player == null)
        {
            FadeOutPanel.color += new Color(0, 0, 0, 0.005f);
            FadeOutPanel.transform.SetAsLastSibling();
        }
    }

    // �{�X�G���A�o�����Ƀe�L�X�g�\��
    public IEnumerator BossTxChange()
    {
        BossTx.SetActive(true);
        var second = new WaitForSeconds(3.0f);
        yield return second;
        BossTx.SetActive(false);
    }

    // �X�R�A�̐��ɉ����Ďg�p�\����̕\���Ɠ���\�Ȓe�̎�ނ𑝂₷
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

    // �X�i�C�p�[���C�t���̃X�R�[�v�I���I�t�̃e�L�X�g�\��
    void SNIPERtext()
    {
        // ���C�����킪�X�i�C�p�[���C�t���̏ꍇ
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

    // �c�e���ɉ����ăe�L�X�g�̐F��ς���
    void BulTxColor(float hazardLv1, float hazardLv2)
    {
        // ���C������
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

        // �T�u����
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

        // �����\���ꗗ�̃s�X�g��
        if (Gunscript.NowPistolBulletCount <= MaxPistolBullets * hazardLv2)
            BulletTx[2].color = colRed;

        else if (Gunscript.NowPistolBulletCount <= MaxPistolBullets * hazardLv1)
            BulletTx[2].color = colYellow;

        else
            BulletTx[2].color = colWhite;

        // �����\���ꗗ�̃X�i�C�p�[
        if (Gunscript.NowSniperBulletCount <= MaxSniperBullets * hazardLv2)
            BulletTx[3].color = colRed;

        else if (Gunscript.NowSniperBulletCount <= MaxSniperBullets * hazardLv1)
            BulletTx[3].color = colYellow;

        else
            BulletTx[3].color = colWhite;

        // �����\���ꗗ�̃V���b�g�K��
        if (Gunscript.NowShotGunBulletCount <= MaxShotGunBullets * hazardLv2)
            BulletTx[4].color = colRed;

        else if (Gunscript.NowShotGunBulletCount <= MaxShotGunBullets * hazardLv1)
            BulletTx[4].color = colYellow;

        else
            BulletTx[4].color = colWhite;

        // �����\���ꗗ�̃}�V���K��
        if (Gunscript.NowMachineGunBulletCount <= MaxMachineGunBullets * hazardLv2)
            BulletTx[5].color = colRed;

        else if (Gunscript.NowMachineGunBulletCount <= MaxMachineGunBullets * hazardLv1)
            BulletTx[5].color = colYellow;

        else
            BulletTx[5].color = colWhite;
    }

    void BulletCntUI() // �c�e���\��
    {
        if (Gunscript.NowPistolBulletCount > MaxPistolBullets) Gunscript.NowPistolBulletCount = (int)MaxPistolBullets;
        if (Gunscript.NowSniperBulletCount > MaxSniperBullets) Gunscript.NowSniperBulletCount = (int)MaxSniperBullets;
        if (Gunscript.NowShotGunBulletCount > MaxShotGunBullets) Gunscript.NowShotGunBulletCount = (int)MaxShotGunBullets;
        if (Gunscript.NowMachineGunBulletCount > MaxMachineGunBullets) Gunscript.NowMachineGunBulletCount = (int)MaxMachineGunBullets;
        if (Gunscript.NowGrenadeBulletCount > MaxGrenades) Gunscript.NowGrenadeBulletCount = (int)MaxGrenades;
        if (Gunscript.NowThrowingknifeBulletCount > MaxThrowKnifes) Gunscript.NowThrowingknifeBulletCount = (int)MaxThrowKnifes;

        switch (Gunscript.MainWeapon) // ���C������c�e��
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

        switch (Gunscript.SubWeapn) // �T�u����c�e��
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

        switch (Gunscript.SpecialWeapon) // �X�y�V��������c�e��
        {
            case "Grenade":
                BulletTx[6].text = Gunscript.NowGrenadeBulletCount + "/" + MaxGrenades;
                break;

            case "ThrowingKnife":
                BulletTx[6].text = Gunscript.NowThrowingknifeBulletCount + "/" + MaxThrowKnifes;
                break;
        }

        BulletTx[2].text = Gunscript.NowPistolBulletCount + "/" + MaxPistolBullets; // �s�X�g���c�e��
        BulletTx[3].text = Gunscript.NowSniperBulletCount + "/" + MaxSniperBullets; // �X�i�C�p�[�c�e��
        BulletTx[4].text = Gunscript.NowShotGunBulletCount + "/" + MaxShotGunBullets; // �V���b�g�K���c�e��
        BulletTx[5].text = Gunscript.NowMachineGunBulletCount + "/" + MaxMachineGunBullets; // �}�V���K���c�e��
        BulletTx[7].text = Gunscript.NowGrenadeBulletCount + "/" + MaxGrenades; // �O���l�[�h�c��
        BulletTx[8].text = Gunscript.NowThrowingknifeBulletCount + "/" + MaxThrowKnifes; // �����i�C�t�c��
    }

    // ���C���E�T�u�E�X�y�V��������̕\���ؑ�
    void GunUI(Image tmpImg, Image tmpImg2, Image tmpImg3)
    {
        SpriteRenderer GunSpr = Gun.GetComponent<SpriteRenderer>();

        switch (Gunscript.MainWeapon) // ���C������؂�ւ�
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

        switch (Gunscript.SubWeapn) // �T�u����؂�ւ�
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

        switch (Gunscript.SpecialWeapon) // �X�y�V��������؂�ւ�
        {
            case "Grenade":
                tmpImg3.sprite = wSp[8];
                break;

            case "ThrowingKnife":
                tmpImg3.sprite = wSp[9];
                break;
        }
    }

    // ���j���[
    void MENU(float Speed, Image buttonImg, Image buttonImg2)
    {
        Image tmpImg = MenuIMG[0].GetComponent<Image>();
        Image tmpImg2 = MenuIMG[1].GetComponent<Image>();

        if (menuCheck) // ����
        {
            // [UpImg] ��]��0.0f�ɂȂ�܂�
            if (tmpImg.transform.localEulerAngles.z > 1.0f)
            {
                if (Screen.width > 1300)
                {
                    // [UpImg] ��] z�}�C�i�X
                    tmpImg.transform.localEulerAngles += new Vector3(0, 0, -Speed);

                    // [UpImg] �T�C�Y width�v���X
                    tmpImg.rectTransform.sizeDelta += new Vector2(Speed / 2.0f, 0);

                    // [UpImg] ���W x�v���X,y�v���X
                    tmpImg.transform.position += new Vector3(Speed / 4.0f, Speed / 5.0f, 0);

                    // [DownImg] ��] z�v���X
                    tmpImg2.transform.localEulerAngles += new Vector3(0, 0, Speed);

                    // [DownImg] �T�C�Y width�v���X
                    tmpImg2.rectTransform.sizeDelta += new Vector2(Speed / 2.0f, 0);

                    // [DownImg] ���W x�v���X,y�}�C�i�X
                    tmpImg2.transform.position += new Vector3(Speed / 4.0f, -Speed / 5.0f, 0);
                }
                else
                {
                    // [UpImg] ��] z�}�C�i�X
                    tmpImg.transform.localEulerAngles += new Vector3(0, 0, -Speed / 2.0f);

                    // [UpImg] �T�C�Y width�v���X
                    tmpImg.rectTransform.sizeDelta += new Vector2(Speed / 4.0f, 0);

                    // [UpImg] ���W x�v���X,y�v���X
                    tmpImg.transform.position += new Vector3(Speed / 12.0f, Speed / 15.0f, 0);

                    // [DownImg] ��] z�v���X
                    tmpImg2.transform.localEulerAngles += new Vector3(0, 0, Speed / 2.0f);

                    // [DownImg] �T�C�Y width�v���X
                    tmpImg2.rectTransform.sizeDelta += new Vector2(Speed / 4.0f, 0);

                    // [DownImg] ���W x�v���X,y�}�C�i�X
                    tmpImg2.transform.position += new Vector3(Speed / 12.0f, -Speed / 15.0f, 0);
                }

                // ���j���[�{�^��
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
        else // �J����
        {
            // [UpImg] ��]��40.0f�ɂȂ�܂�
            if (tmpImg.transform.localEulerAngles.z < 40.0f)
            {
                if (Screen.width > 1300)
                {
                    // [UpImg] ��] z�v���X
                    tmpImg.transform.localEulerAngles += new Vector3(0, 0, Speed);

                    // [UpImg] �T�C�Y width�}�C�i�X
                    tmpImg.rectTransform.sizeDelta += new Vector2(-Speed / 2.0f, 0);

                    // [UpImg] ���W x�}�C�i�X,y�}�C�i�X
                    tmpImg.transform.position += new Vector3(-Speed / 4.0f, -Speed / 5.0f, 0);

                    // [DownImg] ��] z�}�C�i�X
                    tmpImg2.transform.localEulerAngles += new Vector3(0, 0, -Speed);

                    // [DownImg] �T�C�Y width�}�C�i�X
                    tmpImg2.rectTransform.sizeDelta += new Vector2(-Speed / 2.0f, 0);

                    // [DownImg] ���W x�}�C�i�X,y�v���X
                    tmpImg2.transform.position += new Vector3(-Speed / 4.0f, Speed / 5.0f, 0);
                }
                else
                {
                    // [UpImg] ��] z�v���X
                    tmpImg.transform.localEulerAngles += new Vector3(0, 0, Speed / 2.0f);

                    // [UpImg] �T�C�Y width�}�C�i�X
                    tmpImg.rectTransform.sizeDelta += new Vector2(-Speed / 4.0f, 0);

                    // [UpImg] ���W x�}�C�i�X,y�}�C�i�X
                    tmpImg.transform.position += new Vector3(-Speed / 12.0f, -Speed / 15.0f, 0);

                    // [DownImg] ��] z�}�C�i�X
                    tmpImg2.transform.localEulerAngles += new Vector3(0, 0, -Speed / 2.0f);

                    // [DownImg] �T�C�Y width�}�C�i�X
                    tmpImg2.rectTransform.sizeDelta += new Vector2(-Speed / 4.0f, 0);

                    // [DownImg] ���W x�}�C�i�X,y�v���X
                    tmpImg2.transform.position += new Vector3(-Speed / 12.0f, Speed / 15.0f, 0);
                }

                // ���j���[�{�^��
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

    // �Q�[���J�n��UI
    void Load()
    {
        // �c�e���e�L�X�g
        for (int i = 0; i < BulletTx.Length; i++)
        {
            BulletTx[i] = GameObject.Find(TextSt[i]).GetComponent<Text>(); // �R���|�[�l���g�擾
        }

        // ���b�N�I�u�W�F�N�g(Image)
        for (int j = 0; j < LockObj.Length; j++)
        {
            LockObj[j] = GameObject.Find(LockSt[j]); // Find
        }

        // ����w�iImage
        if (Screen.width > 1300)
        {
            mainWeapon = Instantiate(weaponIMG[0], new Vector3(1462, 871, 0), Quaternion.identity); // ����
            subWeapon = Instantiate(weaponIMG[0], new Vector3(1462, 648, 0), Quaternion.identity);
            specialWeapon = Instantiate(weaponIMG[0], new Vector3(1462, 323, 0), Quaternion.identity);
        }
        else
        {
            mainWeapon = Instantiate(weaponIMG[0], new Vector3(975, 585, 0), Quaternion.identity); // ����
            subWeapon = Instantiate(weaponIMG[0], new Vector3(975, 434, 0), Quaternion.identity);
            specialWeapon = Instantiate(weaponIMG[0], new Vector3(975, 211, 0), Quaternion.identity);
        }

        mainWeapon.transform.parent = GameObject.Find("Weapon1").transform; // �e�q�t��
        subWeapon.transform.parent = GameObject.Find("Weapon2").transform;
        specialWeapon.transform.parent = GameObject.Find("sWeapon1").transform;
        mainWeapon.transform.SetAsFirstSibling();
        subWeapon.transform.SetAsFirstSibling();
        specialWeapon.transform.SetAsFirstSibling();
        mainWeaponBackIMG = mainWeapon.GetComponent<Image>(); // �R���|�[�l���g�擾
        subWeaponBackIMG = subWeapon.GetComponent<Image>();
        specialWeaponBackIMG = specialWeapon.GetComponent<Image>();

        // ���j���[Image(���X�^�[�g, �^�C�g����)
        if (Screen.width > 1300)
        {
            reStart = Instantiate(menuButton[0], new Vector3(1870, 70, 0), Quaternion.identity); // ����
            title = Instantiate(menuButton[1], new Vector3(1870, 70, 0), Quaternion.identity);
        }
        else
        {
            reStart = Instantiate(menuButton[0], new Vector3(1260, 49, 0), Quaternion.identity); // ����
            title = Instantiate(menuButton[1], new Vector3(1260, 49, 0), Quaternion.identity);
        }
        reStart.transform.parent = GameObject.Find("MiniMenu").transform; // �e�q�t��
        title.transform.parent = GameObject.Find("MiniMenu").transform;
        reStartIMG = reStart.GetComponent<Image>(); // �R���|�[�l���g�擾
        titleIMG = title.GetComponent<Image>();
    }

    // �G���j���̊ȈՃX�R�A�\��
    public void E_ScoreTxInst(Transform Tf, int score)
    {
        // �X�R�A����
        GameObject tx = Instantiate(EnemyScoreText, new Vector3(0, 0, 0), Quaternion.identity);
        tx.GetComponent<Text>().text = "+" + score;

        tx.transform.parent = GameObject.Find("Canvas").transform;

        // �G�̍��W��UI���W�ɕϊ�
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

    // �G��HP�o�[����
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

    // �{�XHP�o�[�\��
    public void Boss_HPbarInitialize(int Boss, int HP)
    {
        // �{�X�̎�ނɂ��HP�o�[�̐��l����
        BossInt = Boss;
        Slider.SetActive(true);
        slider.maxValue = HP;
        valueHP = HP;
        slider.value = HP;
    }

    // �{�XHP�o�[���l�X�V
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