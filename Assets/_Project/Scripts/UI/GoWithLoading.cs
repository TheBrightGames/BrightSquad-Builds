using UnityEngine;
using UnityEngine.SceneManagement;

public class GoWithLoading : MonoBehaviour
{
    public string targetSceneName;   // cena que você quer abrir depois do loading

    public void OnClickGo()
    {
        PlayerPrefs.SetString("NextScene", targetSceneName);
        PlayerPrefs.Save();

        SceneManager.LoadScene("LoadingScene");
    }
}
