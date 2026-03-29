using UnityEngine;

public class RunwayScript : MonoBehaviour
{
    private void FixedUpdate()
    {
        if (transform.position.z < -90)                              // Si ha llegao al límite de reciclaje
        {
            Destroy(gameObject);                                     // Destruye el runway
        }
    }
}