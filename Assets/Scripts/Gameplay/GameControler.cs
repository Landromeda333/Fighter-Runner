using UnityEngine;
public class GameControler : MonoBehaviour
{
    /* Scripts */
    [SerializeField] PlayerController playerController;             // Referencia al controlador del jugador

    public int score;                                               // Puntuación del jugador
    public float time;                                              // Tiempo jugado
    public enum GameState                                           // Estados del juego
    {
        Standby,
        Play,
        Pause,
        End
    }
    public GameState gameState = GameState.Standby;

    private void Update()
    {
        if (gameState == GameState.Play)                            // Si el juego está en estado de juego
        {
            time += Time.deltaTime;                                 // Transcurre el tiempo
            if(playerController.speed < 200)                        // Si ya ha despegado se mueve más rápido
            {
                playerController.speed += 1.5f * Time.deltaTime;
            }
        }
    }
}