using UnityEngine;

public class MainMenuMusic : MonoBehaviour
{
    void Start()
    {
        Debug.Log("MainMenuMusic Start");

        if (SoundManager.Instance != null)
            SoundManager.Instance.OnEnterMainMenu();
        else
            Debug.LogWarning("SoundManager.Instance é nulo na MainMenu");
    }
}
