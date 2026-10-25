using System.Collections;
using UnityEngine;
public class RocketAction : MonoBehaviour
{
    [SerializeField] GameObject engine;                      // Motor del misil que se activa al lanzarlo
    [SerializeField] Transform missileParent;                // Transform del misil
    [SerializeField] GameObject explosionParticles;          // Explosión del misil al impactar
    MeshRenderer meshRenderer;                               // MeshRenderer del misil
    Rigidbody rb;                                            // Rigidbody
    CapsuleCollider missileCollider;                         // Collider

    Vector3 originalPosition;                                // Posición de aparición del misil
    Quaternion originalRotation;                             // Rotación de aparición del misil

    bool launchStarted = false;                               // Indica si el misil ha sido lanzado

    /* Audio */
    AudioSource audioSource;                                 // AudioSource del misil

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        rb = GetComponent<Rigidbody>();
        missileCollider = GetComponent<CapsuleCollider>();
        audioSource = GetComponent<AudioSource>();
        originalPosition = transform.localPosition;
        originalRotation = transform.rotation;
    }

    private void OnEnable()
    {
        meshRenderer.enabled = true;                         // Se muestra el misil al activarlo
        launchStarted = false;                               // Reinicia el estado del misil
    }

    public void StartLaunch()
    {
        launchStarted = true;                                // Se indica que el misil ha sido lanzado
        rb.isKinematic = false;                              // Se activan las físicas
        engine.SetActive(true);
        missileCollider.enabled = true;                      // Se activa el collider del misil para que empieze a funciionar las colisiones
        transform.SetParent(null);                           // Se quita el padre del misil para que no se mueva con el avión
        audioSource.Play();                                  // Reproduce el sonido de lanzamiento del misil
        rb.linearVelocity = transform.up * 100;              // Se le da movimiento
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (launchStarted && !collision.gameObject.CompareTag("Player"))
        {
            Instantiate(explosionParticles);
            audioSource.Play();
            if (collision.gameObject.CompareTag("Obstacle"))
            {
                collision.gameObject.SetActive(false);       // Desactiva el obstáculo con el que ha colisionado
            }

            StartCoroutine(ResetRocket());                   // Empieza el proceso de reinicio del misil
        }
    }

    /* Corrutinas */
    // Reiniciar el estado del misil
    IEnumerator ResetRocket()
    {
        missileCollider.enabled = false;                     // Se desactiva el collider para evitar futuras colisiones
        meshRenderer.enabled = false;                        // Se oculta el misil
        engine.SetActive(false);                             // Y se apaga el motor para el próximo lanzamiento
        rb.linearVelocity = Vector3.zero;
        rb.isKinematic = true;                               // Se deja estático
        yield return new WaitForSeconds(2);                  // Tiempo de espera para la explosión
        gameObject.transform.SetParent(missileParent);
        transform.localPosition = originalPosition;          // Se recoloca
        transform.rotation = originalRotation;
        yield return new WaitForSeconds(18);                 // Resto del tiempo de espera
        meshRenderer.enabled = true;                         // Se reactiva el MeshRenderer
        launchStarted = false;                               // Se indica que el misil no ha sido lanzado
    }
}