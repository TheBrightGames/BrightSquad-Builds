using UnityEngine;
using UnityEngine.InputSystem; // Usando o sistema novo que você já tem

public class CursorManager : MonoBehaviour
{
    void Start()
    {
        // Trava o mouse no centro e deixa invisível assim que começa
        TravarMouse();
    }

    void Update()
    {
        // Exemplo: Se apertar ESC, destrava para você poder clicar nos botões da UI
        // (Ou se você tiver uma variável de "IsPaused", use ela aqui)
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            DestravarMouse();
        }

        // Exemplo: Se clicar na tela enquanto estiver destravado, trava de novo (para voltar ao jogo)
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            // Só trava se não estiver clicando em um botão de UI (opcional, mas recomendado)
            // Mas para simplificar agora:
            // TravarMouse(); 
        }
    }

    public void TravarMouse()
    {
        // Locked: O cursor fica preso no centro e invisível
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void DestravarMouse()
    {
        // None: O cursor fica livre e visível
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}