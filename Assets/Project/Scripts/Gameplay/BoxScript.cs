using UnityEngine;

public class BoxScript : MonoBehaviour
{
    /* Acceso a Scripts */
    Rigidbody rb;                                                   // Rigidbody

    /*Sistema de partículas*/
    [SerializeField] GameObject explosionParticles;                      // Partículas de explosión

    const int value = 1;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();                             // Obtiene el Rigidbody del objeto
    }

    private void OnEnable()
    {
        rb.isKinematic = false;
    }

    private void OnDisable()
    {
        rb.linearVelocity = Vector3.zero;
        rb.isKinematic = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")                   // Si colisiona con una bala
        {
            LevelManager.Instance.AddScore(value);
            Instantiate(explosionParticles, transform.position, Quaternion.identity);
            gameObject.SetActive(false);
        }
    }
}