using UnityEngine;
using UnityEngine.InputSystem;
public class InputsManager : MonoBehaviour
{
    public static InputsManager Instance;   // Instancia única del InputManager

    public PlayerController player;         // Referencia al controlador del jugador
    public UIManager uIManager;             // Referencia al controlador de la interfaz de usuario
    public Vector2 MoveInput;               // Entrada de movimiento del jugador (X, Y)
    public float RollInput;                 // Roll del avión (rotación en Z)
    public float FireInput;                 // Disparo del avión (cañón)
    public float LaunchInput;               // Lanzamiento de misiles

    Controls controls;                      // Controles generados por el Input System

    void Awake()
    {
        // Se asegura de que solo haya una instancia de InputManager para evitar problemas con el sistema de inputs
        // Método Singleton para InputManager
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Persistente entre escenas

        // Inicializar los controles
        controls = new Controls();

        // Performed == Presionado, canceled == Soltado
        controls.Plane.Warp.performed += ctx => MoveInput = new Vector2(ctx.ReadValue<float>(), MoveInput.y);
        controls.Plane.Warp.canceled += ctx => MoveInput = new Vector2(0, MoveInput.y);

        controls.Plane.Pitching.performed += ctx => MoveInput = new Vector2(MoveInput.x, ctx.ReadValue<float>());
        controls.Plane.Pitching.canceled += ctx => MoveInput = new Vector2(MoveInput.x, 0);

        controls.Plane.Yaw.performed += ctx => RollInput = ctx.ReadValue<float>();
        controls.Plane.Yaw.canceled += ctx => RollInput = 0;

        controls.Plane.Fire.performed += ctx => FireInput = ctx.ReadValue<float>();
        controls.Plane.Fire.canceled += ctx => FireInput = 0;

        controls.Plane.LaunchMissiles.performed += ctx => player.LaunchMissile();

        controls.Cameras.SwitchToCam1.performed += ctx => uIManager.SwitchCamera(1);
        controls.Cameras.SwitchToCam2.performed += ctx => uIManager.SwitchCamera(2);
        controls.Cameras.SwitchToCam3.performed += ctx => uIManager.SwitchCamera(3);

        controls.GamePause.PauseGame.performed += ctx => uIManager.PauseGame();
    }
    void OnEnable()
    {
        if (controls != null)
            controls.Enable();
    }

    void OnDisable()
    {
        if (controls != null)
            controls.Disable();
    }
}