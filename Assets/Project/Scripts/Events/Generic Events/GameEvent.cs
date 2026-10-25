using System.Collections.Generic;
using UnityEngine;

//# Este script se encarga de lanzar SO Events genéricos #//
[CreateAssetMenu (menuName = "Events/Game Event")]
public class GameEvent : ScriptableObject
{
    /* Game Event Listeners */
    readonly List<GameEventListener> listeners = new();    // Clases suscritas al evento

    /* Métodos */
    // Suscripción al evento
    public void Suscribe(GameEventListener listener) => listeners.Add(listener);

    // Desuscripción del evento
    public void Unuscribe(GameEventListener listener) => listeners.Remove(listener);

    // Lanzamiento del evento
    public void Raise()
    {
        var copy = new List<GameEventListener>(listeners); // Copia para evitar conflictos mientras hay cambios en la lista de listeners
        foreach (GameEventListener listener in copy)
        {
            listener.OnEventRaised();                      // Lanzamiento del evento
        }
    }
}