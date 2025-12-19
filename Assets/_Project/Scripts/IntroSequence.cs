using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class IntroSequence : MonoBehaviour
{
    [Header("Configurações Visuais")]
    public Image logoImage;
    public Sprite[] logos;
    public float logoDuration = 2.5f;
    public float fadeDuration = 0.8f;
    public string nextSceneName = "MainMenu";

    [Header("Configurações de Áudio")]
    public AudioClip introMusic;        // <--- NOVA: A música contínua
    public AudioClip[] logoSounds;      // Sons específicos (SFX) para o futuro

    private AudioSource audioSource;
    private bool skipping = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (logoImage == null)
        {
            Debug.LogError("IntroSequence: logoImage não definido.");
            return;
        }

        // --- Lógica da Música Contínua ---
        if (introMusic != null)
        {
            audioSource.clip = introMusic;
            audioSource.loop = false; // Geralmente intro não precisa loopar, mas pode por true se for curta
            audioSource.Play();
        }

        StartCoroutine(RunSequence());
    }

    void Update()
    {
        // Pular com ESC ou Clique
        if ((Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame) ||
            (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame))
        {
            SkipSequence();
        }
    }

    void SkipSequence()
    {
        if (!skipping)
        {
            skipping = true;
        }
    }

    IEnumerator RunSequence()
    {
        // Garante alpha 0 no começo
        Color c = logoImage.color;
        c.a = 0f;
        logoImage.color = c;

        for (int i = 0; i < logos.Length; i++)
        {
            if (logos[i] == null) continue;

            logoImage.sprite = logos[i];

            // --- Som do Logo (SFX) ---
            // Usa PlayOneShot para tocar "por cima" da música de fundo
            // Se o array estiver vazio ou null agora, ele simplesmente ignora.
            if (logoSounds != null && i < logoSounds.Length && logoSounds[i] != null)
            {
                audioSource.PlayOneShot(logoSounds[i]);
            }

            // Fade in Visual
            yield return StartCoroutine(FadeLogo(0f, 1f, fadeDuration));
            if (skipping) break;

            // Espera
            float t = 0f;
            while (t < logoDuration && !skipping)
            {
                t += Time.deltaTime;
                yield return null;
            }
            if (skipping) break;

            // Fade out Visual
            yield return StartCoroutine(FadeLogo(1f, 0f, fadeDuration));
            if (skipping) break;

            yield return new WaitForSeconds(0.5f);
        }

        // Antes de sair, vamos baixar o volume da música suavemente?
        yield return StartCoroutine(FadeOutAudio(1.0f));

        PlayerPrefs.SetString("NextScene", nextSceneName);
        SceneManager.LoadScene("LoadingScene");
    }

    IEnumerator FadeLogo(float from, float to, float duration)
    {
        float t = 0f;
        Color c = logoImage.color;

        while (t < duration)
        {
            t += Time.deltaTime;
            float lerp = Mathf.Clamp01(t / duration);
            c.a = Mathf.Lerp(from, to, lerp);
            logoImage.color = c;

            if (skipping) break;
            yield return null;
        }
        c.a = to;
        logoImage.color = c;
    }

    // Corrotina extra para suavizar o fim da música
    IEnumerator FadeOutAudio(float duration)
    {
        float startVolume = audioSource.volume;
        float t = 0;

        while (t < duration)
        {
            t += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0, t / duration);
            yield return null;
        }
        audioSource.volume = 0;
    }
}