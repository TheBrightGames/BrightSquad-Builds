using UnityEngine;

// Coloque este script em um GameObject vazio na sua cena (ex: um objeto chamado "SceneAudioManager")
public class SceneAudioTrigger : MonoBehaviour
{
    [Header("Música desta Cena")]
    [Tooltip("Qual música deve começar a tocar quando esta cena abrir?")]
    [SerializeField] private AudioClip musicaDaCena;

    [Header("Configurações")]
    [Tooltip("Se marcado, a música da rádio será interrompida para tocar a música da cena.")]
    [SerializeField] private bool forcarTroca = true;

    void Start()
    {
        // Quando a cena inicia, pedimos ao SoundController para tocar nossa música
        if (musicaDaCena != null && SoundController.Instance != null)
        {
            if (forcarTroca)
            {
                SoundController.Instance.TrocarMusica(musicaDaCena);
            }
            // Se você quisesse uma lógica de "Só toca se não tiver nada tocando da rádio",
            // você precisaria de mais verificações no SoundController, mas o padrão é a cena impor sua música.
        }
    }
}