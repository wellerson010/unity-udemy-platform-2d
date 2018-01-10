using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public AudioSource FXSource;
    public AudioSource MusicSource;

    public static SoundManager instance = null;

	void Awake () {
		if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	public void PlaySound(AudioClip audio)
    {
        FXSource.clip = audio;
        FXSource.Play();
    }
}
