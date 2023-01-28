using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*--------------------------------------------
 �{�^���S�ʂ̃v���O����
--------------------------------------------*/

public class ButtonScript : MonoBehaviour
{
    // �X�N���v�g ****************************
    UIScript UIsc;

    // �I�u�W�F�N�g **************************
    GameObject Pobj;

    private void Start()
    {
        UIsc = GameObject.Find("GC").GetComponent<UIScript>();

        Pobj = GameObject.Find("Player");
    }

    private void Update()
    {
        if (Pobj != null)
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape)) MENUCHECK();
    }

    public void MENUCHECK() // ���j���[�֘A
    {
        Image tmpImg = UIsc.pauseIMG.GetComponent<Image>();
        if (UIsc.menuCheck)
        {
            Pobj.SendMessage("Pause");
            tmpImg.color = new Color(0, 0.23f, 0.3f, 1f);
            UIsc.menuCheck = !UIsc.menuCheck;
        }
        else
        {
            Pobj.SendMessage("Pause");
            tmpImg.color = new Color(0, 0.49f, 0.64f, 1f);
            UIsc.menuCheck = !UIsc.menuCheck;
        }
    }
}
