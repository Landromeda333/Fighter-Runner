using UnityEngine;
using UnityEngine.Events;

// Este script se encarga de escuchar los SO Events genéricos #//
public class IntFloatGameEventListener : MonoBehaviour
{
    [SerializeField] protected IntFloatGameEvent evt;// Evento a escuchar
    [SerializeField] UnityEvent<int,float> response;    // Respuesta a ese evento

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
    public void OnEventRaised(int value, float value2) => response?.Invoke(value, value2);
}