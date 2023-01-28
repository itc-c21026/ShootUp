using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*-------------------------------------------
 * スコアのプログラム
 ------------------------------------------*/
public class ScoreNumberScript : MonoBehaviour
{
    int score;
    Text scoreTextUI;

    Text bossCntTx;

    ScoreImageScript[] numberis;

    Score ScoreSC;

    int bossCnt;

    // Start is called before the first frame update
    void Start()
    {
        if (this.gameObject.name == "Score")
        {
            ScoreSC = GameObject.Find("Admin").GetComponent<Score>();
            scoreTextUI = GameObject.Find("S").GetComponent<Text>();
        }else if (this.gameObject.name == "ResultScore")
        {
            bossCnt = PlayerPrefs.GetInt("BOSScnt");
            score = PlayerPrefs.GetInt("SCORE_RESULT");
            score = score + bossCnt * 50;
            scoreTextUI = GameObject.Find("tmpTx").GetComponent<Text>();
            bossCntTx = GameObject.Find("BossCntTx").GetComponent<Text>();
            bossCntTx.text = "BOSS SCOREBONUS " + bossCnt + " x" + bossCnt * 50;  
        }

        numberis = new ScoreImageScript[5];
        numberis[0] = GameObject.Find("Scores").GetComponent<ScoreImageScript>();
        numberis[1] = GameObject.Find("Scores2").GetComponent<ScoreImageScript>();
        numberis[2] = GameObject.Find("Scores3").GetComponent<ScoreImageScript>();
        numberis[3] = GameObject.Find("Scores4").GetComponent<ScoreImageScript>();
        numberis[4] = GameObject.Find("Scores5").GetComponent<ScoreImageScript>();

        numberis[0].nowNumber = 0;
        numberis[1].nowNumber = 0;
        numberis[2].nowNumber = 0;
        numberis[3].nowNumber = 0;
        numberis[4].nowNumber = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.name == "Score")
            score = ScoreSC.ScoreCount;

        string scoreText = score + "";
        scoreTextUI.text = scoreText;

        string[] getScoreOne = new string[5];
        int numberKeta = scoreText.Length;

        for (int i = 0; i <= numberKeta; i++)
        {
            if (i < numberKeta)
            {
                getScoreOne[i] = scoreText.Substring(scoreText.Length - (i + 1), 1);
                numberis[i].nowNumber = int.Parse(getScoreOne[i]);
            }
        }
    }
}
