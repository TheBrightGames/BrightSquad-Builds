using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterSelector : MonoBehaviour
{
    [Header("Configuração")]
    public DadosPersonagem[] personagens;
    public Transform spawnPoint;
    public int[] missoesPorPersonagem;   // 0,1,2

    [Header("UI (Legacy Text)")]
    public Text txtTituloPainel;
    public Text txtNome;
    public Text txtTituloPersonagem;
    public Text txtHistoria;
    public Text txtMotivacao;
    public Text txtHabilidades;

    int indiceAtual = 0;
    GameObject personagemInstanciado;

    const string CHAVE_PLAYER = "PersonagemSelecionado";

    void Start()
    {
        if (personagens == null || personagens.Length == 0)
        {
            Debug.LogError("CharacterSelector: nenhum personagem configurado.");
            return;
        }

        indiceAtual = PlayerPrefs.GetInt(CHAVE_PLAYER, 0);
        indiceAtual = Mathf.Clamp(indiceAtual, 0, personagens.Length - 1);

        MostrarPersonagem(indiceAtual);
    }

    public void Proximo()
    {
        if (personagens.Length == 0) return;
        indiceAtual = (indiceAtual + 1) % personagens.Length;
        MostrarPersonagem(indiceAtual);
    }

    public void Anterior()
    {
        if (personagens.Length == 0) return;
        indiceAtual = (indiceAtual - 1 + personagens.Length) % personagens.Length;
        MostrarPersonagem(indiceAtual);
    }

    void MostrarPersonagem(int index)
    {
        if (personagemInstanciado != null)
            Destroy(personagemInstanciado);

        var dados = personagens[index];

        if (dados.prefab3D != null && spawnPoint != null)
        {
            personagemInstanciado = Instantiate(
                dados.prefab3D,
                spawnPoint.position,
                spawnPoint.rotation
            );
        }

        if (txtNome != null) txtNome.text = dados.nome;
        if (txtTituloPersonagem != null) txtTituloPersonagem.text = dados.titulo;
        if (txtHistoria != null) txtHistoria.text = dados.historia;
        if (txtMotivacao != null) txtMotivacao.text = dados.motivacao;
        if (txtHabilidades != null) txtHabilidades.text = dados.habilidades;
    }

    public string gameSceneName = "BSGame";

    public void Confirmar()
    {
        PlayerPrefs.SetInt(CHAVE_PLAYER, indiceAtual);

        int missionId = 0;
        if (missoesPorPersonagem != null &&
            indiceAtual < missoesPorPersonagem.Length)
        {
            missionId = missoesPorPersonagem[indiceAtual];
        }

        PlayerPrefs.SetInt("MissionIdAtual", missionId);
        PlayerPrefs.Save();

        SceneManager.LoadScene(gameSceneName);
    }
}
