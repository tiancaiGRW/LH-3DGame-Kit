using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GameController : MonoBehaviour
{
    public bool isShowCursor = false;

    public ViewBase pauseView;

    private void Awake()
    {
        //隐藏鼠标
        ShowCursor(isShowCursor);
    }
    private void Update()
    {
        if (PlayerInput.instance != null && PlayerInput.instance.Pause)
        {
           Pause(true);
        }
    }
    public void ShowCursor(bool isShow)
    {
        Cursor.visible = isShow;
        //锁定鼠标
        Cursor.lockState = isShow ? CursorLockMode.None : CursorLockMode.Locked;//isShow = true,不锁定鼠标（None）,isShow = false,锁定鼠标    //是一个判断写法
    }

    public void Pause(bool isPause)
    {
        //游戏暂停，显示鼠标
        ShowCursor(isPause);
        //游戏暂停，失去控制权
        if (isPause && PlayerInput.instance != null)
        {
            PlayerInput.instance.LostConteol();
        }
        else
        {
            PlayerInput.instance.GainControl();
        }
        //停止游戏逻辑
        Time.timeScale = isPause ? 0 : 1 ;//isPause = true,timeScale = 0 停止游戏逻辑，否则tineScale=1,不停游戏逻辑
        //显示暂停界面
        if (isPause)
        {
            pauseView.Show();
        }
        else
        {
            pauseView.Hide();
        }
    }

    public void ExitGame()
    {
#if    UNITY_EDITOR
        EditorApplication.isPlaying = false;
            #else
        Application.Quit();
#endif
    }

    public void Restart()
    {
        Pause(false);
        transform.GetComponent<SceneChange>().Change();
    }
}
