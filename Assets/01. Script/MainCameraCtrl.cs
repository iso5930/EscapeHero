using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraCtrl : MonoBehaviour
{
    private Transform MyTransform;

    public Transform PlayerTransform;

    private Vector3 NewPosition;

    private void Awake()
    {
        Screen.SetResolution(720, 1280, false);
    }

    // Use this for initialization
    void Start ()
    {
        MyTransform = GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(PlayerTransform.gameObject.GetComponent<PlayerCtrl>().IsJumpEnd)
        {
            NewPosition = MyTransform.position;
            NewPosition.y = PlayerTransform.position.y + 3.0f;
        }

        if(Vector3.Distance(NewPosition, MyTransform.position) >= 0.0f)
        {
            Vector3 vDir = NewPosition - MyTransform.position;

            MyTransform.position += vDir * Time.deltaTime;
        }
    }
}
