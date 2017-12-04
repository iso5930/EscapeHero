using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerCtrl : MonoBehaviour
{

    private Transform MyTransform;
    private float MySpeed = 1.0f;

    private float MyPower = 0.0f;

    private float MAX_POWER = 250.0f;

    [HideInInspector]
    public bool IsJump = false;

    [HideInInspector]
    public bool IsJumpEnd = false;

    [HideInInspector]
    public float FootHoldStayTime = 0.0f;

    private GameObject FootHoldTimer;

    public GameObject PowerUI;

    public float JumpFigures = 0.9f;

    public Image PowerUI_Image;

    public Image EvilutionUI_Image;

    public float ALiveTime = 0.0f;

    public Sprite[] PlayerImage;

    private int FootHoldCnt = 0; //착지 발판 Cnt

    public enum FOOTHOLD_TYPE
    {
        FOOTHOLD_NONE,
        FOOTHOLD_NORMAL,
        FOOTHOLD_TIME,
        FOOTHOLD_DISPOSABLE,
        FOOTHOLD_JUMPDOWN,
        FOOTHOLD_JUMPUP
    };

    [HideInInspector]
    public int GameLevel = 0;

    [HideInInspector]
    public int GameScore = 0;

    private float FootHoldSpeed = 0.0f;

    private FOOTHOLD_TYPE OverlabFootHold = FOOTHOLD_TYPE.FOOTHOLD_NONE;

    public GameObject GameMgr = null;

    private bool IsPress = false;

    private float PowerValue = 1.0f;

    private GameObject FinalFootHold = null;
        
    // Use this for initialization
    void Start ()
    {
        MyTransform = GetComponent<Transform>();
        
        FootHoldTimer = GameObject.FindGameObjectWithTag("TIMER");

        FootHoldTimer.SetActive(false);

        PowerUI.SetActive(false);
    }
	
	// Update is called once per frame
	void Update ()
    {
        //플레이어 사망 조건 체크

        PlayerDeadCheck();
        
        //MAX_POWER = 280.0f + (GameLevel * 38.0f); //max = 380 + 280 = 660.0f; //1LV == 55.0f

        MAX_POWER = 280.0f + (GameLevel * 64.0f);

        GetComponent<SpriteRenderer>().sprite = PlayerImage[GameLevel];

        //좌우 이동
        float h = 0.0f;

        if (Application.platform == RuntimePlatform.Android)
        {
            h = GameMgr.GetComponent<BtnCtrl>().Speed;
        }
        else
        {
            h = Input.GetAxis("Horizontal");
        }

        //Debug.Log("Player Horizontal : " + h.ToString());

        if (h > 0) //0 이상 //플레이어의 진행 방향이 >>
        {
            if(FootHoldSpeed < 0) //발판의 진행 방향이 <<
            {
                h = h * 2.0f;
            }
        }
        else if(h < 0) //0 이하 //플레이어의 진행 방향이 <<
        {
            if(FootHoldSpeed > 0) //발판의 진행 방향이 >>
            {
                h = h * 2.0f;
            }
        }

        MyTransform.Translate(Vector3.right * MySpeed * h * Time.deltaTime, Space.Self);

        if (Application.platform != RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                MyPower += MAX_POWER * Time.deltaTime * 2.0f * PowerValue;

                if (MyPower >= MAX_POWER)
                {
                    PowerValue = -1.0f;
                }
                else if(MyPower <= 0.0f)
                {
                    PowerValue = 1.0f;
                }

                PowerUI.SetActive(true);

                float fPersent = MyPower / MAX_POWER * 100.0f;
                float fPersent2 = 53.0f * fPersent / 100.0f;

                PowerUI_Image.rectTransform.sizeDelta = new Vector2(fPersent2, 3.0f);
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                MyPower *= JumpFigures;

                GetComponent<Rigidbody2D>().AddForce(new Vector2(0.0f, MyPower));

                MyPower = 0.0f;

                IsJump = true;

                GetComponent<BoxCollider2D>().enabled = false;

                PowerUI.SetActive(false);
            }
        }

        if (IsPress)
        {
            OnJumpButtonPress();
        }

        if(IsJump)
        {
            if (GetComponent<Rigidbody2D>().velocity.y < 0)
            {
                IsJump = false;

                GetComponent<BoxCollider2D>().enabled = true;
            }
        }

        if(MyTransform.position.x >= 2.6f)
        {
            Vector3 vPos = MyTransform.position;

            vPos.x = 2.6f;

            MyTransform.position = vPos;
        }
        else if(MyTransform.position.x <= -2.6f)
        {
            Vector3 vPos = MyTransform.position;

            vPos.x = -2.6f;

            MyTransform.position = vPos;
        }
    }

    public void OnJumpButtonPress()
    {
        MyPower += MAX_POWER * Time.deltaTime * 2.0f * PowerValue;

        if (MyPower >= MAX_POWER)
        {
            PowerValue = -1.0f;
        }
        else if (MyPower <= 0.0f)
        {
            PowerValue = 1.0f;
        }

        PowerUI.SetActive(true);

        float fPersent = MyPower / MAX_POWER * 100.0f;
        float fPersent2 = 53.0f * fPersent / 100.0f;

        PowerUI_Image.rectTransform.sizeDelta = new Vector2(fPersent2, 3.0f);
    }

    public void OnJumpButtonDown()
    {
        IsPress = true;
    }

    public void OnJumpButtonUp()
    {
        IsPress = false;

        MyPower *= JumpFigures;

        GetComponent<Rigidbody2D>().AddForce(new Vector2(0.0f, MyPower));

        MyPower = 0.0f;

        IsJump = true;

        GetComponent<BoxCollider2D>().enabled = false;

        PowerUI.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "FOOTHOLD") //기본 발판.
        {
            OverlabFootHold = FOOTHOLD_TYPE.FOOTHOLD_NORMAL;
        }
        else if(collision.gameObject.tag == "FOOTHOLD_TIME") //시간 제한 발판
        {
            OverlabFootHold = FOOTHOLD_TYPE.FOOTHOLD_TIME;

            FootHoldTimer.SetActive(true);
        }
        else if(collision.gameObject.tag == "FOOTHOLD_DISPOSABLE") //일회용 발판
        {
            OverlabFootHold = FOOTHOLD_TYPE.FOOTHOLD_DISPOSABLE;
        }
        else if(collision.gameObject.tag == "FOOTHOLD_JUMPDOWN") //점프 다운 발판
        {
            OverlabFootHold = FOOTHOLD_TYPE.FOOTHOLD_JUMPDOWN;
        }
        else if (collision.gameObject.tag == "FOOTHOLD_JUMPUP") //점프 업 발판
        {
            OverlabFootHold = FOOTHOLD_TYPE.FOOTHOLD_JUMPUP;
        }

        if(OverlabFootHold != FOOTHOLD_TYPE.FOOTHOLD_NONE)
        {
            //충돌한것이 발판일때.

            float fY = transform.position.y; //플레이어의 Y값.

            float fFootHoldY = collision.gameObject.transform.position.y;

            if(fY > fFootHoldY && FinalFootHold != collision.gameObject)
            {
                //플레이어의 Y가 더 위에있다.
                //위에서 떨어졌다.
                //착지를 했다.
                ++FootHoldCnt;

                FinalFootHold = collision.gameObject;

                Debug.Log("착지 카운트 : " + FootHoldCnt.ToString());

                //PlayerY = transform.position.y + 4.2f;
                ////    PlayerY = PlayerY % 10.0f;

                ////    float f1 = PlayerY / 10.0f * 100.0f;
                ////    float f2 = 45.0f * f1 / 100.0f;

                ////    EvilutionUI_Image.rectTransform.sizeDelta = new Vector2(3.0f, f2);

                float f1 = 0.0f;
                float f2 = 0.0f;

                switch (GameLevel)
                {
                    case 0:

                    case 1:

                        f1 = FootHoldCnt / 7.0f * 100.0f;
                        f2 = 45.0f * f1 / 100.0f;

                        if (FootHoldCnt >= 7)
                        {
                            FootHoldCnt = 0;
                            ++GameLevel;

                            f2 = 0.0f;
                        }

                        break;

                    case 2:

                        f1 = FootHoldCnt / 6.0f * 100.0f;
                        f2 = 45.0f * f1 / 100.0f;

                        if (FootHoldCnt >= 6)
                        {
                            FootHoldCnt = 0;
                            ++GameLevel;

                            f2 = 0.0f;
                        }

                        break;

                    case 3:

                        f1 = FootHoldCnt / 5.0f * 100.0f;
                        f2 = 45.0f * f1 / 100.0f;

                        if (FootHoldCnt >= 5)
                        {
                            FootHoldCnt = 0;
                            ++GameLevel;

                            f2 = 0.0f;
                        }
                        break;

                    case 4:

                    case 5:

                        f1 = FootHoldCnt / 4.0f * 100.0f;
                        f2 = 45.0f * f1 / 100.0f;

                        if (FootHoldCnt >= 4)
                        {
                            FootHoldCnt = 0;
                            ++GameLevel;

                            f2 = 0.0f;
                        }

                        break;
                }

                EvilutionUI_Image.rectTransform.sizeDelta = new Vector2(3.0f, f2);

            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(OverlabFootHold != FOOTHOLD_TYPE.FOOTHOLD_NONE)
        {
            //GameLevel = (int)((transform.position.y + 4.2f) / 10.0f); //40.0f로 수정 //최대 7.0f; 20.0f; // 100m

            //if (GameLevel >= 6)
            //    GameLevel = 6;

            //위의 GameLevel 조건도 바꾸고.

            float Y = transform.position.y;

            FootHoldStayTime += Time.deltaTime;

            Y += 4.2f;

            GameScore = (int)(Y * 10);

            FootHoldSpeed = collision.gameObject.GetComponent<FootHoldCtrl>().MySpeed;
            
            MyTransform.Translate(Vector3.right * FootHoldSpeed * Time.deltaTime, Space.Self); //이 스피드 값을 가져와야 하나..?

            if (OverlabFootHold == FOOTHOLD_TYPE.FOOTHOLD_TIME)
            {
                string strTimer = collision.gameObject.GetComponent<FootHold_Time>().SpareTime.ToString();

                if (strTimer.Length > 3)
                {
                    strTimer = strTimer.Substring(0, 3);
                }

                FootHoldTimer.GetComponent<Text>().text = strTimer;
            }
            else if (OverlabFootHold == FOOTHOLD_TYPE.FOOTHOLD_JUMPDOWN)
            {
                JumpFigures = 0.9f;
            }
            else if (OverlabFootHold == FOOTHOLD_TYPE.FOOTHOLD_JUMPUP)
            {
                JumpFigures = 1.1f;
            }
            
            if(!IsJump)
            {
                IsJumpEnd = true;
            }
        }      
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        OverlabFootHold = FOOTHOLD_TYPE.FOOTHOLD_NONE;

        FootHoldTimer.SetActive(false);

        JumpFigures = 1.0f;

        IsJumpEnd = false;

        FootHoldSpeed = 0.0f;

        FootHoldStayTime = 0.0f;
    }

    private void PlayerDeadCheck()
    {
        float PlayerY = transform.position.y;

        int iCnt = 0;

        GameObject[] FootHolds = GameObject.FindGameObjectsWithTag("FOOTHOLD");

        foreach (GameObject FootHold in FootHolds)
        {
            float FootHoldY = FootHold.transform.position.y;

            if (FootHoldY < PlayerY)
            {
                ++iCnt;
            }
        }

        FootHolds = GameObject.FindGameObjectsWithTag("FOOTHOLD_TIME");

        foreach (GameObject FootHold in FootHolds)
        {
            float FootHoldY = FootHold.transform.position.y;

            if (FootHoldY < PlayerY)
            {
                ++iCnt;
            }
        }

        FootHolds = GameObject.FindGameObjectsWithTag("FOOTHOLD_DISPOSABLE");

        foreach (GameObject FootHold in FootHolds)
        {
            float FootHoldY = FootHold.transform.position.y;

            if (FootHoldY < PlayerY)
            {
                ++iCnt;
            }
        }

        FootHolds = GameObject.FindGameObjectsWithTag("FOOTHOLD_JUMPDOWN");

        foreach (GameObject FootHold in FootHolds)
        {
            float FootHoldY = FootHold.transform.position.y;

            if (FootHoldY < PlayerY)
            {
                ++iCnt;
            }
        }

        FootHolds = GameObject.FindGameObjectsWithTag("FOOTHOLD_JUMPUP");

        foreach (GameObject FootHold in FootHolds)
        {
            float FootHoldY = FootHold.transform.position.y;

            if (FootHoldY < PlayerY)
            {
                ++iCnt;
            }
        }

        if (iCnt == 0)
        {
            ALiveTime += Time.deltaTime;

            if (ALiveTime >= 1.0f)
            {
                Destroy(this.gameObject);

                SceneManager.LoadScene("ResultScene");
            }
        }
        else
            ALiveTime = 0.0f;
    }
}
