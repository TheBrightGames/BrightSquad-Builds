using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ResetUIOnResult : MonoBehaviour
{
    void Awake()
    {
        // garante timeScale normal
        Time.timeScale = 1f;

        // destrói qualquer EventSystem antigo que veio com DontDestroyOnLoad
        var eventos = FindObjectsByType<EventSystem>(FindObjectsSortMode.None);
        if (eventos.Length > 1)
        {
            for (int i = 0; i < eventos.Length - 1; i++)
                Destroy(eventos[i].gameObject);
        }

        // opcional: desbloqueia cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
