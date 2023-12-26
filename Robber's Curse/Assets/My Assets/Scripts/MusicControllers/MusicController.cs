using System.Collections;
using UnityEngine;
/* Music controller class */
public class MusicController : MonoBehaviour
{
    // Variables and references
    private AudioSource audioSrc;
    public AudioClip[] tracks;
    // 0 - Main Menu
    // 1 - Level 1
    // 2 - tavern
    // 3 - Boss 1
    // 4 - Level 2
    // 5 - Boss 2
    // 6 - Level 3
    // 7 - Boss 3
    // 8 - Level 4
    // 9 - Boss 4
    public int currentTrackIndex = 0;
    public int PrevTrackIndex = 0;
    private bool isChangingTrack = false;
    // Start is called before the first frame update
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
    // Play a chosen track
    void PlayTrack(int index)
    {
        audioSrc.clip = tracks[index];
        audioSrc.Play();
    }
    // Change current track
    public void ChangeTrack(int newTrackIndex)
    {
        if (!isChangingTrack)
        {
            StartCoroutine(ChangeTrackRoutine(newTrackIndex));
        }
    }
    // Play previous track
    public void RestorePrevTrack()
    {
        if (!isChangingTrack && PrevTrackIndex != 2)
        {
            StartCoroutine(ChangeTrackRoutine(PrevTrackIndex));
        }
    }
    // Changing track slowly
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
        if (currentTrackIndex != 2)
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
