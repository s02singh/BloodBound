using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public List<AudioClip> backgroundTracks = new List<AudioClip>();
    private AudioSource audioSource;
    private int currentTrackIndex = 0;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlayNextTrack();
    }

    void Update()
    {
        
        if (!audioSource.isPlaying)
        {
    
            PlayNextTrack();
        }
    }

    void PlayNextTrack()
    {

        if (backgroundTracks.Count == 0)
        {
            Debug.LogWarning("no background tracks");
            return;
        }


        audioSource.clip = backgroundTracks[currentTrackIndex];
        audioSource.Play();

       
        currentTrackIndex++;
        if (currentTrackIndex >= backgroundTracks.Count)
        {
            currentTrackIndex = 0; 
        }
    }
}
