using UnityEngine;

public class BoxScript : MonoBehaviour
{
    /* Acceso a Scripts */
    [SerializeField] GameControler gameController;                  // Acceso al controlador del juego
    Rigidbody rb;                                                   // Rigidbody
    MeshRenderer meshRenderer;                                      // MeshRenderer
    BoxCollider boxCollider;                                        // Collider
    AudioSource audioSource;                                        // AudioSource

    /*Sistema de partículas*/
    [SerializeField] ParticleSystem explosion;                      // Partículas de explosión

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();                             // Obtiene el Rigidbody del objeto
        meshRenderer = GetComponent<MeshRenderer>();                // Obtiene el MeshRenderer del objeto
        boxCollider = GetComponent<BoxCollider>();                  // Obtiene el BoxCollider del objeto
        audioSource = GetComponent<AudioSource>();                // Obtiene el AudioSource del objeto
    }

    private void OnEnable()
    {
        rb.isKinematic = false;
        meshRenderer.enabled = true;
        boxCollider.enabled = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")                   // Si colisiona con una bala
        {
            audioSource.Play();                     // Reproduce una explosión
            gameController.score++;
            rb.isKinematic = true;
            meshRenderer.enabled = false;
            boxCollider.enabled = false;
            explosion.Play();                                       // Activa las partículas de explosión
        }
    }
}