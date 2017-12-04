using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultText : MonoBehaviour
{
    private Text ScoreText;

    // Use this for initialization
    void Start ()
    {
        ScoreText = GetComponent<Text>();

        ScoreText.text = GameObject.Find("ScoreMgr").GetComponent<ScoreMgrCtrl>().Score.ToString();

        ScoreText.text += " M";
	}
}
