using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*----------------------------
 �V�[���J�ڃv���O����
----------------------------*/

public class SceneScript : MonoBehaviour
{

    public void InGame()
    {
        SceneManager.LoadScene("GamePlayScene");
    }

    public void InTitle()
    {
        SceneManager.LoadScene("TitleScene");   
    }

    public void InResult()
    {
        SceneManager.LoadScene("ResultScene");
    }

    public void GameExit()
    {
        Application.Quit();//�Q�[���v���C�I��
    }
}
