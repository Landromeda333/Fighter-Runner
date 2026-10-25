using UnityEngine;
using UnityEngine.Events;

// Este script se encarga de escuchar los SO Events genéricos #//
public class GameEventListener : MonoBehaviour
{
    [SerializeField] protected GameEvent evt;// Evento a escuchar
    [SerializeField] UnityEvent response;    // Respuesta a ese evento

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
    public void OnEventRaised() => response?.Invoke();
}