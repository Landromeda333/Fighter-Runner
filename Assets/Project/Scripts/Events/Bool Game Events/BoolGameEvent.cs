using System.Collections.Generic;
using UnityEngine;

//# Este script se encarga de lanzar SO Events genéricos #//
[CreateAssetMenu (menuName = "Events/Bool Game Event")]
public class BoolGameEvent : ScriptableObject
{
    /* Game Event Listeners */
    readonly List<BoolGameEventListener> listeners = new();    // Clases suscritas al evento

    /* Métodos */
    // Suscripción al evento
    public void Suscribe(BoolGameEventListener listener) => listeners.Add(listener);

    // Desuscripción del evento
    public void Unuscribe(BoolGameEventListener listener) => listeners.Remove(listener);

    // Lanzamiento del evento
    public void Raise(bool value)
    {
        var copy = new List<BoolGameEventListener>(listeners); // Copia para evitar conflictos mientras hay cambios en la lista de listeners
        foreach (BoolGameEventListener listener in copy)
        {
            listener.OnEventRaised(value);                      // Lanzamiento del evento
        }
    }
}