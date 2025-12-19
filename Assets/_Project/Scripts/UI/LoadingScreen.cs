using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingScreen : MonoBehaviour
{
    [Header("UI")]
    public Slider sliderLoading;
    public Text txtPercentBar;
    public Text txtLoadingLabel;

    [Header("Configuração")]
    public string defaultScene = "Splash";   // usada se não tiver nada em NextScene
    public float minTime = 2f;

    string sceneToLoad;

    void Start()
    {
        // tenta pegar a cena pedida pelo botão
        sceneToLoad = PlayerPrefs.GetString("NextScene", "");

        // se não tiver, usa defaultScene (primeira vez, por exemplo)
        if (string.IsNullOrEmpty(sceneToLoad))
            sceneToLoad = defaultScene;

        sliderLoading.minValue = 0;
        sliderLoading.maxValue = 100;
        sliderLoading.value = 0;

        if (txtPercentBar != null)
            txtPercentBar.text = "0%";

        if (txtLoadingLabel != null)
            txtLoadingLabel.text = "Loading...";

        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneToLoad);
        op.allowSceneActivation = false;

        float elapsed = 0f;

        while (!op.isDone)
        {
            elapsed += Time.deltaTime;

            float t = Mathf.Clamp01(elapsed / minTime);
            int percent = Mathf.RoundToInt(t * 100f);

            sliderLoading.value = percent;
            if (txtPercentBar != null)
                txtPercentBar.text = percent + "%";

            float progress = Mathf.Clamp01(op.progress / 0.9f);
            if (t >= 1f && progress >= 1f)
                op.allowSceneActivation = true;

            yield return null;
        }
    }
}
