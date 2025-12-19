using UnityEngine;
using UnityEngine.Audio;

public class SoundController : MonoBehaviour
{
    private static SoundController instance;
    public static SoundController Instance => instance;

    // --- EVENTOS ---
    public event System.Action<bool> OnEstadoMusicaChanged; // Renomeei para ficar claro
    public event System.Action<bool> OnEstadoSFXChanged;    // --- NOVO: Evento só pro SFX

    [Header("Configurações Iniciais")]
    public bool tocarAoIniciar = true;

    [Header("Configurações do Mixer (SFX)")]
    public AudioMixer mainMixer;
    private bool sfxMutado = false;
    // Propriedade pública para saber se o SFX está ligado (para a UI usar)
    public bool SFXEstaLigado => !sfxMutado;

    [Header("Playlist da Rádio")]
    [SerializeField] private AudioClip[] trilhasRadio;
    private int indiceAtual = -1;

    // Controle da Música (Rádio)
    private bool estadoMusica = true;
    public bool MusicaEstaLigada => estadoMusica;

    [Header("Áudio Source Principal")]
    [SerializeField] private AudioSource fundoMusical;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        InicializarAudio();
    }

    private void InicializarAudio()
    {
        if (fundoMusical == null)
        {
            fundoMusical = GetComponent<AudioSource>();
            if (fundoMusical == null) Debug.LogError("SoundController: Falta AudioSource!");
            return;
        }

        fundoMusical.loop = true;
        AtualizarEstadoDoAudioSource();

        // Dispara os eventos iniciais para atualizar a UI
        OnEstadoMusicaChanged?.Invoke(estadoMusica);
        OnEstadoSFXChanged?.Invoke(!sfxMutado);

        if (tocarAoIniciar && estadoMusica && fundoMusical.clip != null && !fundoMusical.isPlaying)
        {
            fundoMusical.Play();
        }
    }

    public void ComecarMusica()
    {
        if (fundoMusical != null && !fundoMusical.isPlaying && estadoMusica)
        {
            fundoMusical.Play();
        }
    }

    // --- LÓGICA DA MÚSICA (RÁDIO) ---

    private void AtualizarEstadoDoAudioSource()
    {
        if (fundoMusical == null) return;
        fundoMusical.mute = !estadoMusica;
    }

    public void LigarDesligarMusica()
    {
        estadoMusica = !estadoMusica;
        AtualizarEstadoDoAudioSource();

        // Avisa a UI que a música mudou
        OnEstadoMusicaChanged?.Invoke(estadoMusica);

        if (estadoMusica && fundoMusical != null && !fundoMusical.isPlaying && fundoMusical.clip != null)
        {
            fundoMusical.Play();
        }
    }

    public void TrocarMusica(AudioClip novaMusica)
    {
        if (fundoMusical == null || novaMusica == null) return;
        if (fundoMusical.clip == novaMusica && fundoMusical.isPlaying) return;

        fundoMusical.clip = novaMusica;
        AtualizarEstadoDoAudioSource();

        if (estadoMusica) fundoMusical.Play();
    }

    public void DefinirVolume(float volume)
    {
        if (fundoMusical == null) return;
        fundoMusical.volume = Mathf.Clamp01(volume);
    }

    public void ProximaFaixaRadio()
    {
        if (trilhasRadio == null || trilhasRadio.Length == 0) return;
        indiceAtual = (indiceAtual + 1) % trilhasRadio.Length;
        TrocarMusica(trilhasRadio[indiceAtual]);
    }

    // --- LÓGICA DE SFX ---

    public void AlternarSFX()
    {
        if (mainMixer == null) return;

        sfxMutado = !sfxMutado;

        if (sfxMutado)
        {
            mainMixer.SetFloat("VolumeSFX", -80f);
            Debug.Log("SFX Mutado");
        }
        else
        {
            mainMixer.SetFloat("VolumeSFX", 0f);
            Debug.Log("SFX Ligado");
        }

        // --- NOVO: Avisa a UI que o SFX mudou (Envia true se estiver ligado)
        OnEstadoSFXChanged?.Invoke(!sfxMutado);
    }
}