using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{ 
    public static AudioManager instance;

    public AudioSource normalSong;
    public AudioSource bossSong;
    public AudioSource victorySong;
    public AudioSource deathSound;

    private AudioSource[] songsList;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        songsList = new AudioSource[3];
        songsList[0] = normalSong;
        songsList[1] = bossSong;
        songsList[2] = victorySong;
    }

    private void Start()
    {
        songsList[0].Play();
    }

    public IEnumerator Crossfade(int song1, int song2)
    {
        AudioSource s1 = songsList[song1];
        AudioSource s2 = songsList[song2];
        float currentTime = 0;
        float sourceVolume = s1.volume;
        while (currentTime < 1f)
        {
            s1.volume = Mathf.Lerp(sourceVolume, 0f, currentTime / 1f);
            currentTime += Time.deltaTime;
            yield return null;
        }
        s1.Stop();
        s1.volume = sourceVolume;
        s2.Play();
    }

    public void PlayDeathSound()
    {
        deathSound.Play();
    }
}
