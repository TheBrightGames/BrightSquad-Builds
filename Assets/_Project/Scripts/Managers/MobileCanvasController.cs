using UnityEngine;

public class MobileCanvasController : MonoBehaviour
{
    [Header("Configurações de Teste")]
    [Tooltip("Marque para ver os botões enquanto testa no Unity Editor.")]
    public bool mostrarNoEditor = true;

    void Awake()
    {
        bool isMobile = false;

        // 1. Verifica se é uma build para Android ou iOS
#if UNITY_ANDROID || UNITY_IOS
        isMobile = true;
#endif

        // 2. Se estiver rodando no Editor do Unity, respeita a checkbox
#if UNITY_EDITOR
        if (mostrarNoEditor)
        {
            isMobile = true;
        }
#else
        // 3. Se for uma build de PC (Windows/Mac/Linux) final, força esconder
        if (Application.platform == RuntimePlatform.WindowsPlayer || 
            Application.platform == RuntimePlatform.OSXPlayer ||
            Application.platform == RuntimePlatform.LinuxPlayer)
        {
            isMobile = false;
        }
#endif

        // Aplica a visibilidade
        gameObject.SetActive(isMobile);
    }
}