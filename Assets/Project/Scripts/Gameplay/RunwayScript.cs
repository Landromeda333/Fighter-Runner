using UnityEngine;

public class RunwayScript : MonoBehaviour
{
    private void OnBecameInvisible()
    {
        Destroy(gameObject);                                     // Destruye el runway
    }
}