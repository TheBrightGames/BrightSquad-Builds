using UnityEngine;
using UnityEngine.EventSystems; // Necessário para toque

public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    // Singleton para acessarmos fácil de qualquer lugar
    public static VirtualJoystick Instance;

    [Header("Configuração Visual")]
    public RectTransform handleRect;   // A bolinha que mexe (JoystickHandle)
    public float range = 100f;         // O quão longe a bolinha pode ir

    // O vetor que o seu personagem vai ler (x, y entre -1 e 1)
    private Vector2 inputVector;
    public Vector2 InputVector => inputVector;

    private Vector2 backgroundPosition;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // Salva a posição inicial do fundo do joystick
        if (handleRect != null)
        {
            // Centraliza o handle
            handleRect.anchoredPosition = Vector2.zero;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData); // Começa a arrastar imediatamente ao tocar
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 position;
        RectTransform backgroundRect = GetComponent<RectTransform>();

        // Converte a posição do toque na tela para posição dentro do Canvas
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            backgroundRect,
            eventData.position,
            eventData.pressEventCamera,
            out position))
        {
            // Calcula a posição relativa (0 a 1)
            position.x = (position.x / backgroundRect.sizeDelta.x);
            position.y = (position.y / backgroundRect.sizeDelta.y);

            // Calcula o vetor de entrada
            inputVector = new Vector2(position.x * 2, position.y * 2);

            // Limita o vetor para ser um círculo (magnitude 1)
            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

            // Move a parte visual (Handle)
            if (handleRect != null)
            {
                handleRect.anchoredPosition = new Vector2(
                    inputVector.x * (backgroundRect.sizeDelta.x / 3),
                    inputVector.y * (backgroundRect.sizeDelta.y / 3)
                );
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Quando solta o dedo, zera tudo
        inputVector = Vector2.zero;
        if (handleRect != null)
            handleRect.anchoredPosition = Vector2.zero;
    }
}