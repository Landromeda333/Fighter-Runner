using UnityEngine;

public class CameraFollowPlane : MonoBehaviour
{
    [SerializeField] Transform planeTransform;                       // Transform del avión del jugador
    void LateUpdate() // Para que la cámara 2 no se gire con el avión y traspase el suelo se tiene que fijar la cámara
    {
        transform.position = planeTransform.position - planeTransform.forward * 9.03f + Vector3.up * 3.5f;
        transform.rotation = Quaternion.Euler(8f, 0f, 0f); // Rotación de la cámara fija
    }
}