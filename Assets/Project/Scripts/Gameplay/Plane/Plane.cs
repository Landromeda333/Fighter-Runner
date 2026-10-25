using UnityEngine;

public class Plane : MonoBehaviour
{
    [SerializeField] GameEvent planeAvailable ,playerCrashed;

    [SerializeField] IntGameEvent onSpeedChanged;

    public static Plane Instance;

    /* Referencias a GameObjects */
    [SerializeField] GameObject[] engines  = new GameObject[2];       // Referencia a los motores del avión

    public ParticleSystem[] enginesParticles;           // Partículas de postquemadores

    AudioSource source;

    Animator animator;                                                // Referencia al componente Animator del jugador

    public float minX = -27, maxX = 20, minY = 4, maxY = 30, minZ=-75;// Límites de movimiento del jugador en el eje X, Y y Z en el menú de información de controles
    [HideInInspector] public float speed;                                               // Velocidad del jugador, se incrementa a medida que el jugador avanza
    const float maxSpeed = 200;

    int lastWholeSecond;

    [HideInInspector] public bool takenOff;                                             // Indica si el jugador ha despegado del suelo

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }
        source = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    public void StartGameHandle()
    {
        enabled = true;
        animator.SetBool("TakeOff", true);                      // Se activa la animación de despegue
    }

    void Update()
    {
        if (speed < maxSpeed)                        // Si ya ha despegado se mueve más rápido
        {
            speed += 1.5f * Time.deltaTime;
            int wholeSecond = Mathf.FloorToInt(speed/5);
            if (wholeSecond != lastWholeSecond)
            {
                lastWholeSecond = wholeSecond;
                onSpeedChanged.Raise(wholeSecond);
            }
        }
        else
        {
            enabled = false;
        }

        foreach (ParticleSystem engineParticles in enginesParticles)
        {
            var main2 = engineParticles.main;
            main2 = engineParticles.main;
            main2.startLifetime = speed / 100;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
            playerCrashed.Raise();                          // Llama al método GameOver del UIManager cuando el jugador colisiona con algo
    }

    /* Métodos para Game Events */
    public void OnPause()
    {
        source.Pause();
        enabled = false;
    }

    public void OnContinueGame()
    {
        source.Play();
        enabled = speed < 200;
    }

    public void OnEndGame()
    {
        enabled = false;
        source.Stop();
    }

    /* Animation Event */
    public void TakenOffHandle() => takenOff = true;

    public void TurnOnEngines()
    {
        foreach (GameObject engine in engines)
        {
            engine.SetActive(true);                             // Se activan los postquemadores
        }

        /*
         * Reproducción del sonido de los postquemadores en el momento del despegue
        audioController.masterAudioSource[1].clip = audioController.audioClipsAircraft[2]; // Clip de audio de postquemadores
        audioController.masterAudioSource[1].loop = true;
        audioController.masterAudioSource[1].Play();
        audioController.masterAudioSource[2].enabled = true;
        */
    }

    public void StartGameMode()
    {
        planeAvailable.Raise();
        minX = -27f;
        maxX = 27f;
        minY = 0f;
        maxY = 27f;

        /*
         * Reproducción del sonido normal del avión al terminar el despegue
        audioController.musicAudioSource.Play();
        audioController.masterAudioSource[1].clip = audioController.audioClipsAircraft[1]; // Cambiar el clip de audio al de vuelo
        audioController.masterAudioSource[1].Play();
        audioController.masterAudioSource[1].loop = false; // Hacer que el audio del avión se reproduzca en bucle
        */
    }
}