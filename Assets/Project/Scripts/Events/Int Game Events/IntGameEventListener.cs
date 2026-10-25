using UnityEngine;
using UnityEngine.Events;

// Este script se encarga de escuchar los SO Events genéricos #//
public class IntGameEventListener : MonoBehaviour
{
    [SerializeField] protected IntGameEvent evt;// Evento a escuchar
    [SerializeField] UnityEvent<int> response;    // Respuesta a ese evento

    private void OnEnable()
    {
        evt?.Suscribe(this);
    }

    private void OnDisable()
    {
        evt?.Unuscribe(this);
    }

    /* Métodos */
    // Respuesta al evento
    public void OnEventRaised(int value) => response?.Invoke(value);
}