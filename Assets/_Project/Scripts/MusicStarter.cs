using UnityEngine;

public class MusicStarter : MonoBehaviour
{
    // Se você quiser forçar uma música específica pro menu, arraste ela aqui.
    // Se deixar vazio, ele toca a que já estiver configurada no SoundController.
    public AudioClip musicaDestaCena;

    void Start()
    {
        if (SoundController.Instance != null)
        {
            // Se tiver uma música específica configurada aqui, troca.
            if (musicaDestaCena != null)
            {
                SoundController.Instance.TrocarMusica(musicaDestaCena);
            }
            // Se não, apenas manda tocar o que já tem.
            else
            {
                SoundController.Instance.ComecarMusica();
            }
        }
    }
}