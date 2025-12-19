using UnityEngine;
using UnityEngine.InputSystem;

public class Dialogue : MonoBehaviour
{
    [Header("Variables")]
    public string[] speechText;
    public string actorName;
    private DialogueControl dc;
    private bool onRadious;
    private bool isDialogueActive = false;
    public LayerMask playerLayer;
    public float radious;

    private Animator animator;

    // NOVO: referência para desbloqueio
    public UnlockBuildings unlockBuildings;
    public bool desbloqueiaAoTerminar = false;
    public GameObject interactionIcon;
    public UIDialogueButtons uiButtons;




    void Start()
    {
        dc = FindFirstObjectByType<DialogueControl>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        Interact();
    }

    void Update()
    {
        if (Keyboard.current != null &&
            Keyboard.current.eKey.wasPressedThisFrame &&
            onRadious && !isDialogueActive)
        {
            StartDialogue();
        }
    }

    public void StartDialogue()
    {
        isDialogueActive = true;
        animator.SetBool("isTalking", true);
        dc.Speech(speechText, actorName);
        Debug.Log("Dialogo iniciado com " + actorName);

        if (interactionIcon != null)
            interactionIcon.SetActive(false);

        if (uiButtons != null)
        {
            uiButtons.ShowInteract(false);   // some o “E”
            uiButtons.ShowDialogue(true);    // aparecem os 2 botões do mobile / TAB
        }


    }

    // ÚNICO EndDialogue
    private void EndDialogue()
    {
        isDialogueActive = false;
        animator.SetBool("isTalking", false);
        dc.HidePanel();
        Debug.Log("Dialogo encerrado!");

        if (desbloqueiaAoTerminar && unlockBuildings != null)
        {
            unlockBuildings.DesbloquearPredios();
        }


        if (uiButtons != null)
        {
            uiButtons.ShowDialogue(false);

            // se ainda estiver no raio, pode voltar a mostrar o botão de interagir
            uiButtons.ShowInteract(onRadious);
        }

        // resto do seu código de fechar diálogo...


    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, radious);
    }

    public void Interact()
    {
        Vector3 point1 = transform.position + Vector3.up * radious;
        Vector3 point2 = transform.position - Vector3.up * radious;

        Collider[] hits = Physics.OverlapCapsule(
            point1, point2, radious, playerLayer);

        if (hits.Length > 0)
        {
            if (!onRadious)
            {
                onRadious = true;

                if (interactionIcon != null)
                    interactionIcon.SetActive(true);

                if (uiButtons != null && !isDialogueActive)
                    uiButtons.ShowInteract(true);   // só o botão de interagir
            }
        }
        else
        {
            if (onRadious)
            {
                onRadious = false;

                if (interactionIcon != null)
                    interactionIcon.SetActive(false);

                if (uiButtons != null)
                {
                    uiButtons.ShowInteract(false);
                    uiButtons.ShowDialogue(false);
                }
            }

            if (isDialogueActive)
                EndDialogue();
        }



    }
}
