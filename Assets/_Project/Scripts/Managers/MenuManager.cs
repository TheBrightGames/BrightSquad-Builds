using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Nomes das cenas")]
    public string mainMenuScene = "MainMenu";
    public string selectPlayerScene = "SelectPlayer";
    public string missionSelectScene = "MissionSelect";
    public string gameScene = "BSGame";
    public string creditsScene = "Credits";

    // MENU PRINCIPAL

    public void StartGame()
    {
        SceneManager.LoadScene(selectPlayerScene);
    }


 




    public void BotaoContinuar()
    {
        // lê qual foi o último personagem que teve quick save
        int lastPlayerId = PlayerPrefs.GetInt("Last_QuickSave_PlayerId", -1);
        if (lastPlayerId == -1)
        {
            Debug.Log("[CONTINUAR] Nenhum quick save encontrado, indo para SelectPlayer.");
            SceneManager.LoadScene(selectPlayerScene);
            return;
        }

        // verifica se esse personagem realmente tem quick save
        string hasKey = $"P{lastPlayerId}_HasQuickSave";
        if (PlayerPrefs.GetInt(hasKey, 0) == 0)
        {
            Debug.Log("[CONTINUAR] Flag global existe mas personagem não tem quick save, indo para SelectPlayer.");
            SceneManager.LoadScene(selectPlayerScene);
            return;
        }

        int missionId = PlayerPrefs.GetInt($"P{lastPlayerId}_Last_MissionId", 0);

        PlayerPrefs.SetInt("PersonagemSelecionado", lastPlayerId);
        PlayerPrefs.SetInt("MissionIdAtual", missionId);
        PlayerPrefs.Save();

        Debug.Log($"[CONTINUAR] Carregando {gameScene} com Player={lastPlayerId}, Missao={missionId}");
        SceneManager.LoadScene(gameScene);
    }


    public void ResetarProgresso()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        Debug.Log("Progresso resetado (New Game).");
        SceneManager.LoadScene(selectPlayerScene);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBGL
        Application.OpenURL("about:blank");
#else
        Application.Quit();
#endif
    }

    // SELEÇÃO DE PERSONAGEM

    public void ConfirmarPersonagem()
    {
        SceneManager.LoadScene(gameScene);
    }

    public void BotaoMudarPersonagem()
    {
        SaveUtils.LimparQuickSave();
        SceneManager.LoadScene(selectPlayerScene);
    }

    public void VoltarParaMenuPrincipal()
    {
        SceneManager.LoadScene(mainMenuScene);
    }

    // SELEÇÃO DE MISSÃO

    public void VoltarParaSelectPlayer()
    {
        SceneManager.LoadScene(selectPlayerScene);
    }

    public void IniciarMissao()
    {
        SceneManager.LoadScene(gameScene);
    }
    public void SelecionarMissao()
    {
        SceneManager.LoadScene(missionSelectScene);
    }

    public void PosCreditos()
    {
        SceneManager.LoadScene(creditsScene);
    }

}
