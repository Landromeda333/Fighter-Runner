using System.Collections.Generic;
using UnityEngine;

//# Este script se encarga de lanzar SO Events genéricos #//
[CreateAssetMenu (menuName = "Events/String Game Event")]
public class StringGameEvent : ScriptableObject
{
    /* Game Event Listeners */
    readonly List<StringGameEventListener> listeners = new();    // Clases suscritas al evento

    /* Métodos */
    // Suscripción al evento
    public void Suscribe(StringGameEventListener listener) => listeners.Add(listener);

    // Desuscripción del evento
    public void Unuscribe(StringGameEventListener listener) => listeners.Remove(listener);

    // Lanzamiento del evento
    public void Raise(string value)
    {
        var copy = new List<StringGameEventListener>(listeners); // Copia para evitar conflictos mientras hay cambios en la lista de listeners
        foreach (StringGameEventListener listener in copy)
        {
            listener.OnEventRaised(value);                      // Lanzamiento del evento
        }
    }
}