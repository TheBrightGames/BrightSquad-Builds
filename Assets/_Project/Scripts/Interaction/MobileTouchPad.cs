using UnityEngine;
using UnityEngine.EventSystems;

public class MobileTouchPad : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    public static MobileTouchPad Instance;

    private Vector2 touchDelta;
    public Vector2 LookInput => touchDelta; // Propriedade que o InputManagerGlobal lê

    [Header("Sensibilidade")]
    public float sensibilidade = 0.5f;

    void Awake()
    {
        Instance = this;
    }

    // Chamado enquanto o dedo se move na área
    public void OnDrag(PointerEventData data)
    {
        touchDelta = data.delta * sensibilidade;
    }

    // Chamado quando solta o dedo
    public void OnPointerUp(PointerEventData data)
    {
        touchDelta = Vector2.zero;
    }

    // Chamado quando encosta o dedo
    public void OnPointerDown(PointerEventData data)
    {
        OnDrag(data);
    }

    // Zera o delta no final do frame para a câmera parar de girar se o dedo parar
    void LateUpdate()
    {
        if (touchDelta != Vector2.zero)
            touchDelta = Vector2.zero;
    }
}