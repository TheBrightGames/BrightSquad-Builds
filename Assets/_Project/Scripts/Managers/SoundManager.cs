using UnityEngine;
using UnityEngine.InputSystem;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Trilhas de Menu")]
    public AudioClip mainMenuMusic;
    public AudioClip selectPlayerMusic;

    [Header("Trilhas de Gameplay")]
    public AudioClip gameplayRadioDefault; // faixa padrão ao entrar na gameplay

    bool mutedMusic;
    bool mutedSfx;

    const string MUSIC_MUTE_KEY = "MusicMuted";
    const string SFX_MUTE_KEY = "SfxMuted";

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // persiste entre cenas

        mutedMusic = PlayerPrefs.GetInt(MUSIC_MUTE_KEY, 0) == 1;
        mutedSfx = PlayerPrefs.GetInt(SFX_MUTE_KEY, 0) == 1;

        ApplyMuteStates();

        Debug.Log("SoundManager Awake – musicSource=" + musicSource + " mainMenuMusic=" + mainMenuMusic);
    }

    void ApplyMuteStates()
    {
        if (musicSource != null)
            musicSource.mute = mutedMusic;

        if (sfxSource != null)
            sfxSource.mute = mutedSfx;
    }

    void SaveMuteStates()
    {
        PlayerPrefs.SetInt(MUSIC_MUTE_KEY, mutedMusic ? 1 : 0);
        PlayerPrefs.SetInt(SFX_MUTE_KEY, mutedSfx ? 1 : 0);
        PlayerPrefs.Save();
    }

    // --------- Controle de música ---------

    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        if (musicSource == null || clip == null)
        {
            Debug.LogWarning("PlayMusic cancelado: musicSource ou clip nulo.");
            return;
        }

        if (musicSource.clip == clip && musicSource.isPlaying)
            return; // já está tocando essa

        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.Play();

        Debug.Log("PlayMusic: " + clip.name);
    }

    public void StopMusic()
    {
        if (musicSource != null)
            musicSource.Stop();
    }

    public void MuteMusic(bool value)
    {
        mutedMusic = value;
        ApplyMuteStates();
        SaveMuteStates();
    }

    public bool IsMusicMuted() => mutedMusic;

    // --------- Controle de SFX ---------

    public void PlaySfx(AudioClip clip)
    {
        if (sfxSource != null && clip != null && !mutedSfx)
            sfxSource.PlayOneShot(clip);
    }

    public void MuteSfx(bool value)
    {
        mutedSfx = value;
        ApplyMuteStates();
        SaveMuteStates();
    }

    public bool IsSfxMuted() => mutedSfx;

    // --------- Conveniências por cena ---------

    public void OnEnterMainMenu()
    {
        Debug.Log("OnEnterMainMenu chamado");
        PlayMusic(mainMenuMusic, true);
    }

    public void OnEnterSelectPlayer()
    {
        PlayMusic(selectPlayerMusic, true);
    }

    public void OnEnterGameplay()
    {
        PlayMusic(gameplayRadioDefault, true);
    }

    // ---- Métodos para botão de UI ----
    public void ToggleMusicMute()
    {
        MuteMusic(!IsMusicMuted());
    }

    public void ToggleSfxMute()
    {
        MuteSfx(!IsSfxMuted());
    }
}
