using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Music
{
    public int id;

    public AudioClip main;
    
    /*
    public AudioClip regen;
    public AudioClip projectile;
    public AudioClip doubleJump;
    public AudioClip rootWave;
    public AudioClip shield;*/
}

/// <summary>
/// Plays music based on the song's title/alias. 
/// Use FadePlay to fade out current song first and Play to directly play the song.
/// </summary>
public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer Instance;

    [SerializeField] private List<Music> soundtracks;

    [SerializeField] private AudioSource speaker;
    [SerializeField] private Player player;

    void Awake()
    {
        Instance = this;
    }

    public void AssignPlayer(Player player)
    {
        this.player = player;
    }

    /// <summary>
    /// Use this when you want to play a song when another one is currently is playing.
    /// </summary>
    public void FadePlay(int id, float duration = 1, float intermission = 1)
    {
        StartCoroutine(FadeOut(id, duration, intermission));
    }

    public IEnumerator FadeOut(int id, float duration, float intermission)
    {
        float current = 0;

        while(current < 1)
        {
            current += Time.deltaTime / duration;

            speaker.volume = 1 - current;

            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(intermission);

        StartCoroutine(FadeInVolume(duration));
        Play(id);
    }

    // Fade in but only for volume
    public IEnumerator FadeInVolume(float duration)
    {
        float current = 0;

        while (current < 1)
        {
            current += Time.deltaTime / duration;

            speaker.volume = current;

            yield return new WaitForEndOfFrame();
        }
    }

    public void Play(int id)
    {
        FullVolume();

        Music soundtrack;

        foreach(Music music in soundtracks)
        {
            if(music.id == id)
            {
                /*
                soundtrack = music;

                speakers[0].clip = soundtrack.main;

                speakers[1].clip = soundtrack.regen;
                speakers[2].clip = soundtrack.projectile;
                speakers[3].clip = soundtrack.doubleJump;
                speakers[4].clip = soundtrack.rootWave;
                speakers[5].clip = soundtrack.shield;

                speakers[0].Play();

                if (player.GetPowerByName("regen").currentlyActive) { speakers[1].Play(); }
                if (player.GetPowerByName("projectile").currentlyActive) { speakers[2].Play(); }
                if (player.GetPowerByName("doubleJump").currentlyActive) { speakers[3].Play(); }
                if (player.GetPowerByName("rootWave").currentlyActive) { speakers[4].Play(); }
                if (player.GetPowerByName("shield").currentlyActive) { speakers[5].Play(); }

                break;*/

                soundtrack = music;

                speaker.clip = soundtrack.main;
                speaker.Play();
            }
        }
    }

    public void Mute()
    {
        speaker.volume = 0;
    }

    public void FullVolume()
    {
        speaker.volume = 1;
    }
}
