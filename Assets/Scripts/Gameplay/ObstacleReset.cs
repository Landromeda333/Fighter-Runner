using UnityEngine;

public class ObstacleReset : MonoBehaviour
{
    [SerializeField] GameObject[] components; // Componentes del obstáculo
    private void OnDisable() // Como los misiles pueden romper los obstáculos, es necesario restaurarlos
    {
        foreach (var component in components)
        {
            if (!component.activeSelf) { 
                component.SetActive(true);
            }
        }
    }
}