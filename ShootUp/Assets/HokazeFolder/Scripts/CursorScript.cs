using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*---------------------------------
 * カーソルオブジェクト
 --------------------------------*/

public class CursorScript : MonoBehaviour
{
    GameObject Aim;

    private void Start()
    {
        Aim = GameObject.Find("Cursor");
    }

    private void Update()
    {
        Vector3 mousePos = Input.mousePosition;

        Aim.transform.position = mousePos;
    }
}
