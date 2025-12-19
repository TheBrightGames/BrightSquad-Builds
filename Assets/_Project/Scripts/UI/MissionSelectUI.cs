using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MissionSelectUI : MonoBehaviour
{
    [Header("UI")]
    public Button[] botoesMissoes;
    public GameObject painelMensagem;
    public Text txtMensagem;

    [System.Serializable]
    public class VisualMissao
    {
        public Sprite spriteDesbloqueada;
        public Sprite spriteBloqueada;
        public Image iconeCadeado;
    }

    [Header("Visuais")]
    public VisualMissao[] visuais;

    [Header("Dono de cada missão (por índice)")]
    // 0=Naldinho, 1=Leo, 2=Silvana
    public int[] donoPorMissao;

    void Start()
    {
        AtualizarEstadoBotoes();
    }

    void AtualizarEstadoBotoes()
    {
        int personagemAtual = PlayerPrefs.GetInt("PersonagemSelecionado", 0);
        int ultimaConcluida = PlayerPrefs.GetInt("UltimaMissaoConcluida", -1);

        for (int i = 0; i < botoesMissoes.Length; i++)
        {
            var btn = botoesMissoes[i];
            if (btn == null) continue;

            bool podeJogar = PodeJogar(i, personagemAtual, ultimaConcluida);

            btn.interactable = true;

            var img = btn.GetComponent<Image>();
            if (img != null && visuais != null && i < visuais.Length && visuais[i] != null)
            {
                img.sprite = podeJogar
                    ? visuais[i].spriteDesbloqueada
                    : visuais[i].spriteBloqueada;
            }

            if (visuais != null && i < visuais.Length && visuais[i] != null && visuais[i].iconeCadeado != null)
            {
                visuais[i].iconeCadeado.gameObject.SetActive(!podeJogar);
            }

            var txt = btn.GetComponentInChildren<Text>();
            if (txt != null)
                txt.text = $"Missão {i + 1}";
        }
    }

    public void TentarAbrirMissao(int missionId)
    {
        int personagemAtual = PlayerPrefs.GetInt("PersonagemSelecionado", 0);
        int ultimaConcluida = PlayerPrefs.GetInt("UltimaMissaoConcluida", -1);

        int dono = 0;
        if (donoPorMissao != null && missionId < donoPorMissao.Length)
            dono = donoPorMissao[missionId];

        bool missaoJaConcluida = PlayerPrefs.GetInt("Missao_" + missionId + "_Concluida", 0) == 1;

        if (!PodeJogar(missionId, personagemAtual, ultimaConcluida))
        {
            PlayerPrefs.SetInt("MissionIdTentada", missionId);
            PlayerPrefs.SetInt("PersonagemDaMissaoTentada", dono);
            PlayerPrefs.Save();

            if (painelMensagem != null && txtMensagem != null)
            {
                txtMensagem.text = "Conclua as missões deste agente para liberar.";
                painelMensagem.SetActive(true);
            }
            return;
        }

        PlayerPrefs.SetInt("MissionIdAtual", missionId);
        PlayerPrefs.SetInt("PersonagemSelecionado", dono);
        PlayerPrefs.Save();

        SaveUtils.LimparQuickSave();

        if (!missaoJaConcluida)
        {
            SceneManager.LoadScene("BSGame");
        }
        else
        {
            SceneManager.LoadScene("SelectPlayer");
        }
    }

    public void FecharMensagem()
    {
        if (painelMensagem != null)
            painelMensagem.SetActive(false);
    }

    bool PodeJogar(int missionId, int personagemAtual, int ultimaConcluida)
    {
        bool missaoJaConcluida = PlayerPrefs.GetInt("Missao_" + missionId + "_Concluida", 0) == 1;

        int dono = 0;
        if (donoPorMissao != null && missionId < donoPorMissao.Length)
            dono = donoPorMissao[missionId];

        bool ehDoPersonagem = (dono == personagemAtual);
        bool ehProximaDoPersonagem = ehDoPersonagem && (missionId == ultimaConcluida + 1);

        return missaoJaConcluida || ehProximaDoPersonagem;
    }

    public void MenuPrincipal()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void BotaoEscolherPersonagem()
    {
        int dono = PlayerPrefs.GetInt("PersonagemDaMissaoTentada", 0);
        int missionId = PlayerPrefs.GetInt("MissionIdTentada", 0);

        PlayerPrefs.SetInt("PersonagemSelecionado", dono);
        PlayerPrefs.SetInt("MissionIdAtual", missionId);

        SaveUtils.LimparQuickSave();
        PlayerPrefs.Save();

        SceneManager.LoadScene("BSGame");
    }
}
