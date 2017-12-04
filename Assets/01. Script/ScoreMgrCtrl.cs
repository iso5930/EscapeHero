using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreMgrCtrl : MonoBehaviour
{
    [HideInInspector]
    public int Score = 0;

	// Use this for initialization
	void Start ()
    {
        DontDestroyOnLoad(transform.gameObject);

        GameObject[] ScoreMgrs = GameObject.FindGameObjectsWithTag("SCORE_MGR");

        int iCnt = 0;

        foreach(GameObject ScoreMgr in ScoreMgrs)
        {
            ++iCnt;
        }

        if (iCnt > 1)
            Destroy(this.gameObject);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            //MainScene일때.
            Score = GameObject.Find("GameMgr").GetComponent<Score>().MaxScore;
        }
    }
}
