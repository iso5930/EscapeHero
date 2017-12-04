using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BtnCtrl : MonoBehaviour
{
    public float Speed = 0.0f;

    public void OnClickReStart()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void OnClickExit()
    {
        Application.Quit();
    }

    public void OnLeftButton()
    {
        Speed = -1.0f;
    }

    public void OnRightButton()
    {
        Speed = 1.0f;
    }

    public void OnDirButtonUp()
    {
        Speed = 0.0f;
    }

    public void OnJumpButton()
    {
        Debug.Log("점프 버튼");
    }
}
