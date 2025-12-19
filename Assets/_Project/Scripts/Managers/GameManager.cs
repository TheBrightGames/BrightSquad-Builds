using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Textos HUD Lixo")]
    public Text textoPapel;
    public Text textoVidro;
    public Text textoPlastico;
    public Text textoMetal;

    [Header("ESG / Limpeza")]
    public Image barraVerdeFill;
    public Text textoESG;
    public int alvoTotalLixo = 50;

    [Header("ESG / Cores")]
    public Color corBaixa = Color.red;
    public Color corMedia = Color.yellow;
    public Color corAlta = Color.green;


    [Header("Pontuação por tipo (pontos por unidade)")]
    public int pontosPlastico = 1;
    public int pontosMetal = 3;
    public int pontosPapel = 2;
    public int pontosVidro = 4;

    [Header("Missão por Pontos")]
    public int pontosTotaisMissao = 100; // alvo para 100%
    public int scoreAtual = 0;



    int qtdPlastico = 0;
    int qtdMetal = 0;
    int qtdPapel = 0;
    int qtdVidro = 0;

    public int TotalLixoColetado => qtdPlastico + qtdMetal + qtdPapel + qtdVidro;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        SaveRapido();
    }

    void Update()
    {
        //ReturnGame();

        if (Keyboard.current != null && Keyboard.current.bKey.wasPressedThisFrame)
        {
            Debug.Log("[INPUT] B pressionado: chamando SalvarProgressoRapido()");
            SalvarProgressoRapido();
        }
    }

    public void ReturnGame()
    {
        //if (Keyboard.current != null && Keyboard.current.mKey.wasPressedThisFrame)
            SceneManager.LoadScene("MainMenu");
    }

    public void ColetarItem(TipoDeLixo tipo, int quantidadeRecebida)
    {
        switch (tipo)
        {
            case TipoDeLixo.Plastico:
                qtdPlastico += quantidadeRecebida;
                AtualizarTexto(textoPlastico, qtdPlastico);
                break;
            case TipoDeLixo.Metal:
                qtdMetal += quantidadeRecebida;
                AtualizarTexto(textoMetal, qtdMetal);
                break;
            case TipoDeLixo.Papel:
                qtdPapel += quantidadeRecebida;
                AtualizarTexto(textoPapel, qtdPapel);
                break;
            case TipoDeLixo.Vidro:
                qtdVidro += quantidadeRecebida;
                AtualizarTexto(textoVidro, qtdVidro);
                break;
        }



        // ---- NOVO: somar pontos ----
        int pontos = 0;
        switch (tipo)
        {
            case TipoDeLixo.Plastico:
                pontos = pontosPlastico;
                break;
            case TipoDeLixo.Metal:
                pontos = pontosMetal;
                break;
            case TipoDeLixo.Papel:
                pontos = pontosPapel;
                break;
            case TipoDeLixo.Vidro:
                pontos = pontosVidro;
                break;
        }

        scoreAtual += pontos * quantidadeRecebida;

        // ESG continua usando quantidade de lixo (como já fazia)
        AtualizarESG();

        // Se quiser, pode ter uma barra extra baseada em pontos:
        float porcentPontos = Mathf.Clamp01((float)scoreAtual / pontosTotaisMissao);
        // usar porcentPontos em outra barra, ou só para checar vitória
        if (scoreAtual >= pontosTotaisMissao)
        {
            // chegou a 100% dos pontos da missão
            // aqui você chama MissionManager.instance.FimDeMissaoSucesso() ou abre tela de vitória
        }





        AtualizarESG();
    }

    void AtualizarESG()
    {
        int total = TotalLixoColetado;
        float porcentagem = Mathf.Clamp01((float)total / alvoTotalLixo);

        if (barraVerdeFill != null)
        {
            barraVerdeFill.fillAmount = porcentagem;

            if (porcentagem < 0.5f)
                barraVerdeFill.color = Color.Lerp(corBaixa, corMedia, porcentagem / 0.5f);
            else
                barraVerdeFill.color = Color.Lerp(corMedia, corAlta, (porcentagem - 0.5f) / 0.5f);
        }

        if (textoESG != null)
            textoESG.text = Mathf.RoundToInt(porcentagem * 100f) + "%";
    }

    void AtualizarTexto(Text textoUI, int valor)
    {
        if (textoUI != null)
            textoUI.text = valor.ToString();
    }

    // ---------- QUICK SAVE POR PERSONAGEM ----------

    string P(int playerId, string key)
    {
        return $"P{playerId}_{key}";
    }

    public void SalvarProgressoRapido()
    {
        int playerId = PlayerPrefs.GetInt("PersonagemSelecionado", 0);
        int missionId = PlayerPrefs.GetInt("MissionIdAtual", 0);

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Vector3 pos = player.transform.position;
            Vector3 rot = player.transform.eulerAngles;

            PlayerPrefs.SetFloat(P(playerId, "Last_Player_PosX"), pos.x);
            PlayerPrefs.SetFloat(P(playerId, "Last_Player_PosY"), pos.y);
            PlayerPrefs.SetFloat(P(playerId, "Last_Player_PosZ"), pos.z);
            PlayerPrefs.SetFloat(P(playerId, "Last_Player_RotY"), rot.y);
        }

        float tempo = 0f;
        if (MissionManager.instance != null)
        {
            tempo = MissionManager.instance.GetTempoDecorrido();
            PlayerPrefs.SetFloat(P(playerId, "Last_TempoDecorrido"), tempo);
        }

        PlayerPrefs.SetInt(P(playerId, "Last_LixoPlastico"), qtdPlastico);
        PlayerPrefs.SetInt(P(playerId, "Last_LixoMetal"), qtdMetal);
        PlayerPrefs.SetInt(P(playerId, "Last_LixoPapel"), qtdPapel);
        PlayerPrefs.SetInt(P(playerId, "Last_LixoVidro"), qtdVidro);

        PlayerPrefs.SetInt(P(playerId, "Last_MissionId"), missionId);
        PlayerPrefs.SetInt(P(playerId, "HasQuickSave"), 1);

        PlayerPrefs.SetInt("Last_QuickSave_PlayerId", playerId);

        PlayerPrefs.Save();

        Debug.Log("[SAVE] QuickSave feito. " +
                  $"Player={playerId}, Missao={missionId}, Tempo={tempo}, " +
                  $"Lixo(P,M,Pa,V)=({qtdPlastico},{qtdMetal},{qtdPapel},{qtdVidro})");

        if (player != null)
            Debug.Log($"[SAVE] Pos={player.transform.position}");
    }

    public void BotaoVoltarMenuPrincipal()
    {
        SalvarProgressoRapido();
        SceneManager.LoadScene("MainMenu");
    }

    void SaveRapido()
    {
        int playerId = PlayerPrefs.GetInt("PersonagemSelecionado", 0);

        if (PlayerPrefs.GetInt(P(playerId, "HasQuickSave"), 0) == 1)
        {
            qtdPlastico = PlayerPrefs.GetInt(P(playerId, "Last_LixoPlastico"), 0);
            qtdMetal = PlayerPrefs.GetInt(P(playerId, "Last_LixoMetal"), 0);
            qtdPapel = PlayerPrefs.GetInt(P(playerId, "Last_LixoPapel"), 0);
            qtdVidro = PlayerPrefs.GetInt(P(playerId, "Last_LixoVidro"), 0);

            AtualizarTexto(textoPlastico, qtdPlastico);
            AtualizarTexto(textoMetal, qtdMetal);
            AtualizarTexto(textoPapel, qtdPapel);
            AtualizarTexto(textoVidro, qtdVidro);
            AtualizarESG();

            float tempo = PlayerPrefs.GetFloat(P(playerId, "Last_TempoDecorrido"), 0f);
            if (MissionManager.instance != null)
            {
                MissionManager.instance.SetTempoDecorrido(tempo);
            }

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                Vector3 pos;
                pos.x = PlayerPrefs.GetFloat(P(playerId, "Last_Player_PosX"), player.transform.position.x);
                pos.y = PlayerPrefs.GetFloat(P(playerId, "Last_Player_PosY"), player.transform.position.y);
                pos.z = PlayerPrefs.GetFloat(P(playerId, "Last_Player_PosZ"), player.transform.position.z);

                float rotY = PlayerPrefs.GetFloat(P(playerId, "Last_Player_RotY"), player.transform.eulerAngles.y);

                player.transform.position = pos;
                player.transform.rotation = Quaternion.Euler(0f, rotY, 0f);

                Debug.Log($"[LOAD] (P{playerId}) PosRestaurada={player.transform.position}, RotY={rotY}");
            }
            else
            {
                Debug.Log("[LOAD] Player não encontrado para restaurar posição.");
            }

            Debug.Log($"[LOAD] (P{playerId}) QuickSave lido. Tempo={tempo}, Lixo(P,M,Pa,V)=({qtdPlastico},{qtdMetal},{qtdPapel},{qtdVidro})");
        }
        else
        {
            Debug.Log($"[LOAD] (P{playerId}) Nenhum QuickSave encontrado.");
        }
    }
}
