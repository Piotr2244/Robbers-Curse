using System.Collections;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    private AudioSource audioSrc;
    public AudioClip[] tracks;
    // 0 - Main Menu
    // 1 - Level 1
    // 2 - tavern
    // 3 - Boss 1
    public int currentTrackIndex = 0;
    public int PrevTrackIndex = 0;
    private bool isChangingTrack = false;

    void Start()
    {
        audioSrc = GetComponent<AudioSource>();
        PlayTrack(currentTrackIndex);
    }

    private void Awake()
    {
        TavernController.ChangeMusic += ChangeTrack;
        TavernController.RestoreMusic += RestorePrevTrack;
        GoblinBoss.ChangeMusic += ChangeTrack;
        GoblinBoss.RestoreMusic += RestorePrevTrack;
        DemonBoss.ChangeMusic += ChangeTrack;
        DemonBoss.RestoreMusic += RestorePrevTrack;
        EvilHero.ChangeMusic += ChangeTrack;
        EvilHero.RestoreMusic += RestorePrevTrack;
        WizardBoss.ChangeMusic += ChangeTrack;
        WizardBoss.RestoreMusic += RestorePrevTrack;
    }
    private void OnDisable()
    {
        TavernController.ChangeMusic -= ChangeTrack;
        TavernController.RestoreMusic -= RestorePrevTrack;
        GoblinBoss.ChangeMusic -= ChangeTrack;
        GoblinBoss.RestoreMusic -= RestorePrevTrack;
        DemonBoss.ChangeMusic -= ChangeTrack;
        DemonBoss.RestoreMusic -= RestorePrevTrack;
        EvilHero.ChangeMusic -= ChangeTrack;
        EvilHero.RestoreMusic -= RestorePrevTrack;
        WizardBoss.ChangeMusic -= ChangeTrack;
        WizardBoss.RestoreMusic -= RestorePrevTrack;
    }

    void PlayTrack(int index)
    {
        audioSrc.clip = tracks[index];
        audioSrc.Play();
    }
    public void ChangeTrack(int newTrackIndex)
    {
        if (!isChangingTrack)
        {
            StartCoroutine(ChangeTrackRoutine(newTrackIndex));
        }
    }
    public void RestorePrevTrack()
    {
        if (!isChangingTrack)
        {
            StartCoroutine(ChangeTrackRoutine(PrevTrackIndex));
        }
    }
    IEnumerator ChangeTrackRoutine(int newTrackIndex)
    {
        isChangingTrack = true;

        float fadeDuration = 1.0f;
        float startVol = audioSrc.volume;

        while (audioSrc.volume > 0)
        {
            audioSrc.volume -= startVol * Time.deltaTime / fadeDuration;
            yield return null;
        }
        audioSrc.Stop();
        audioSrc.volume = startVol;
        PrevTrackIndex = currentTrackIndex;
        currentTrackIndex = newTrackIndex;
        PlayTrack(newTrackIndex);

        while (audioSrc.volume < startVol)
        {
            audioSrc.volume += startVol * Time.deltaTime / fadeDuration;
            yield return null;
        }

        audioSrc.volume = startVol;
        isChangingTrack = false;
    }


}
