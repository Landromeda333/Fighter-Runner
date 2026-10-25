using UnityEngine;

public class ScrollParallax : MonoBehaviour
{
    /* GameObjects */
    [SerializeField] GameObject[] obstacles = new GameObject[4];         // Obstáculos del juego
    [SerializeField] GameObject boxItem;                                 // Objeto de puntuación que en este caso será una caja

    int index = 0;                                                       // Índice para elegir obstáculos
    int probability;                                                     // Probabilidad para una cantidad de obstáculos progresiva

    void FixedUpdate()
    {
        if (Plane.Instance.speed > 0)                                  // Si se ha iniciado el movimiento del jugador
        {
            transform.Translate(Vector3.back * Plane.Instance.speed * Time.deltaTime); // Movimiento de las partes del escenario
            if (transform.position.z < -100)                              // Si ha llegao al límite de reciclaje
            {
                probability = Random.Range(1, 11);                       // Probabilidad de aparición de obstáculos
                if (index<obstacles.Length)
                {
                    obstacles[index].SetActive(false);                   // Desactiva el obstáculo actual
                }
                boxItem.SetActive(false);                                // Desactiva el objeto de puntuación
                boxItem.transform.localPosition = new Vector3(Random.Range(-20,20),50, Random.Range(-21, 21)); // Reubica el objeto de puntuación en una posición aleatoria
                if (probability <= (Plane.Instance.speed/20))          // Se usa la velocidad del jugador como medidor de progreso para la aparición de objetos
                {
                    index = Random.Range(0, 4);                          // Se elige un obstáculo
                    obstacles[index].SetActive(true);                    // Activa el obstáculo elegido
                }
                if (Random.Range(1,6) == 1 && Plane.Instance.takenOff) // Lo mismo con las cajas
                {
                    boxItem.SetActive(true);
                }
                transform.position += Vector3.forward * 380;             // Se mueve la pieza del escenario hacia adelante para reciclarla
            }
        }
    }
}