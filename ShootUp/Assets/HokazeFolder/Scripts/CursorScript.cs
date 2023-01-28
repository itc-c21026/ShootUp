using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*---------------------------------
 * �J�[�\���I�u�W�F�N�g
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
