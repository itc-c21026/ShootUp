using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    public int ScoreCount;
    public int ReceiveScore;
    void Start()
    {
        
    }
    void Update()
    {
        
    }
    public void Score_Plus()
    {
        ScoreCount += ReceiveScore;
    }
}
