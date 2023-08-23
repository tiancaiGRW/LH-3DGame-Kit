using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChange : MonoBehaviour
{
    public LoadView loadView;
    public int SceneIndex;
   public void Change()
    {
        if (loadView != null)
        {
            loadView.Show();
        }

        SceneContorller.Instance.LoadScene(SceneIndex, (progress) =>
        {
            if (loadView)
            {
                loadView.UpdateProgress(progress);
            }
        }, () =>
        {
            loadView.Close();
        });
    }
}
