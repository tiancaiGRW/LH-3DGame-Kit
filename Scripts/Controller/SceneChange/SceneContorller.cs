using System;
using System.Collections;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneContorller : MonoBehaviour
{

	#region 字段

	private int currentIndex;
	private Action<float> onProgressChange;
	private Action onFinsh;
	private static SceneContorller _instacne;

	#endregion

	#region 属性
	public static SceneContorller Instance
	{
		get
		{
			if (_instacne == null)
			{
				GameObject obj = new GameObject("SceneController");
				obj.AddComponent<SceneContorller>();
			}

			return _instacne;
		}
	}
    #endregion

    #region Unity生命周期

    private void Awake()
	{
		DontDestroyOnLoad(gameObject);
		if (_instacne != null)
		{
			throw new Exception("场景中存在多个 SceneCotroller");
		}

		_instacne = this;
	}
    #endregion

    #region 方法
    public void LoadScene(int index,Action<float> onProgressChange,Action onFinsh)
	{
		this.currentIndex = index;
		this.onProgressChange = onProgressChange;
		this.onFinsh = onFinsh;
		StartCoroutine(LoadScene());
	}

	private IEnumerator LoadScene()
	{
		yield return null;
		AsyncOperation anyncOperation = SceneManager.LoadSceneAsync(this.currentIndex);
		while (!anyncOperation.isDone)
		{
			yield return null;
			onProgressChange?.Invoke(anyncOperation.progress);//if语句便携写法。
		}

		yield return new WaitForSeconds(1f);
		onFinsh?.Invoke();
	}
    #endregion

}