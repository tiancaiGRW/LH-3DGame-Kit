using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackMaskView : MonoBehaviour
{
    
    public static BlackMaskView Instance;
    private Image image;
  
    private void Awake()
    {
        
        if (Instance != null)
        {
            throw new System.Exception("场景中存在多个 BlackMaskView");
        }
        Instance = this;
    }
    private void Start()
    {
        
        image = transform.GetComponent<Image>();
    }
    //透明度从1-0
    public IEnumerator FadeIn()
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1);//设置image的颜色(RGB)以及透明度最大值1
        yield return null;
        while (image.color.a > 0)//image.color.a为图像透明值参数
        {
            yield return null;
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a - Time.deltaTime);//图像遮罩的透明度逐帧减为0；
        }
    }
    //透明度从0-1
    public IEnumerator FadeOut()
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0);//设置image的颜色(RGB)以及透明度最小值0
        yield return null;
        while (image.color.a < 1)//image.color.a为图像透明值参数
        {
            yield return null;
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a + Time.deltaTime);//图像遮罩的透明度逐帧加到1，完全遮罩；
        }
      

    }

    private void OnDestroy()//
    {
        Instance = null;
    }
}
