using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovements : MonoBehaviour
{
    [Header("Components")]
    private CharacterController controller;
    private Transform myCamera;
    private Animator animator;
    [SerializeField] private Transform foot;
    [SerializeField] private LayerMask colisionLayer;

    [Header("Movement")]
    public float velocity = 5f;
    private bool isGround;
    private float yForce;

    [Header("Look")]
    public InputActionReference moveAction; // Move (WASD + left stick)
    public InputActionReference lookAction; // Look (mouse delta + right stick)
    public InputActionReference jumpAction; // Jump
    public float lookSensitivity = 100f;

    Vector2 moveInput;
    Vector2 lookInput;
    float cameraPitch;

    [Header("Audio")]
    [SerializeField] private AudioSource passosAudioSouce;
    [SerializeField] private AudioClip[] passosAudioClip;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        myCamera = Camera.main.transform;
        animator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        if (moveAction != null) moveAction.action.Enable();
        if (lookAction != null) lookAction.action.Enable();

        if (jumpAction != null)
        {
            jumpAction.action.performed += OnJump;
            jumpAction.action.Enable();
        }
    }

    void OnDisable()
    {
        if (moveAction != null) moveAction.action.Disable();
        if (lookAction != null) lookAction.action.Disable();

        if (jumpAction != null)
        {
            jumpAction.action.performed -= OnJump;
            jumpAction.action.Disable();
        }
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        moveInput = moveAction != null ? moveAction.action.ReadValue<Vector2>() : Vector2.zero;
        lookInput = lookAction != null ? lookAction.action.ReadValue<Vector2>() : Vector2.zero;

        Move();
        Look();
        ApplyGravityAndMoveY();

        // debug PC: liberar cursor com P
        if (Keyboard.current != null && Keyboard.current.pKey.wasPressedThisFrame)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void Move()
    {
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
        move = Vector3.ClampMagnitude(move, 1f);
        move = myCamera.TransformDirection(move);
        move.y = 0;

        controller.Move(move * velocity * Time.deltaTime);

        if (move != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(move),
                Time.deltaTime * 10f
            );
        }

        animator.SetBool("Move", move != Vector3.zero);

        isGround = Physics.CheckSphere(foot.position, 0.3f, colisionLayer);
        animator.SetBool("isGround", isGround);
    }

    void Look()
    {
        // Se estiver usando Cinemachine FreeLook, NÃO gira o player aqui.
        // Deixa apenas a câmera cuidar da rotação com o mouse.

        // Se quiser manter o pitch da câmera manual (sem Cinemachine), comentaria a linha abaixo.
        if (myCamera == null) return;

        float pitchDelta = -lookInput.y * lookSensitivity * Time.deltaTime;
        cameraPitch = Mathf.Clamp(cameraPitch + pitchDelta, -60f, 60f);
        myCamera.localEulerAngles = new Vector3(cameraPitch, 0, 0);
    }


    void ApplyGravityAndMoveY()
    {
        if (isGround && yForce < 0)
            yForce = -2f;

        yForce += Physics.gravity.y * Time.deltaTime;
        controller.Move(new Vector3(0, yForce, 0) * Time.deltaTime);
    }

    void OnJump(InputAction.CallbackContext ctx)
    {
        if (!isGround) return;

        yForce = 5f;
        animator.SetTrigger("Jump");
    }

    void Passos()
    {
        if (passosAudioSouce != null && passosAudioClip.Length > 0)
            passosAudioSouce.PlayOneShot(passosAudioClip[Random.Range(0, passosAudioClip.Length)]);
    }
}
