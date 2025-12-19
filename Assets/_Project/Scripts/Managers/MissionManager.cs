using UnityEngine;
using UnityEngine.UI;

public class MissionManager : MonoBehaviour
{
    // Singleton para outros scripts acessarem esta missão
    public static MissionManager instance;

    [System.Serializable]
    public class Desbloqueio
    {
        public string id;
        public GameObject objeto;
    }

    public enum DonoMissao
    {
        Naldinho = 0,
        Leo = 1,
        Silvana = 2
    }

    [System.Serializable]
    public class MissaoConfig
    {
        public string nome;
        public DonoMissao dono;

        int DonoToInt(DonoMissao dono)
        {
            return (int)dono; // Naldinho=0, Leo=1, Silvana=2
        }


        public Transform spawnPoint;
        public Desbloqueio[] desbloqueiosDaMissao;

        [Header("Objetivos / Lixo desta missão")]
        public GameObject[] gruposDeObjetivos;
    }

    [Header("Config de Missões")]
    public MissaoConfig[] missoes;

    [Header("Score")]
    public float tempoLimite = 180f;
    public int pontosMaximos = 1000;
    public int pontosMinimos = 200;

    [Header("HUD")]
    public Text txtTempoHUD;

    int missionId;
    float tempoDecorrido;
    bool missaoConcluida;
    int playerId;

    void Awake()
    {
        // registra singleton
        instance = this;
    }

    void Start()
    {
        playerId = PlayerPrefs.GetInt("PersonagemSelecionado", 0);
        missionId = PlayerPrefs.GetInt("MissionIdAtual", 0);

        AplicarDesbloqueiosGlobais();
        AtivarObjetivosDaMissao();

        // Checa a chave por player, igual ao GameManager
        string hasQSKey = $"P{playerId}_HasQuickSave";
        if (PlayerPrefs.GetInt(hasQSKey, 0) == 0)
        {
            PosicionarPlayerNoSpawn();
            Debug.Log("[MISSION] Posicionando no spawn (sem quicksave).");
        }
        else
        {
            Debug.Log("[MISSION] Não reposiciona, quicksave ativo.");
        }
    }




    void Update()
    {
        if (missaoConcluida) return;

        tempoDecorrido += Time.deltaTime;
        AtualizarTempoHUD();

        if (tempoDecorrido >= tempoLimite && !ChegouESG100())
            FalhouMissao();

        if (ChegouESG100())
            ConcluirMissao();
    }

    void AtualizarTempoHUD()
    {
        if (txtTempoHUD == null) return;

        float tempoRestante = Mathf.Max(0f, tempoLimite - tempoDecorrido);
        int minutos = Mathf.FloorToInt(tempoRestante / 60f);
        int segundos = Mathf.FloorToInt(tempoRestante % 60f);

        txtTempoHUD.text = string.Format("{0:00}:{1:00}", minutos, segundos);
    }

    bool ChegouESG100()
    {
        if (GameManager.instance == null) return false;

        int total = GameManager.instance.TotalLixoColetado;
        float porcentagem = Mathf.Clamp01(
            (float)total / GameManager.instance.alvoTotalLixo
        );

        return porcentagem >= 0.99f;
    }

    void AtivarObjetivosDaMissao()
    {
        if (missoes == null) return;

        foreach (var m in missoes)
        {
            if (m == null || m.gruposDeObjetivos == null) continue;
            foreach (var g in m.gruposDeObjetivos)
                if (g != null) g.SetActive(false);
        }

        if (missionId < 0 || missionId >= missoes.Length) return;
        var cfg = missoes[missionId];
        if (cfg.gruposDeObjetivos == null) return;

        foreach (var g in cfg.gruposDeObjetivos)
            if (g != null) g.SetActive(true);
    }

