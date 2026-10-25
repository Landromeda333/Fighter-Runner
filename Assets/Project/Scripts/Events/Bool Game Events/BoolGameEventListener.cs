using UnityEngine;
using UnityEngine.Events;

// Este script se encarga de escuchar los SO Events genéricos #//
public class BoolGameEventListener : MonoBehaviour
{
    [SerializeField] protected BoolGameEvent evt;// Evento a escuchar
    [SerializeField] UnityEvent<bool> response;    // Respuesta a ese evento

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
    public void OnEventRaised(bool value) => response?.Invoke(value);
}