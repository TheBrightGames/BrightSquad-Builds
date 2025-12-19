using UnityEngine;

public class UIDialogueButtons : MonoBehaviour
{
    [Header("Botão de interagir (E / toque)")]
    public GameObject interactButton;   // aparece só perto do NPC

    [Header("Botões de diálogo (TAB / mobile)")]
    public GameObject[] dialogueButtons; // aparecem só com diálogo aberto

    void Awake()
    {
        ShowInteract(false);
        ShowDialogue(false);
    }

    public void ShowInteract(bool show)
    {
        if (interactButton != null)
            interactButton.SetActive(show);
    }

    public void ShowDialogue(bool show)
    {
        if (dialogueButtons == null) return;

        foreach (var b in dialogueButtons)
            if (b != null) b.SetActive(show);
    }
}
