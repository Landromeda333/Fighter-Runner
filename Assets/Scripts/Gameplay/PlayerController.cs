using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    /* Referencias a Scripts */
    [SerializeField] ObjectPooler pooler;                             // Pooler para las balas
    [SerializeField] UIManager uIController;                          // UI Manager para manejar la interfaz de usuario
    [SerializeField] AudioController audioController;                 // Controlador de audio para manejar los sonidos del juego
    [SerializeField] GameControler gameController;                    // Controlador del juego para manejar el estado del juego

    /* Referencias a GameObjects */
    [SerializeField] GameObject[] engines  = new GameObject[2];       // Referencia a los motores del avión
    [SerializeField] GameObject muzzleFlash;                          // Referencia al objeto de destello del cañón
    [SerializeField] GameObject[] missiles = new GameObject[4];       // Referencia a los misiles del avión
    [SerializeField] Transform floor;                                 // Referencia a los aviones del juego

    Animator animator;                                                // Referencia al componente Animator del jugador

    Coroutine shootCoroutine;                                         // Coroutine para manejar el disparo

    Vector3 moveDirection;                                            // Dirección de movimiento del jugador
    Vector3 pos;                                                      // Posición del jugador

    const float maneuverability = 20;                                 // Manejo de la maniobrabilidad del jugador, se usa para controlar la velocidad de movimiento
    public float minX = -27, maxX = 20, minY = 4, maxY = 30, minZ=-75;// Límites de movimiento del jugador en el eje X, Y y Z en el menú de información de controles
    public float speed;                                               // Velocidad del jugador, se incrementa a medida que el jugador avanza
    public bool takenOff;                                             // Indica si el jugador ha despegado del suelo
    public bool shootStarted;                                         // Indica si el disparo ha comenzado
    public enum InverseYAxis                                          // Enumeración para manejar la inversión del eje Y
    {
        Normal = 1,
        Inverse = -1
    };

    private void OnEnable()
    {
        if (InputsManager.Instance != null)                         
        {
            InputsManager.Instance.player = (this);
        }
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        // Si el avión ha despegado o está en el menú de información de controles, se permite el movimiento
        if ((gameController.gameState == GameControler.GameState.Play && takenOff) || (gameController.gameState == GameControler.GameState.Standby && uIController.informationWindow.activeSelf))
        {
            moveDirection = new Vector3(InputsManager.Instance.MoveInput.x, PlayerPrefs.GetInt("inverseYAxis") * InputsManager.Instance.MoveInput.y, 0).normalized; // Se obtienen los valores de los inputs
            transform.Translate(moveDirection * maneuverability * Time.deltaTime);           // Se le aplica la velocidad de movimiento
            transform.Rotate(0, 0, InputsManager.Instance.RollInput * 200 * Time.deltaTime); // Movimiento de rotación del avión

            // Limitación de movimiento del jugador dentro de los límites establecidos
            pos = transform.position;
            pos.x = Mathf.Clamp(pos.x, minX, maxX);
            pos.y = Mathf.Clamp(pos.y, minY, maxY);
            pos.z = minZ;
            transform.position = pos;
            if (transform.position.y <= floor.position.y + 8 && !audioController.masterAudioSource[13].isPlaying)
            {
                audioController.masterAudioSource[13].Play(); // Reproduce el sonido de motor si el avión está cerca del suelo
                uIController.warningPullUp.enabled = true;    // Muestra la advertencia de "Pull Up" si el avión está cerca del suelo
            }
            else if (transform.position.y > floor.position.y + 8 && audioController.masterAudioSource[13].isPlaying)
            {
                audioController.masterAudioSource[13].Stop(); // Para el sonido de motor si el avión está cerca del suelo
                uIController.warningPullUp.enabled = false;   // Muestra la advertencia de "Pull Up" si el avión está cerca del suelo
            }
        }
    }

    void Update()
    {
        if (InputsManager.Instance.FireInput == 1 && shootCoroutine == null)
        {
            shootCoroutine = StartCoroutine(Fire());          // Para no tener problemas de superposición de corrutinas se le asigna una corrutina a la variable para que se pueda detener
        }

        if (InputsManager.Instance.FireInput == 0 && shootCoroutine != null)
        {
            StopCoroutine(shootCoroutine);
            shootCoroutine = null;                            // Se vacía la corrutina para evitar problemas
            shootStarted = false;
            muzzleFlash.SetActive(false);                     // Se quita el flash del cañón por si se interrumpe el disparo para que no se quede puesto
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Bullet")
        {
            uIController.GameOver();                          // Llama al método GameOver del UIManager cuando el jugador colisiona con algo
        }
    }

    /* Métodos */
    public void LaunchMissile()
    {
        foreach (GameObject missile in missiles)
        {
            if (!missile.GetComponent<RocketAction>().launchStarted) // Si el misil no está activo
            {
                missile.GetComponent<RocketAction>().StartLaunch();
                break;                                               // Sale del bucle una vez se ha activado un misil
            }
        }
    }

    /* Corrutinas */
    // Despegue del avión
    public IEnumerator TakeOff()
    {
        GetComponent<Rigidbody>().isKinematic = true;           // Se pone para que no afecte a la animación de despegue
        gameController.gameState = GameControler.GameState.Play;// Se inicia el juego para que se empiece a mover el ScrollParallax
        animator.SetBool("TakeOff", true);                      // Se activa la animación de despegue
        yield return new WaitForSeconds(5);                     // Tiempo de espera para activar los postquemadores
        foreach (GameObject engine in engines)
        {
            engine.SetActive(true);                             // Se activan los postquemadores
        }
        audioController.masterAudioSource[1].clip = audioController.audioClipsAircraft[2]; // Clip de audio de postquemadores
        audioController.masterAudioSource[1].loop = true;
        audioController.masterAudioSource[1].Play();
        audioController.masterAudioSource[2].enabled = true;
        yield return new WaitForSeconds(11);                    // Tiempo para que se termine la animación
        GetComponent<Rigidbody>().isKinematic = false;
        takenOff = true;
        // Cambio de valores para la partida
        minX = -27f;
        maxX = 27f;
        minY = 0f;
        maxY = 27f;
    }

    // Disparo del avión
    IEnumerator Fire()
    {
        if (!shootStarted)
        {
            yield return new WaitForSeconds(0.5f);                // Tiempo inicial antes de poder disparar
            shootStarted = true;
        }
        while (true)                                            // Se repite mientras se mantenga pulsado
        {
            GameObject bullet = pooler.GetBullet();             // Se intancia una bala del pooler
            bullet.SetActive(true);
            muzzleFlash.SetActive(true);                        // Se activa el flash del cañón
            yield return new WaitForSeconds(0.05f);             // Tiempo de espera para el efecto visual del flash
            muzzleFlash.SetActive(false);
            yield return new WaitForSeconds(0.05f);              // Delay entre disparos
        }
    }
}