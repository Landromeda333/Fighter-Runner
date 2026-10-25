using UnityEngine;

public class bullet : MonoBehaviour
{
    Rigidbody rb;                                   // Componente Rigidbody del proyectil

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        Invoke("Desactivate", 3);                   // Desactiva el proyectil después de un tiempo
        rb.linearVelocity = transform.forward * 120;// Se le da movimiento a la vala
    }
    
    void Desactivate()
    {
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Player")   // Si colisiona con algo se destruye
        {
            gameObject.SetActive(false);
        }
    }

    private void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }

}