using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*-----------------------------------------------
 * タイトル画面とリザルト画面の画面境界線座標調整
 ----------------------------------------------*/

public class initializeScript : MonoBehaviour
{
    Image borderIMG;

    int screenSizeInt = 0;

    private void Start()
    {
        borderIMG = GameObject.Find("border").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        borderIMG.transform.position = new Vector3(Screen.width / 3 * 2, Screen.height / 2, 0);
    }
}
