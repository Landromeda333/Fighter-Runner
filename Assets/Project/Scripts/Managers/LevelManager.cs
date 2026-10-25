using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] IntGameEvent onScoreChanged, onTimeChanged;//
    [SerializeField] IntFloatGameEvent gameOver;                //
    public static LevelManager Instance;                        //

    ParticleSystem speedParticles;                              // Partículas de velocidad

    float timePlayed;                  //

    [HideInInspector] public int score;                         // Puntuación del jugador
    [HideInInspector] public int lastWholeSecond = -1;                                   //

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        speedParticles = GetComponent<ParticleSystem>();
    }

    public void OnStartGame()
    {
        timePlayed = Time.time;
        enabled = true;
    }

    private void Update()
    {
        int wholeSecond = Mathf.FloorToInt(Time.time - timePlayed);
        if (wholeSecond != lastWholeSecond)
        {
            lastWholeSecond = wholeSecond;
            onTimeChanged.Raise(wholeSecond);
        }
    }

    public void OnPause()
    {
        Time.timeScale = 0;                                     // Se para el tiempo del juego
    }

    public void OnContinueGame()
    {
        Time.timeScale = 1;
    }

    public void OnEndGame()
    {
        enabled = false;
    }

    public void OnSpeedChanged()
    {
        var main = speedParticles.main;
        main.startSpeed = Plane.Instance.speed;
        main.maxParticles = (int)Plane.Instance.speed;
    }

    public void AddScore(int ammount)
    {
        score += ammount;
        onScoreChanged.Raise(score);
    }

    public void PlayerCrashHandle()
    {
        gameOver.Raise(score, lastWholeSecond);
    }
}