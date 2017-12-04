using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootHoldCtrl : MonoBehaviour
{
    private Transform MyTransform;

    private MapCreator GameMgr = null;

    [HideInInspector]
    public float MySpeed = 1.0f;

    // Use this for initialization
    void Start()
    {
        GameMgr = GameObject.Find("GameMgr").GetComponent<MapCreator>();

        MyTransform = GetComponent<Transform>();

        int RandomValue = (int)Random.Range(0.0f, 10.0f);

        if((RandomValue % 2) == 0)
        {
            MySpeed = 1.0f;
        }
        else
        {
            MySpeed = -1.0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (MyTransform.position.x > 2.5f)
        {
            //MySpeed *= -1.0f;

            MySpeed = (1.0f + (0.4f * (float)GameMgr.GameLevel)) * -1.0f;            
            //차이 값 : 4 / 10 = 0.4; 레벨 당 0.4;

            //게임 난이도에 따라서 속도 값에 변화를 줘야한다.
        }
        else if (MyTransform.position.x < -2.5f)
        {
            //MySpeed *= -1.0f;
            MySpeed = (1.0f + (0.4f * (float)GameMgr.GameLevel)) * 1.0f;
        }

        MyTransform.Translate(Vector3.right * MySpeed * Time.deltaTime, Space.Self);

        if (GameMgr.IsDelete(this.gameObject))
        {
            Destroy(this.gameObject);
        }
    }
}
