using System.Collections.Generic;
using UnityEngine;

//# Este script se encarga de lanzar SO Events genéricos #//
[CreateAssetMenu (menuName = "Events/Int Game Event")]
public class IntGameEvent : ScriptableObject
{
    /* Game Event Listeners */
    readonly List<IntGameEventListener> listeners = new();    // Clases suscritas al evento

    /* Métodos */
    // Suscripción al evento
    public void Suscribe(IntGameEventListener listener) => listeners.Add(listener);

    // Desuscripción del evento
    public void Unuscribe(IntGameEventListener listener) => listeners.Remove(listener);

    // Lanzamiento del evento
    public void Raise(int value)
    {
        var copy = new List<IntGameEventListener>(listeners); // Copia para evitar conflictos mientras hay cambios en la lista de listeners
        foreach (IntGameEventListener listener in copy)
        {
            listener.OnEventRaised(value);                      // Lanzamiento del evento
        }
    }
}