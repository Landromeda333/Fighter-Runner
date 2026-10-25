using System;
using UnityEngine;
using UnityEngine.Events;

// Este script se encarga de escuchar los SO Events genéricos #//
public class StringGameEventListener : MonoBehaviour
{
    [SerializeField] protected StringGameEvent evt;// Evento a escuchar
    [SerializeField] UnityEvent<string> response;    // Respuesta a ese evento

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
    public void OnEventRaised(string value) => response?.Invoke(value);
}