using UnityEngine;
using UnityEngine.InputSystem;

public class GameAudioSettings : MonoBehaviour
{
    [Range(0f, 1f)]
    public float volumeJogo = 0.20f;

    void Start()
    {
        // Se aparecer essa mensagem, o script está na cena e funcionando
        Debug.Log("GameAudioSettings: Script iniciado com sucesso.");

        if (SoundController.Instance != null)
        {
            SoundController.Instance.DefinirVolume(volumeJogo);
        }
        else
        {
            Debug.LogError("GameAudioSettings: SoundController NÃO ENCONTRADO! Verifique se ele está na cena.");
        }
    }

    void Update()
    {
        // Verifica se o teclado existe
        if (Keyboard.current == null) return;

        // Se apertar X...
        if (Keyboard.current.xKey.wasPressedThisFrame)
        {
            Debug.Log("GameAudioSettings: Tecla X detectada! Tentando mudar SFX...");
            MudarEstadoSFX();
        }

        // Se apertar M...
        if (Keyboard.current.mKey.wasPressedThisFrame)
        {

            MudarEstadoMusica();
            Debug.Log("GameAudioSettings: Tecla M detectada! Mutando música...");
            
        }


    }

    public void MudarEstadoSFX()
    {
        if (SoundController.Instance != null)
        {
            SoundController.Instance.AlternarSFX();
            Debug.Log("GameAudioSettings: Comando enviado para SoundController.");
        }
        else
        {
            Debug.LogError("GameAudioSettings: SoundController sumiu! Impossível mudar SFX.");
        }
    }


    public void MudarEstadoMusica()
    {
        if (SoundController.Instance != null)
        {
            SoundController.Instance.LigarDesligarMusica();
            Debug.Log("GameAudioSettings: Comando enviado para SoundController.");
        }
        else
        {
            Debug.LogError("GameAudioSettings: SoundController sumiu! Impossível mudar Música.");
        }
    }




}