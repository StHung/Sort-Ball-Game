using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static AudioManager Instance { get { return _instance; } }

    public List<AudioClip> backgroundMusics;

    [SerializeField] AudioSource soundAus;

    [SerializeField] AudioSource musicAus;

    private Queue<AudioClip> musics;

    private bool isQuiting;
    private AudioClip currentMusic;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(_instance);
        }
    }

    private void Start()
    {
        musics = new Queue<AudioClip>();
        foreach (AudioClip item in backgroundMusics)
        {
            musics.Enqueue(item);
        }
    }

    IEnumerator PlayBGMusic()
    {
        while (!isQuiting)
        {
            currentMusic = musics.Dequeue();
            musicAus.clip = currentMusic;
            yield return new WaitForSeconds(musicAus.clip.length);
            musics.Enqueue(currentMusic);
        }
    }


    private void OnApplicationQuit()
    {
        isQuiting = true;
    }

    public void PlayGameWinSound()
    {
        AudioClip winSFX = Resources.Load<AudioClip>("winSFX");
        soundAus.PlayOneShot(winSFX);
    }

    public void PlayTouchSound()
    {
        AudioClip touchSFX = Resources.Load<AudioClip>("touchSFX");
        soundAus.PlayOneShot(touchSFX);
    }
}