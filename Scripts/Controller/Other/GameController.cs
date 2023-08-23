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
        //�������
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
        //�������
        Cursor.lockState = isShow ? CursorLockMode.None : CursorLockMode.Locked;//isShow = true,��������꣨None��,isShow = false,�������    //��һ���ж�д��
    }

    public void Pause(bool isPause)
    {
        //��Ϸ��ͣ����ʾ���
        ShowCursor(isPause);
        //��Ϸ��ͣ��ʧȥ����Ȩ
        if (isPause && PlayerInput.instance != null)
        {
            PlayerInput.instance.LostConteol();
        }
        else
        {
            PlayerInput.instance.GainControl();
        }
        //ֹͣ��Ϸ�߼�
        Time.timeScale = isPause ? 0 : 1 ;//isPause = true,timeScale = 0 ֹͣ��Ϸ�߼�������tineScale=1,��ͣ��Ϸ�߼�
        //��ʾ��ͣ����
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
