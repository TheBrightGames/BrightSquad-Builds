using UnityEngine;
using UnityEngine.SceneManagement;

public class Boot : MonoBehaviour
{
    [Header("Teste no Editor")]
    public bool alwaysShowLoadingInEditor = true;

    void Start()
    {
        // 1) Força sempre LoadingScene no Editor, se o flag estiver ligado
        if (Application.isEditor && alwaysShowLoadingInEditor)
        {
            SceneManager.LoadScene("LoadingScene");
            return;
        }

        // 2) Fluxo normal de build
        bool alreadyOpened = PlayerPrefs.GetInt("AlreadyOpened", 0) == 1;

        if (!alreadyOpened)
        {
            PlayerPrefs.SetInt("AlreadyOpened", 1);
            PlayerPrefs.Save();
            SceneManager.LoadScene("LoadingScene");
        }
        else
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
