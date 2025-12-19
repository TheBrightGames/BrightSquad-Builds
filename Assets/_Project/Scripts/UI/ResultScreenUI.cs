using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultScreenUI : MonoBehaviour
{
    [Header("Textos principais")]
    public Text txtTitulo;
    public Text txtSubtitulo;
    public Text txtMensagem;

    [Header("Resultados")]
    public Text txtTempoValor;
    public Text txtLixoValor;
    public Text txtScoreValor;

    [Header("Medalha")]
    public Image imgMedalha;
    public Sprite spriteSemMedalha;
    public Sprite spriteBronze;
    public Sprite spritePrata;
    public Sprite spriteOuro;
    public Text txtDescricaoMedalha;

    [Header("Config medalhas (score)")]
    public int scoreBronze = 1000;
    public int scorePrata = 2000;
    public int scoreOuro = 3000;

    [Header("Cenas")]
    public string cenaGameplay = "BSGame";
    public string cenaMissoes = "MissionSelect";
    public string cenaSelectAgent = "SelectPlayer";


    [Header("Prêmio da Comunidade")]
    public CommunityRewardDB rewardDB;
    public Image imgPremio;
    public Text txtTituloPremioValor;
    public Text txtDescricaoPremio;
    public Text txtTituloPremioSemMedalha;
    public Sprite spritePremioPadrao;   // arraste aqui no Inspector



    void Start()
    {
        CarregarDados();
        CarregarPremioComunidade();
        // sempre que a tela aparecer, libera o cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void CarregarDados()
    {
        float tempo = PlayerPrefs.GetFloat("LastResult_Tempo", 0f);
        int score = PlayerPrefs.GetInt("LastResult_Score", 0);
        int venceu = PlayerPrefs.GetInt("LastResult_Venceu", 1);
        int lixoTotalGlobal = PlayerPrefs.GetInt("Lixo_Total_Global", 0);

        if (txtTempoValor != null)
            txtTempoValor.text = FormatTime(tempo);

        if (txtScoreValor != null)
            txtScoreValor.text = score.ToString();

        if (txtLixoValor != null)
            txtLixoValor.text = lixoTotalGlobal.ToString();

        AtualizarMedalha(score);

        bool foiVitoria = (venceu == 1);
        AtualizarTextosPorTipo(foiVitoria);
    }

    void CarregarPremioComunidade()
    {
        if (rewardDB == null || imgPremio == null ||
            txtTituloPremioValor == null || txtDescricaoPremio == null)
            return;

        string unlockId = PlayerPrefs.GetString("LastResult_UnlockId", "");

        if (string.IsNullOrEmpty(unlockId))
        {
            // Usa o prêmio padrão (sem medalha / sem melhoria)
            imgPremio.gameObject.SetActive(true);
            imgPremio.enabled = true;
            imgPremio.sprite = spritePremioPadrao;

            if (txtTituloPremioSemMedalha != null)
                txtTituloPremioSemMedalha.text = "Nenhuma melhoria conquistada";
            txtTituloPremioValor.text = "";
            txtDescricaoPremio.text = "Ganhe uma medalha para desbloquear melhorias para o bairro!";
            return;
        }

        var entry = rewardDB.GetById(unlockId);
        if (entry == null)
        {
            imgPremio.gameObject.SetActive(true);
            imgPremio.enabled = true;
            imgPremio.sprite = spritePremioPadrao;

            if (txtTituloPremioSemMedalha != null)
                txtTituloPremioSemMedalha.text = "Nenhuma melhoria conquistada";
            txtTituloPremioValor.text = "";
            txtDescricaoPremio.text = "Ganhe uma medalha para desbloquear melhorias para o bairro!";
            return;
        }

        // Caso com melhoria desbloqueada
        imgPremio.gameObject.SetActive(true);
        imgPremio.enabled = true;
        imgPremio.sprite = entry.icon;
        txtTituloPremioValor.text = entry.titulo;
        txtDescricaoPremio.text = entry.descricao;
    }




    void AtualizarTextosPorTipo(bool vitoria)
    {
        if (vitoria)
        {
            if (txtTitulo) txtTitulo.text = "MISSÃO CUMPRIDA!";
            if (txtSubtitulo) txtSubtitulo.text = "Parabéns, agente!";
            if (txtMensagem) txtMensagem.text = "Você ajudou a deixar o bairro muito mais limpo.";
        }
        else
        {
            if (txtTitulo) txtTitulo.text = "QUASE LÁ! BOA TENTATIVA!";
            if (txtSubtitulo) txtSubtitulo.text = "Vamos tentar de novo?";
            if (txtMensagem) txtMensagem.text = "O bairro ainda precisa da sua ajuda. Volte e tente novamente!";
        }
    }

    void AtualizarMedalha(int score)
    {
        if (imgMedalha == null || txtDescricaoMedalha == null)
            return;

        if (score >= scoreOuro)
        {
            imgMedalha.sprite = spriteOuro;
            txtDescricaoMedalha.text = "OURO - Incrível! Você limpou quase tudo!";
        }
        else if (score >= scorePrata)
        {
            imgMedalha.sprite = spritePrata;
            txtDescricaoMedalha.text = "PRATA - Falta pouco para a medalha de Ouro!";
        }
        else if (score >= scoreBronze)
        {
            imgMedalha.sprite = spriteBronze;
            txtDescricaoMedalha.text = "BRONZE - Você está quase pegando a medalha de Prata!";
        }
        else
        {
            imgMedalha.sprite = spriteSemMedalha;
            txtDescricaoMedalha.text = "SEM MEDALHA - Tente de novo, o bairro precisa de você!";
        }

    }

    string FormatTime(float totalSeconds)
    {
        int minutos = Mathf.FloorToInt(totalSeconds / 60f);
        int segundos = Mathf.FloorToInt(totalSeconds % 60f);
        return string.Format("{0:00}:{1:00}", minutos, segundos);
    }

    // Jogar novamente (mesma missão, mesmo agente, sem quick save)
    public void BotaoJogarNovamente()
    {
        int missionId = PlayerPrefs.GetInt("LastResult_MissionId", 0);
        int playerId = PlayerPrefs.GetInt("LastResult_PlayerId", 0);

        SaveUtils.LimparQuickSave();

        // zera lixo para próxima tentativa
        PlayerPrefs.SetInt("Lixo_Total_Global", 0);            // se quiser resetar global
        PlayerPrefs.SetInt("LastResult_LixoColetado", 0);      // se usar por run
        PlayerPrefs.Save();

        PlayerPrefs.SetInt("PersonagemSelecionado", playerId);
        PlayerPrefs.SetInt("MissionIdAtual", missionId);
        PlayerPrefs.Save();

        Time.timeScale = 1f;
        SceneManager.LoadScene(cenaGameplay);
    }



    public void BotaoProximaMissao()
    {
        Time.timeScale = 1f;

        int missionId = PlayerPrefs.GetInt("LastResult_MissionId", 0);
        int playerId = PlayerPrefs.GetInt("LastResult_PlayerId", 0);

        int proxima = missionId + 1;
        SaveUtils.LimparQuickSave();

        PlayerPrefs.SetInt("PersonagemSelecionado", playerId);
        PlayerPrefs.SetInt("MissionIdAtual", proxima);
        PlayerPrefs.Save();

        SceneManager.LoadScene(cenaGameplay);
    }

    public void BotaoVoltarMissoes()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(cenaMissoes);
    }

    public void BotaoTrocarAgente()
    {
        SaveUtils.LimparQuickSave();

        Time.timeScale = 1f;
        SceneManager.LoadScene(cenaSelectAgent);
    }
}
