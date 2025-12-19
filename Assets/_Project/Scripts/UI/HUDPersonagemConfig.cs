using UnityEngine;

[System.Serializable]
public class DadosHUDPersonagem
{
    public string nomeExibicao;
    public Sprite avatarCircular;
}

public class HUDPersonagemConfig : MonoBehaviour
{
    public DadosHUDPersonagem[] personagensHUD;
    private const string CHAVE_PLAYER = "PersonagemSelecionado";

    [Header("HUD")]
    public UnityEngine.UI.Text textoNome;
    public UnityEngine.UI.Image imagemAvatar;

    void Start()
    {
        int indice = PlayerPrefs.GetInt(CHAVE_PLAYER, 0);
        if (personagensHUD == null || personagensHUD.Length == 0) return;

        indice = Mathf.Clamp(indice, 0, personagensHUD.Length - 1);

        var dados = personagensHUD[indice];

        if (textoNome != null) textoNome.text = dados.nomeExibicao;
        if (imagemAvatar != null && dados.avatarCircular != null)
            imagemAvatar.sprite = dados.avatarCircular;
    }
}
