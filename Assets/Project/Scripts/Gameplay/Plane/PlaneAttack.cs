using System.Collections;
using System.Linq;
using UnityEngine;

public class PlaneAttack : MonoBehaviour
{
    IUsable[] missilesCache;

    [SerializeField] GameObject muzzleFlash;                            // Referencia al objeto de destello del cañón
    [SerializeField] GameObject[] missiles = new GameObject[4];         // Referencia a los misiles del avión

    AudioSource source;

    [SerializeField] AudioClip initShootClip, shootingClip, stopShootingClip;

    Coroutine shootCoroutine;                                           // Coroutine para manejar el disparo

    bool shootStarted;                                           // Indica si el disparo ha comenzado

    private void Awake()
    {
        InputsManager.Instance.attack = this;
        source = GetComponent<AudioSource>();
    }

    private void Start()
    {
        missilesCache = missiles.Select(go => go.GetComponent<IUsable>()).Where(m => m != null).ToArray();
    }

    public void LaunchMissile()
    {
        var missile = missilesCache.FirstOrDefault(m => m.IsAvailable());
        missile?.Shoot();
    }

    public void FireHandle(float status)
    {
        if (status == 1)
        {
            if (shootCoroutine == null)
            {
                shootCoroutine = StartCoroutine(Fire());          // Para no tener problemas de superposición de corrutinas se le asigna una corrutina a la variable para que se pueda detener
            }
        }
        else if (shootCoroutine != null)
        {
            StopCoroutine(shootCoroutine);
            shootCoroutine = null;                            // Se vacía la corrutina para evitar problemas
            shootStarted = false;
            muzzleFlash.SetActive(false);                     // Se quita el flash del cañón por si se interrumpe el disparo para que no se quede puesto
        }
    }

    // Disparo del avión
    IEnumerator Fire()
    {
        if (!shootStarted)
        {
            yield return new WaitForSeconds(0.5f);                      // Tiempo inicial antes de poder disparar
            shootStarted = true;
        }
        while (true)                                                    // Se repite mientras se mantenga pulsado
        {
            BulletPooler.Instance.GetBullet(transform)?.SetActive(true);// Se intancia una bala del pooler
            muzzleFlash.SetActive(true);                                // Se activa el flash del cañón
            yield return new WaitForSeconds(0.05f);                     // Tiempo de espera para el efecto visual del flash
            muzzleFlash.SetActive(false);
            yield return new WaitForSeconds(0.05f);                     // Delay entre disparos
        }
    }

    public void OnPause()
    {
        source.Pause();
    }

    public void OnContinueGame()
    {
        if (shootStarted)              // Si el jugador estaba disparando, reanudar el sonido del disparo
        {
            source.Play();
        }
    }
}