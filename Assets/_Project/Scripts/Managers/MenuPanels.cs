using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPanels : MonoBehaviour
{
    public GameObject panelMenu;
    public GameObject panelSelectPlayer;

    private void Start()
    {
        MostrarMenuPrincipal();
    }

    public void MostrarMenuPrincipal()
    {
        if (panelMenu != null) panelMenu.SetActive(true);
        if (panelSelectPlayer != null) panelSelectPlayer.SetActive(false);
    }

    public void MostrarSelecaoPersonagem()
    {
        if (panelMenu != null) panelMenu.SetActive(false);
        if (panelSelectPlayer != null) panelSelectPlayer.SetActive(true);
    }



    public void ResetarProgresso()
    {
        // Se quiser apagar TUDO que foi salvo com PlayerPrefs:
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        Debug.Log("Progresso resetado (New Game).");
        MostrarSelecaoPersonagem();
    }

    public void Continuar()
    {
        // se quiser, checa se já existe personagem salvo
        // se não houver personagem salvo ainda, manda escolher um
        if (!PlayerPrefs.HasKey("PersonagemSelecionado"))
        {
            SceneManager.LoadScene("SelectPlayer");
        }
        else
        {
            SceneManager.LoadScene("MissionSelect");
        }
    }


    public void BotaoMudarPersonagem()
    {
        // aqui a ideia é só trocar o agente, mantendo progresso e missões
        SceneManager.LoadScene("SelectPlayer");
    }


}
