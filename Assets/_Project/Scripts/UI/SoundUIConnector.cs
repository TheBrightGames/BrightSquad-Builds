using UnityEngine;
using UnityEngine.UI;

public class SoundUIConnector : MonoBehaviour
{
    // Escolha no Inspector se este botão controla a Música (Rádio) ou o SFX (Efeitos)
    public enum TipoDeAudio { Musica, SFX }

    [Header("Configuração")]
    public TipoDeAudio tipoBotao;

    [Header("Referências de UI")]
    [SerializeField] private Image iconeSomImage;
    [SerializeField] private Sprite spriteSomLigado;    // Vermelho (Ativo)
    [SerializeField] private Sprite spriteSomDesligado; // Branco (Inativo)

    private Button meuBotao;

    void Start()
    {
        meuBotao = GetComponent<Button>();

        if (meuBotao != null)
        {
            // Remove ouvintes antigos para evitar cliques duplos ou erros
            meuBotao.onClick.RemoveAllListeners();

            // Configura o clique dependendo do tipo selecionado no Inspector
            if (tipoBotao == TipoDeAudio.Musica)
            {
                // Chama a nova função de música
                meuBotao.onClick.AddListener(() => SoundController.Instance.LigarDesligarMusica());
            }
            else // SFX
            {
                // Chama a nova função de SFX
                meuBotao.onClick.AddListener(() => SoundController.Instance.AlternarSFX());
            }
        }

        InscreverNosEventos();
    }

    void InscreverNosEventos()
    {
        if (SoundController.Instance == null) return;

        if (tipoBotao == TipoDeAudio.Musica)
        {
            // Pega estado inicial da música (novo nome: MusicaEstaLigada)
            AtualizarSpriteUI(SoundController.Instance.MusicaEstaLigada);

            // Inscreve no evento de música (novo nome: OnEstadoMusicaChanged)
            SoundController.Instance.OnEstadoMusicaChanged += AtualizarSpriteUI;
        }
        else // SFX
        {
            // Pega estado inicial do SFX (novo nome: SFXEstaLigado)
            AtualizarSpriteUI(SoundController.Instance.SFXEstaLigado);

            // Inscreve no evento de SFX (novo nome: OnEstadoSFXChanged)
            SoundController.Instance.OnEstadoSFXChanged += AtualizarSpriteUI;
        }
    }

    private void AtualizarSpriteUI(bool estaLigado)
    {
        if (iconeSomImage == null) return;
        iconeSomImage.sprite = estaLigado ? spriteSomLigado : spriteSomDesligado;
    }

    void OnDestroy()
    {
        // Remove a inscrição correta para evitar erros ao mudar de cena
        if (SoundController.Instance != null)
        {
            if (tipoBotao == TipoDeAudio.Musica)
                SoundController.Instance.OnEstadoMusicaChanged -= AtualizarSpriteUI;
            else
                SoundController.Instance.OnEstadoSFXChanged -= AtualizarSpriteUI;
        }
    }

   
}