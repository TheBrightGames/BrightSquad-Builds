using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    PlayerInput input;


    public InputActionReference moveAction; // arrasta a action Move do .inputactions

    Vector2 moveInput;

    void OnEnable()
    {
        moveAction.action.Enable();
    }

    void OnDisable()
    {
        moveAction.action.Disable();
    }

    void Update()
    {
        moveInput = moveAction.action.ReadValue<Vector2>();
        // use moveInput.x / moveInput.y para mover o Naldinho
    }

    void Awake()
    {
        instance = this;
        input = GetComponent<PlayerInput>();
    }

    public void HabilitarControle(bool valor)
    {
        if (input != null)
            input.enabled = valor;
    }
}
