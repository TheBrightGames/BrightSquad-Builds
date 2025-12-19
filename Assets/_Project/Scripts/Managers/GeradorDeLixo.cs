using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeradorDeLixo : MonoBehaviour
{
    [Header("Sons")]
    public AudioClip somDeSpawn;      // Arraste o arquivo de som aqui
    private AudioSource audioSource;  // A "caixa de som"

    [Header("Configurações da Área")]
    public Vector3 tamanhoDaArea = new Vector3(10, 1, 10); // Largura (X) e profundidade (Z)
    public float tempoEntreSpawns = 5f;    // Tempo em segundos entre cada lixo
    public float margem = 0.5f;            // Distância mínima das paredes/bordas

    [Header("Configuração dos Lixos")]
    public List<ConfiguracaoLixo> listaDeLixos; // Lista que aparece no Inspector

    private bool podeSpawnar = true;

    [System.Serializable]
    public class ConfiguracaoLixo
    {
        public string nome;                // Só para organização (ex: "Garrafa")
        public GameObject prefab;          // O objeto que vai nascer
        public int quantidadeMaxima = 25;  // Limite máximo
        [HideInInspector] public int quantidadeAtual = 0;
    }

    void Start()
    {
        // conecta com o AudioSource do mesmo GameObject
        audioSource = GetComponent<AudioSource>();

        // Começa a rotina de gerar lixo repetidamente
        StartCoroutine(RotinaDeSpawn());
    }

    IEnumerator RotinaDeSpawn()
    {
        while (podeSpawnar)
        {
            yield return new WaitForSeconds(tempoEntreSpawns);

            SpawnarLixoAleatorio();

            VerificarFimDoSpawn();
        }
    }

    void SpawnarLixoAleatorio()
    {
        // Filtra só os lixos que ainda não atingiram o limite
        List<ConfiguracaoLixo> lixosDisponiveis = new List<ConfiguracaoLixo>();

        foreach (var lixo in listaDeLixos)
        {
            if (lixo.quantidadeAtual < lixo.quantidadeMaxima)
                lixosDisponiveis.Add(lixo);
        }

        if (lixosDisponiveis.Count == 0) return;

        ConfiguracaoLixo lixoSorteado =
            lixosDisponiveis[Random.Range(0, lixosDisponiveis.Count)];

        // posição aleatória dentro da área, com margem
        float halfX = tamanhoDaArea.x / 2f;
        float halfZ = tamanhoDaArea.z / 2f;

        Vector3 offsetLocal = new Vector3(
    Random.Range(-halfX + margem, halfX - margem),
    0f,
    Random.Range(-halfZ + margem, halfZ - margem)
);

        // aplica a rotação do objeto na área
        Vector3 posicaoAleatoria = transform.position + transform.rotation * offsetLocal;


        Instantiate(lixoSorteado.prefab, posicaoAleatoria, Quaternion.identity);

        lixoSorteado.quantidadeAtual++;

        if (somDeSpawn != null && audioSource != null)
            audioSource.PlayOneShot(somDeSpawn);
    }

    void VerificarFimDoSpawn()
    {
        bool aindaTemOQueNascer = false;
        foreach (var lixo in listaDeLixos)
        {
            if (lixo.quantidadeAtual < lixo.quantidadeMaxima)
            {
                aindaTemOQueNascer = true;
                break;
            }
        }

        if (!aindaTemOQueNascer)
        {
            podeSpawnar = false;
            Debug.Log("Todos os lixos foram gerados!");
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.3f);

        // faz o Gizmo usar posição + rotação + escala do objeto
        Matrix4x4 matrizAntiga = Gizmos.matrix;
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);

        Gizmos.DrawCube(Vector3.zero, tamanhoDaArea);

        // volta a matriz padrão para não afetar outros gizmos
        Gizmos.matrix = matrizAntiga;
    }

}
