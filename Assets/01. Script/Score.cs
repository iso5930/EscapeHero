using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public GameObject PlayerObj;

    public GameObject ScoreObj;
    public GameObject MaxScoreObj;
    public GameObject LevelObj;

    private Text ScoreText;
    private Text MaxScoreText;
    private Text LevelText;

    [HideInInspector]
    public int MaxScore = 0;
    
	// Use this for initialization
	void Start ()
    {
        //DontDestroyOnLoad(transform.gameObject);

        ScoreText = ScoreObj.GetComponent<Text>();
        MaxScoreText = MaxScoreObj.GetComponent<Text>();
        LevelText = LevelObj.GetComponent<Text>();
        //MaxScoreText
    }
	
	// Update is called once per frame
	void Update ()
    {
        //Score
        float Y = PlayerObj.GetComponent<Transform>().position.y;

        Y += 4.2f;

        int iScore = (int)(Y * 10);
        //Score

        //int iScore = PlayerObj.GetComponent<PlayerCtrl>().GameScore;

        if (MaxScore < iScore)
            MaxScore = iScore;

        ScoreText.text = iScore.ToString() + " M";
        MaxScoreText.text = MaxScore.ToString() + " M";
        LevelText.text = PlayerObj.GetComponent<PlayerCtrl>().GameLevel.ToString();
    }
}
