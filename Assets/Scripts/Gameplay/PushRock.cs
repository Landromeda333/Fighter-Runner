using UnityEngine;

public class PushRock : MonoBehaviour
{
    /* Scripts */
    [SerializeField] PlayerController playerController; // Referencia al controlador del jugador

    Rigidbody rb;                                       // Rigidbody
    Quaternion originalRotation;                        // Rotación original
    Vector3 originalPosition;                           // Posición original

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        originalPosition = transform.localPosition;
        originalRotation = transform.rotation;
    }

    private void FixedUpdate()
    {
        if (transform.parent.position.z <= playerController.speed*3.5 && rb.isKinematic) // Para que la roca se lance en el momento adecuado
        {
            rb.isKinematic = false;
            rb.AddForce(transform.right * 150, ForceMode.Impulse);// Se empuja la roca
        }
    }

    private void OnEnable()
    {
        if (!rb.isKinematic)                          // Se pone estática
        {
            rb.isKinematic = true;                   // Se pone estática
            transform.rotation = originalRotation;          // Y se resetea
            transform.localPosition = originalPosition;
        }
    }

    private void OnDisable()
    {
        rb.isKinematic = true;                          // Se pone estática
        transform.rotation = originalRotation;          // Y se resetea
        transform.localPosition = originalPosition;
    }
}