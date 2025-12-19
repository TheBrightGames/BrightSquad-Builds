using UnityEngine;
using UnityEngine.InputSystem;

public class RadioController : MonoBehaviour
{
    void Update()
    {
        // O 'R' continua funcionando aqui
        if (Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
        {
            ChamarProximaFaixa();
        }
    }

    // MUDANÇA AQUI: Troque 'private' por 'public'
    // Se for private, o botão do Unity não encontra a função na lista.
    public void ChamarProximaFaixa()
    {
        if (SoundController.Instance != null)
        {
            // Ajustei o log, pois agora pode vir do R ou do Botão
            Debug.Log("Comando de próxima faixa detectado.");

            SoundController.Instance.ProximaFaixaRadio();
        }
    }
}