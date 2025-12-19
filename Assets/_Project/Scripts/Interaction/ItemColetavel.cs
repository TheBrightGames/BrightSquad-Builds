using UnityEngine;

// Cria o menu de tipos de lixo
public enum TipoDeLixo
{
    Plastico,
    Metal,
    Papel,
    Vidro
}

public class ItemColetavel : MonoBehaviour
{
    [Header("Configurações do Lixo")]
    public TipoDeLixo tipoDeLixo; // Escolha se é Plástico, Metal, etc.

    // ESTA É A PARTE NOVA: Define quanto vale esse lixo.
    // Deixe 1 para lixos normais e mude para 5 no Inspector para o lixo agrupado.
    public int quantidade = 1;

    [Header("Audio")]
    public AudioClip somDeColeta; // Som de "plim" ao pegar

    private void OnTriggerEnter(Collider other)
    {
        // Verifica se quem encostou foi o Player
        if (other.CompareTag("Player"))
        {
            // 1. Tenta encontrar o Gerente do Jogo
            if (GameManager.instance != null)
            {
                // MUDANÇA AQUI: Agora enviamos o TIPO e a QUANTIDADE
                GameManager.instance.ColetarItem(tipoDeLixo, quantidade);
            }
            else
            {
                Debug.LogError("ERRO: O GameManager não está na cena!");
            }

            // 2. Toca o som (se houver som configurado)
            if (somDeColeta != null)
            {
                // Cria um som temporário no local onde o lixo estava
                AudioSource.PlayClipAtPoint(somDeColeta, transform.position);
            }

            // 3. Destrói o lixo para ele sumir da cena
            Destroy(gameObject);
        }
    }
}