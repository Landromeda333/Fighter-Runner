using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    /* Accesos a Scripts */
    [SerializeField] GameControler gameControler;       // Referencia al controlador del juego
    [SerializeField] PlayerController playerController; // Referencia al controlador del jugador
    [SerializeField] AudioController audioController;   // Referencia al controlador de audio

    /* Textos */
    [SerializeField] TextMeshProUGUI scoreText;         // Texto para mostrar la puntuación del jugador
    [SerializeField] TextMeshProUGUI timePlayedText;    // Texto para mostrar el tiempo jugado
    [SerializeField] TextMeshProUGUI speedText;         // Texto para mostrar la velocidad del jugador
    [SerializeField] TextMeshProUGUI recordScoreText;   // Texto para mostrar el record de puntuación
    [SerializeField] TextMeshProUGUI finishScoreText;   // Texto para mostrar la puntuación final al terminar el juego
    public TextMeshProUGUI warningPullUp;               // Texto que parpadea en el menú principal

    /* Game Objects */
    [SerializeField] GameObject scoreObject;            // Objeto que contiene el texto de puntuación
    [SerializeField] GameObject timePlayedObject;       // Objeto que contiene el texto de tiempo jugado
    [SerializeField] GameObject gameOverWindow;         // Ventana que se muestra al finalizar el juego
    [SerializeField] GameObject mainMenuWindow;         // Ventana del menú principal
    [SerializeField] GameObject mainMenu;               // Objeto que contiene el contenido del menú principal
    [SerializeField] GameObject configMenuWindow;       // Ventana de configuración del menú principal
    [SerializeField] GameObject configButton;           // Botón de configuración en el menú principal
    [SerializeField] GameObject startBtn;               // Botón de inicio del juego en el menú principal
    [SerializeField] GameObject pauseWindow;            // Ventana de pausa del juego
    [SerializeField] GameObject pauseSection;           // Sección de pausa en la ventana de pausa
    [SerializeField] GameObject soundWindow;            // Ventana de sonido en la sección de configuración
    [SerializeField] GameObject settingsSection;        // Sección de ajustes en la ventana de pausa
    [SerializeField] GameObject controlsWindow;         // Ventana de controles en la sección de configuración
    [SerializeField] GameObject recordBox;              // Ventana de records del juego
    [SerializeField] GameObject[] crosshires = new GameObject[3];// Objeto que contiene el crosshair del jugador
    [SerializeField] GameObject[] informationContent = new GameObject[2];// Pestañas de la ventana de información (controles[0], comoJugar[1])
    public GameObject informationWindow;      // Ventana de información del menú principal

    /* Sistema de partículas */
    [SerializeField] ParticleSystem speedParticles;     // Partículas de velocidad
    public ParticleSystem[] enginesParticles;           // Partículas de postquemadores

    /* Toggle */
    [SerializeField] Toggle InverseYAxisToggle;         // Toggle para invertir el eje Y del movimiento del jugador

    /* Sistema de Inputs*/
    Controls controls;                                  // Sistema de Inputs

    /* Cámaras */
    public Camera[] cameras = new Camera[3];                              // Cámara del morro

    public int highScore;                               // Record de puntuación del jugador

    TimeSpan ts;                                        // Variable para mostrar el tiempo jugado en formato mm:ss

    float blinkSpeed = 2f;

    private void Awake()
    {
        controls = new Controls();                      // Inicializar el sistema de controles
        highScore = PlayerPrefs.GetInt("highScore1"); // Cargar el record desde PlayerPrefs
    }

    private void OnEnable()
    {
        if (InputsManager.Instance != null)
        {
            InputsManager.Instance.uIManager = (this);
        }
    }

    private void Update()
    {
        if (gameControler.gameState == GameControler.GameState.Play) // Si el juego está en estado de juego
        {
            ts = TimeSpan.FromSeconds(gameControler.time);           // Muestra el tiempo jugado en formato mm:ss
            scoreText.text = gameControler.score.ToString("0");
            timePlayedText.text = ts.ToString(@"mm\:ss");
            speedText.text = "Speed: " + (playerController.speed / 2).ToString("0") + "%";  // Muestra la velocidad del jugador en porcentaje

            // Modificación de las partículas de velocidad según la velocidad del jugador (Más velocidad, más notorio)
            var main = speedParticles.main;
            main.startSpeed = playerController.speed;
            main.maxParticles = (int)playerController.speed;

            // Ajuste del tamaño de las partículas de los motores según la velocidad del jugador
            foreach (ParticleSystem engineParticles in enginesParticles)
            {
                var main2 = engineParticles.main;
                main2 = engineParticles.main;
                main2.startLifetime = playerController.speed / 100;
            }
        }
    }

    private void LateUpdate()
    {
        if (warningPullUp.enabled)
        {
            float alpha = Mathf.PingPong(Time.time * blinkSpeed, 1f);
            warningPullUp.color = new Color(warningPullUp.color.r, warningPullUp.color.g, warningPullUp.color.b, alpha);
        }
    }

    /* Métodos */
    // Cambiar de cámara
    public void SwitchCamera(int index)
    {
        if (gameControler.gameState == GameControler.GameState.Play && playerController.takenOff)   // Para evitar cambiar de cámara en el pause y game over, y evitar un bug en el despegue con la cámara 1
        {
            //En vez de hacer un for o un switch para activar/desactivar las cámaras (que es un tocho grande de código), simplemente se activan las cosas si el índice coincide
            cameras[0].enabled = (index == 1);
            cameras[1].enabled = (index == 2);
            cameras[2].enabled = (index == 3);

            cameras[0].GetComponent<AudioListener>().enabled = (index == 1);
            cameras[1].GetComponent<AudioListener>().enabled = (index == 2);
            cameras[2].GetComponent<AudioListener>().enabled = (index == 3);

            crosshires[0].SetActive(index == 1);
            crosshires[1].SetActive(index == 2);
            crosshires[2].SetActive(index == 3);
        }
    }

    // Cargar el nivel 1
    public void ChangeScene()
    {
        SceneManager.LoadScene("Level_1");
    }

    // Empezar el juego
    public void BtnPlay()
    {
        playerController.StartCoroutine(playerController.TakeOff());
        audioController.musicAudioSource.Play();
        audioController.masterAudioSource[1].clip = audioController.audioClipsAircraft[1]; // Cambiar el clip de audio al de vuelo
        audioController.masterAudioSource[1].Play();
        audioController.masterAudioSource[1].loop = false; // Hacer que el audio del avión se reproduzca en bucle
        startBtn.SetActive(false);
    }

    // Mostrar el menú de configuración en el menú principal
    public void BtnConfig()
    {
        mainMenu.SetActive(false);
        configMenuWindow.SetActive(true);
        configButton.SetActive(false);
    }

    // Mostrar menú de información
    public void BtnInformation()
    {
        mainMenuWindow.SetActive(false);
        configButton.SetActive(false);
        informationWindow.SetActive(true);
        playerController.minX = -35;
        playerController.maxX = 35;
        playerController.minY = -35;
        playerController.maxY = 28;
        playerController.minZ = 72.5f;
        recordBox.SetActive(false);
    }

    public void BtnChangeHowToPlay()
    {
        informationContent[0].SetActive(false); // Desactivar la pestaña de controles
        informationContent[1].SetActive(true); // Activar la pestaña de cómo jugar
    }

    public void BtnChangeControls()
    {
        informationContent[0].SetActive(true); // Desactivar la pestaña de controles
        informationContent[1].SetActive(false); // Activar la pestaña de cómo jugar
    }

    // Menú de configuración desde el menú de pausa en partida
    public void BtnPauseSettings()
    {
        pauseSection.SetActive(false);
        settingsSection.SetActive(true);
    }

    // Cambio de pestaña a la pestaña de sonido
    public void BtnChangeTabSound()
    {
        controlsWindow.SetActive(false);
        soundWindow.SetActive(true);
    }

    // Cambio de pestaña a la pestaña de controles
    public void BtnChangeTabControls()
    {
        controlsWindow.SetActive(true);
        if (PlayerPrefs.GetInt("inverseYAxis") == 1)
        {
            InverseYAxisToggle.SetIsOnWithoutNotify(false); // Activar el toggle de invertir eje Y si está activo
        }
        else
        {
            InverseYAxisToggle.SetIsOnWithoutNotify(true);
        }
        soundWindow.SetActive(false);
    }

    // Invertir el eje Y del movimiento del jugador
    public void InverseYAxisFunction()
    {
        if (PlayerPrefs.GetInt("inverseYAxis") == 1)
        {
            PlayerPrefs.SetInt("inverseYAxis", -1);
        }
        else
        {
            PlayerPrefs.SetInt("inverseYAxis", 1);
        }
        PlayerPrefs.Save();
    }

    // Cerrar ventana de configuración en el menú principal
    public void BtnBackToMainMenu()
    {
        configMenuWindow.SetActive(false);
        mainMenuWindow.SetActive(true);
        mainMenu.SetActive(true);
        configButton.SetActive(true);
        informationWindow.SetActive(false);
        recordBox.SetActive(true);
    }

    // Volver al menú de pausa desde la sección de configuración
    public void BtnBackToPause()
    {
        settingsSection.SetActive(false);
        pauseSection.SetActive(true);
    }

    // Pausar el juego
    public void PauseGame()
    {
        //Pausar el juego
        if (gameControler.gameState == GameControler.GameState.Play || (gameControler.gameState == GameControler.GameState.Standby && pauseWindow))
        {
            Time.timeScale = 0;                                         // Se para el tiempo del juego
            gameControler.gameState = GameControler.GameState.Pause;
            // Ocultar la UI de puntuación y tiempo jugado
            scoreObject.SetActive(false);
            timePlayedObject.SetActive(false);
            speedText.gameObject.SetActive(false);
            // Mostrar la ventana de pausa
            pauseWindow.SetActive(true);
            pauseSection.SetActive(true);
            foreach (GameObject crosshair in crosshires) 
            {
                crosshair.SetActive(false); // Desactivar el crosshair
            }
            // Pausar el audio del juego
            if (!playerController.takenOff)                  // Para evitar desincronización del sonido
            {
                audioController.masterAudioSource[1].Pause();// Pausar el audio del avión
            }
            audioController.masterAudioSource[2].Pause();    // Pausar el audio del avión
        }
        // Reanudar el juego
        else if (gameControler.gameState == GameControler.GameState.Pause)
        {
            Time.timeScale = 1;
            foreach(Camera camera in cameras)
            {
                if (camera.enabled) // Desactivar el crosshair
                {
                    crosshires[Array.IndexOf(cameras, camera)].SetActive(true); // Activar el crosshair de la cámara activa
                }
            }
            pauseWindow.SetActive(false);
            scoreObject.SetActive(true);
            timePlayedObject.SetActive(true);
            speedText.gameObject.SetActive(true);

            gameControler.gameState = GameControler.GameState.Play;
            if (gameControler.time == 0) // Pausa en el Standby
            {
                gameControler.gameState = GameControler.GameState.Standby;
            }
            else // Pausa en mitad de partida
            {
                gameControler.gameState = GameControler.GameState.Play;
            }
            // Ocultar la ventana de ajustes
            settingsSection.SetActive(false);

            if (!audioController.masterAudioSource[1].isPlaying)
            {
                audioController.masterAudioSource[1].Play();// Reanudar el audio del avión
            }
            if (playerController.shootStarted)              // Si el jugador estaba disparando, reanudar el sonido del disparo
            {
                audioController.masterAudioSource[2].Play();
            }
        }
    }

    // Ventana del Game Over
    public void GameOver()
    {
        Time.timeScale = 0;
        audioController.musicAudioSource.clip = audioController.audioClipsBackground[2]; // Cambiar el clip de música a Game Over
        audioController.musicAudioSource.Play(); // Reproducir la música de Game Over
        gameControler.gameState = GameControler.GameState.End;
        // Desactivar la UI de puntuación y tiempo jugado
        scoreObject.SetActive(false);
        timePlayedObject.SetActive(false);
        speedText.gameObject.SetActive(false);
        // Mostrar la ventana de Game Over
        gameOverWindow.SetActive(true);
        audioController.masterAudioSource[1].Pause(); // Pausar el audio del avión
        foreach(GameObject crosshire in crosshires){
            crosshire.SetActive(false); // Desactivar el crosshair
        }
        warningPullUp.enabled = false; // Desactivar la advertencia de "Pull Up"
        foreach (AudioSource audioSource in audioController.masterAudioSource)
        {
            audioSource.Pause(); // Pausar todos los audios maestros
        }

        // Comprobación de records puntuación
        for (int i = 1; i <= 10; i++)
        {
            // Actualización de record puntuación
            if (gameControler.score > PlayerPrefs.GetInt("highScore" + i))
            {
                // Desplazar los records hacia abajo
                for (int j = 10; j > i; j--)
                {
                    PlayerPrefs.SetInt("highScore" + j, PlayerPrefs.GetInt("highScore" + (j - 1)));
                }
                // Establecer el nuevo record
                PlayerPrefs.SetInt("highScore" + i, gameControler.score);
                break; // Salir del bucle una vez se haya actualizado el record
            }
        }

        // Comprobación de records tiempo
        for (int i = 1; i <= 10; i++)
        {
            if (gameControler.time > PlayerPrefs.GetFloat("highTime" + i))
            {
                // Desplazar los records hacia abajo
                for (int j = 10; j > i; j--)
                {
                    PlayerPrefs.SetFloat("highTime" + j, PlayerPrefs.GetFloat("highTime" + (j - 1)));
                }
                // Establecer el nuevo record
                PlayerPrefs.SetFloat("highTime" + i, gameControler.time);
                break; // Salir del bucle una vez se haya actualizado el record
            }
        }
        // Muestra del record actual y la puntuación final
        recordScoreText.text = PlayerPrefs.GetInt("highScore1").ToString() + "   " + TimeSpan.FromSeconds(PlayerPrefs.GetFloat("highTime1")).ToString(@"mm\:ss"); // Mostrar el record
        finishScoreText.text = gameControler.score.ToString() + "   " + ts.ToString(@"mm\:ss"); // Mostrar la puntuación final y el tiempo jugado
        PlayerPrefs.Save();
    }

    // Botón de reiniciar el juego
    public void BtnRestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }

    // Salir de la partida y volver al menú principal
    public void BtnExitFromGame()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
    }

    // Cerrar el programa
    public void BtnExit()
    {
        Application.Quit();
    }
}