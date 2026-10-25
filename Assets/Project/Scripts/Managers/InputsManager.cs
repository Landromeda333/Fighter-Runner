using UnityEngine;
using UnityEngine.InputSystem;

public class InputsManager : MonoBehaviour
{
    public static InputsManager Instance;   // Instancia única del InputManager

    [HideInInspector] public PlaneMovement movement;
    [HideInInspector] public PlaneAttack attack;

    [HideInInspector] public Vector2 moveInput;               // Entrada de movimiento del jugador (X, Y)

    [HideInInspector] public float rollInput;                 // Roll del avión (rotación en Z)

    PlayerInputActions playerInputs;        // Controles generados por el Input System

    [SerializeField] IntGameEvent switchCameraRequest;
    [SerializeField] GameEvent cancelPerformed, onAnyKeyPerformed, onPauseRequest;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
            return;
        }
        playerInputs = new PlayerInputActions();
    }

    private void Start()
    {
        ActivateUIInput();
    }

    void OnMovementRequest(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }

    void OnWarpPerformed(InputAction.CallbackContext ctx)
    {
        rollInput = ctx.ReadValue<float>();
    }

    void OnFirePerformed(InputAction.CallbackContext ctx)
    {
        attack.FireHandle(ctx.ReadValue<float>());
    }

    void OnLaunchMissilePerformed(InputAction.CallbackContext ctx)
    {
        attack.LaunchMissile();
    }

    void OnPausePerformed(InputAction.CallbackContext ctx)
    {
        onPauseRequest.Raise();
    }

    void SwitchCamera1Performed(InputAction.CallbackContext ctx)
    {
        switchCameraRequest.Raise(1);
    }
    void SwitchCamera2Performed(InputAction.CallbackContext ctx)
    {
        switchCameraRequest.Raise(2);
    }
    void SwitchCamera3Performed(InputAction.CallbackContext ctx)
    {
        switchCameraRequest.Raise(3);
    }
    public void ActivatePlayerInput()
    {
        DeactivateUIInput();
        playerInputs.Player.Enable();
        playerInputs.Player.Warp.performed += OnWarpPerformed;
        playerInputs.Player.Warp.canceled += OnWarpPerformed;

        playerInputs.Player.Movement.performed += OnMovementRequest;
        playerInputs.Player.Movement.canceled += OnMovementRequest;

        playerInputs.Player.Fire.performed += OnFirePerformed;
        playerInputs.Player.Fire.canceled += OnFirePerformed;

        playerInputs.Player.LaunchMissiles.performed += OnLaunchMissilePerformed;

        playerInputs.Player.SwitchToCam1.performed += SwitchCamera1Performed;
        playerInputs.Player.SwitchToCam2.performed += SwitchCamera2Performed;
        playerInputs.Player.SwitchToCam3.performed += SwitchCamera3Performed;

        playerInputs.Player.Pause.performed += OnPausePerformed;
    }

    void DeactivatePlayerInput()
    {
        playerInputs.Player.Warp.performed -= OnWarpPerformed;
        playerInputs.Player.Warp.canceled -= OnWarpPerformed;

        playerInputs.Player.Movement.performed -= OnMovementRequest;
        playerInputs.Player.Movement.canceled -= OnMovementRequest;

        playerInputs.Player.Fire.performed -= OnFirePerformed;
        playerInputs.Player.Fire.canceled -= OnFirePerformed;

        playerInputs.Player.LaunchMissiles.performed -= OnLaunchMissilePerformed;

        playerInputs.Player.SwitchToCam1.performed -= SwitchCamera1Performed;
        playerInputs.Player.SwitchToCam2.performed -= SwitchCamera2Performed;
        playerInputs.Player.SwitchToCam3.performed -= SwitchCamera3Performed;

        playerInputs.Player.Pause.performed -= OnPausePerformed;

        playerInputs.Player.Disable();
    }

    // Activación de los controles de la UI
    public void ActivateUIInput()
    {
        DeactivatePlayerInput();
        playerInputs.UI.Enable();
        playerInputs.UI.Cancel.performed += OnCancelPerformed;
    }

    // Desactivación de los controles de la UI
    void DeactivateUIInput()
    {
        playerInputs.UI.Cancel.performed -= OnCancelPerformed;
        playerInputs.UI.Disable();
    }

    void OnCancelPerformed(InputAction.CallbackContext ctx)
    {
        cancelPerformed.Raise();
    }

    public void ResetValues()
    {
        moveInput = Vector2.zero;               // Entrada de movimiento del jugador (X, Y)
        rollInput = 0;                 // Roll del avión (rotación en 
    }
}