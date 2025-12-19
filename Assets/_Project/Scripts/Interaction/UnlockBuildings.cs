using UnityEngine;

public class UnlockBuildings : MonoBehaviour
{
    [Header("Objetos que serão desbloqueados")]
    public GameObject bombeiros;
    public GameObject delegacia;
    public GameObject oficina;
    public GameObject melhorias;

    [Header("Chave de save")]
    public string playerPrefKey = "PrediosDesbloqueados";

    void Start()
    {
        // Ao carregar a cena, verifica se já estavam desbloqueados
        bool jaDesbloqueou = PlayerPrefs.GetInt(playerPrefKey, 0) == 1;

        if (jaDesbloqueou)
        {
            SetPrediosAtivos(true);
        }
        else
        {
            SetPrediosAtivos(false);
        }
    }

    public void DesbloquearPredios()
    {
        SetPrediosAtivos(true);
        PlayerPrefs.SetInt(playerPrefKey, 1);
        PlayerPrefs.Save();
    }

    void SetPrediosAtivos(bool ativo)
    {
        if (bombeiros != null) bombeiros.SetActive(ativo);
        if (delegacia != null) delegacia.SetActive(ativo);
        if (oficina != null) oficina.SetActive(ativo);
        if (melhorias != null) melhorias.SetActive(ativo);
    }
}
