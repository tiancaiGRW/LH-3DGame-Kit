using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadView : ViewBase
{
    #region 字段
    public Slider slider_progress;
    public Text text_progress;
    #endregion

    #region 生命周期
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    #region 方法

    public void UpdateProgress(float progress)
    {
        slider_progress.value = progress;
        
    }

    public void OnsliderProgressValueChange(float v)
    {
        text_progress.text = string.Format("{0}%", Mathf.Round(v * 100));
    }
    #endregion
}
