using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionView : ViewBase
{
    public Slider music;
    public Slider sound;

    private void Awake()
    {
        music.value = PlayerPrefs.GetFloat(AduioManager.key_music, 0.5f);
        sound.value = PlayerPrefs.GetFloat(AduioManager.key_sound, 0.5f);
    }
    public void OnSliderMusicChange(float f)
    {
        PlayerPrefs.SetFloat(AduioManager.key_music, f);
        if(AduioManager.instance != null)
        {
         AduioManager.instance.UpdateVolume();
        }
    }
    public void OnSliderSoundChange(float f)
    {
        PlayerPrefs.SetFloat(AduioManager.key_sound, f);
        if (AduioManager.instance != null)
        {
            AduioManager.instance.UpdateVolume();
        }
    }
}
