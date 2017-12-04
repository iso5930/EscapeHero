using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreator : MonoBehaviour
{
    private PlayerCtrl Player = null;

    private GameObject LastFootHold = null;

    public GameObject[] FootHoldList;

    public int GameLevel = 1;
    
    // Use this for initialization
    void Start ()
    {
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCtrl>();

        //최초 시작시 발판 생성. 플레이어와 1차이 나도록.

        float PlayerY = -4.5f;

        for(int i = 1; i < 11; ++i)
        {
            float X = Random.Range(-2.5f, 2.5f);

            float Y = (i * 1.0f) + PlayerY;

            GameObject NewFootHold = GameObject.Instantiate(this.FootHoldList[0]) as GameObject; //as?

            NewFootHold.transform.position = new Vector3(X, Y, 0.0f);

            LastFootHold = NewFootHold;
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        float FootHoldY = LastFootHold.transform.position.y;
        float PlayerY = Player.transform.position.y;

        //플레이어 위치에서 4.2를 보정한 값에 * 10.0f가 현재 m값. Y가 400 - 4.2니깐 Level 디자인은 396까지.

        //400까지고 레벨 단위는 40단위니깐..

        //400 / 40 = 10;

        //Game

        GameLevel = (int)((PlayerY + 14.2f) / 20.0f); //0 ~ 10까지의 값이 나온다..  //4.0 나중에 40.0f로 수정 //20.0f로 수정

        //GameLevel += 1;

        //Debug.Log("Map Level : " + GameLevel.ToString());

        if (GameLevel >= 10)
            GameLevel = 10;
       
        if (FootHoldY - PlayerY < 10.0f) //마지막으로 생성한 발판이 일정 값이하로 내려가면 새로운 발판을 생성한다.
        {
            float X = Random.Range(-2.5f, 2.5f);

            float Y = FootHoldY + 1.0f + (0.5f * GameLevel); //난이도에 따른 계수 값 추가.

            int iIndex = (int)Random.Range(0.0f, 4.9f);

            GameObject NewFootHold = GameObject.Instantiate(this.FootHoldList[iIndex]) as GameObject; //as?

            NewFootHold.transform.position = new Vector3(X, Y, 0.0f);

            float NewScaleX = 1.0f - (float)(0.08f * GameLevel);

            Vector3 NewScale = NewFootHold.transform.localScale;
            NewScale.x = NewScaleX;

            NewFootHold.transform.localScale = NewScale;
            //NewFootHold.transform.localScale.x = 1.1f;

            //4000m 까지..?

            //최소 길이 0.2f

            //100m 당... 흠 너무 김.

            //Level = 1 ~ 10; 400m 마다 LevelUp;

            //0.8 / 10 = 0.08; 1.0 - 0.16

            LastFootHold = NewFootHold;
        }
	}

    public bool IsDelete(GameObject FootHold)
    {
        float FootHoldY = FootHold.transform.position.y;

        float PlayerY = Player.transform.position.y;

        if(PlayerY > FootHoldY)
        {
            float fDist = Mathf.Abs(FootHoldY - PlayerY);

            if (fDist >= 1.5f && Player.FootHoldStayTime >= 0.2f)
            {
                return true;
            }
        }

        return false;
    }
}
