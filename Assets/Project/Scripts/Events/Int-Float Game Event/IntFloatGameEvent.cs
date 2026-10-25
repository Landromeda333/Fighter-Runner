using System.Collections.Generic;
using UnityEngine;

//# Este script se encarga de lanzar SO Events genéricos #//
[CreateAssetMenu (menuName = "Events/Int Float Game Event")]
public class IntFloatGameEvent : ScriptableObject
{
    /* Game Event Listeners */
    readonly List<IntFloatGameEventListener> listeners = new();    // Clases suscritas al evento

    /* Métodos */
    // Suscripción al evento
    public void Suscribe(IntFloatGameEventListener listener) => listeners.Add(listener);

    // Desuscripción del evento
    public void Unuscribe(IntFloatGameEventListener listener) => listeners.Remove(listener);

    // Lanzamiento del evento
    public void Raise(int value, float value2)
    {
        var copy = new List<IntFloatGameEventListener>(listeners); // Copia para evitar conflictos mientras hay cambios en la lista de listeners
        foreach (IntFloatGameEventListener listener in copy)
        {
            listener.OnEventRaised(value, value2);                      // Lanzamiento del evento
        }
    }
}