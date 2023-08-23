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
            throw new System.Exception("�����д��ڶ�� BlackMaskView");
        }
        Instance = this;
    }
    private void Start()
    {
        
        image = transform.GetComponent<Image>();
    }
    //͸���ȴ�1-0
    public IEnumerator FadeIn()
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1);//����image����ɫ(RGB)�Լ�͸�������ֵ1
        yield return null;
        while (image.color.a > 0)//image.color.aΪͼ��͸��ֵ����
        {
            yield return null;
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a - Time.deltaTime);//ͼ�����ֵ�͸������֡��Ϊ0��
        }
    }
    //͸���ȴ�0-1
    public IEnumerator FadeOut()
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0);//����image����ɫ(RGB)�Լ�͸������Сֵ0
        yield return null;
        while (image.color.a < 1)//image.color.aΪͼ��͸��ֵ����
        {
            yield return null;
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a + Time.deltaTime);//ͼ�����ֵ�͸������֡�ӵ�1����ȫ���֣�
        }
      

    }

    private void OnDestroy()//
    {
        Instance = null;
    }
}