    void PosicionarPlayerNoSpawn()
    {
        if (missoes == null || missionId >= missoes.Length) return;

        var cfg = missoes[missionId];
        if (cfg.spawnPoint == null) return;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = cfg.spawnPoint.position;
            player.transform.rotation = cfg.spawnPoint.rotation;
        }
    }

    void ConcluirMissao()
    {
        missaoConcluida = true;

        float t = Mathf.Clamp01(tempoDecorrido / tempoLimite);
        int score = Mathf.RoundToInt(
            Mathf.Lerp(pontosMaximos, pontosMinimos, t)
        );

        int lixoDaRun = GameManager.instance != null
            ? GameManager.instance.TotalLixoColetado
            : 0;

        int lixoTotalGlobal = PlayerPrefs.GetInt("Lixo_Total_Global", 0);
        lixoTotalGlobal += lixoDaRun;
        PlayerPrefs.SetInt("Lixo_Total_Global", lixoTotalGlobal);

        SalvarScore(score, tempoDecorrido);
        SalvarDesbloqueiosDaMissao();
        PlayerPrefs.SetInt("UltimaMissaoConcluida", missionId);
        PlayerPrefs.SetInt("Missao_" + missionId + "_Concluida", 1);

        // dados para ResultScreen
        PlayerPrefs.SetFloat("LastResult_Tempo", tempoDecorrido);
        PlayerPrefs.SetInt("LastResult_Score", score);
        PlayerPrefs.SetInt("LastResult_MissionId", missionId);
        PlayerPrefs.SetInt("LastResult_PlayerId", playerId);
        PlayerPrefs.SetInt("LastResult_Venceu", 1); // 1 = vitória

        PlayerPrefs.Save();

        UnityEngine.SceneManagement.SceneManager.LoadScene("ResultScreen");
    }

    void FalhouMissao()
    {
        missaoConcluida = true;

        int score = pontosMinimos / 2;
        SalvarScore(score, tempoDecorrido);

        PlayerPrefs.SetFloat("LastResult_Tempo", tempoDecorrido);
        PlayerPrefs.SetInt("LastResult_Score", score);
        PlayerPrefs.SetInt("LastResult_MissionId", missionId);
        PlayerPrefs.SetInt("LastResult_PlayerId", playerId);
        PlayerPrefs.SetInt("LastResult_Venceu", 0); // 0 = quase lá / game over

        PlayerPrefs.Save();

        UnityEngine.SceneManagement.SceneManager.LoadScene("ResultScreen");
    }

    void SalvarScore(int score, float tempo)
    {
        string timeKey = SaveKeys.TimeKey(playerId, missionId);
        string scoreKey = SaveKeys.ScoreKey(playerId, missionId);

        float bestTime = PlayerPrefs.GetFloat(timeKey, float.MaxValue);
        int bestScore = PlayerPrefs.GetInt(scoreKey, 0);

        if (tempo < bestTime) PlayerPrefs.SetFloat(timeKey, tempo);
        if (score > bestScore) PlayerPrefs.SetInt(scoreKey, score);
    }

    void SalvarDesbloqueiosDaMissao()
    {
        if (missoes == null || missionId >= missoes.Length) return;

        var cfg = missoes[missionId];
        if (cfg.desbloqueiosDaMissao == null) return;

        // só desbloqueia se o personagem certo concluiu
        int donoDaMissao = (int)cfg.dono;
        if (playerId != donoDaMissao)
        {
            // personagem errado -> não libera nada
            return;
        }

        string ultimoUnlockId = "";

        foreach (var d in cfg.desbloqueiosDaMissao)
        {
            if (d == null || string.IsNullOrEmpty(d.id)) continue;

            PlayerPrefs.SetInt(SaveKeys.UnlockKey(d.id), 1);
            ultimoUnlockId = d.id;
        }

        if (!string.IsNullOrEmpty(ultimoUnlockId))
            PlayerPrefs.SetString("LastResult_UnlockId", ultimoUnlockId);
    }


    void AplicarDesbloqueiosGlobais()
    {
        if (missoes == null) return;

        foreach (var m in missoes)
        {
            if (m == null || m.desbloqueiosDaMissao == null) continue;

            foreach (var d in m.desbloqueiosDaMissao)
            {
                if (d == null || d.objeto == null || string.IsNullOrEmpty(d.id))
                    continue;

                bool ativo = PlayerPrefs.GetInt(SaveKeys.UnlockKey(d.id), 0) == 1;
                d.objeto.SetActive(ativo);
            }
        }
    }

    // Getter para o GameManager conseguir ler o tempo
    public float GetTempoDecorrido()
    {
        return tempoDecorrido;
    }

    // Setter já existia, usado para restaurar o tempo do save rápido
    public void SetTempoDecorrido(float valor)
    {
        tempoDecorrido = valor;
    }
}
