using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioSettings))]
public class RandomAudioPlayer : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip[] defulatClips;

    private void Awake()
    {
        audioSource =transform.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            throw new System.Exception("Œ¥≤È—ØµΩ“Ù‘¥");

        }
    }

    public void PlayerRandomAudio()
    {
        int index=Random.Range(0,defulatClips.Length);
        audioSource.clip = defulatClips[index];
        audioSource.Play();
    }
}
