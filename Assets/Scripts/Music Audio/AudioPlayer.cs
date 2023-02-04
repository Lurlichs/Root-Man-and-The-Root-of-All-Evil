using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SFX
{
    public AudioClip audioClip;
    public string alias;
}

/// <summary>
/// Plays any SFX from anywhere. Just call AudioPlayer.Instance.PlayClip(...);
/// </summary>
public class AudioPlayer : MonoBehaviour
{
    public static AudioPlayer Instance;

    [SerializeField] private List<SFX> clips;
    [SerializeField] private AudioSource speaker;

    void Awake()
    {
        Instance = this;
    }

    public void PlayClip(AudioClip audioClip, float vol = 1, bool pitchVar = false)
    {
        if (pitchVar) { speaker.pitch = Random.Range(0.9f, 1.1f); }
        speaker.volume = vol;
        speaker.PlayOneShot(audioClip);
    }

    public void PlayClip(int audioClipId, float vol = 1, bool pitchVar = false)
    {
        if (pitchVar) { speaker.pitch = Random.Range(0.9f, 1.1f); }
        speaker.volume = vol;
        speaker.PlayOneShot(clips[audioClipId].audioClip);
    }

    public void PlayClip(string alias, float vol = 1, bool pitchVar = false)
    {
        if (pitchVar) { speaker.pitch = Random.Range(0.9f, 1.1f); }
        speaker.volume = vol;

        foreach(SFX clip in clips)
        {
            if(clip.alias == alias)
            {
                speaker.PlayOneShot(clip.audioClip);
                break;
            }
        }
    }


}
