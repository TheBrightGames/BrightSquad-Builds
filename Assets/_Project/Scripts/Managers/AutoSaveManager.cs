using System.Collections;
using UnityEngine;

public class AutoSaveManager : MonoBehaviour
{
    [Header("Config")]
    public float intervaloSalvamento = 60f; // segundos

    [Header("UI")]
    public GameObject savingIcon; // imagem ou painel “Salvando...”

    [Header("Refs")]
    public GameManager gameManager; // arraste o GameManager aqui

    void Start()
    {
        if (savingIcon != null)
            savingIcon.SetActive(false);

        if (gameManager == null)
            gameManager = FindFirstObjectByType<GameManager>();

        StartCoroutine(AutoSaveLoop());
    }

    IEnumerator AutoSaveLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(intervaloSalvamento);
            yield return StartCoroutine(SalvarComIcone());
        }
    }

    IEnumerator SalvarComIcone()
    {
        if (savingIcon != null)
            savingIcon.SetActive(true);

        SalvarEstadoRapido();   // chama o mesmo quick save do B

        yield return new WaitForSeconds(0.7f);

        if (savingIcon != null)
            savingIcon.SetActive(false);
    }

    void SalvarEstadoRapido()
    {
        if (gameManager == null)
        {
            Debug.LogWarning("[AUTOSAVE] GameManager não encontrado.");
            return;
        }

        gameManager.SalvarProgressoRapido();   // MESMO método do B
        Debug.Log("[AUTOSAVE] Salvo em " + Time.time + "s");
    }
}
