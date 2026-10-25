using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPooler : MonoBehaviour
{
    public static BulletPooler Instance;

    [SerializeField] GameObject bulletPrefab;                                                    // Bala
    List<GameObject> bulletPool = new List<GameObject>();                                        // Como no sé el tamaño exacto que va a tener durante la partida, uso una lista para poder modificar el tamaño

    const int bulletSize = 50;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        StartCoroutine(BulletInstantiator());
    }

    public GameObject GetBullet(Transform planeTransform)
    {
        foreach (GameObject bullet in bulletPool)
        {
            if (!bullet.activeSelf)
            {
                bullet.transform.position = planeTransform.position + planeTransform.forward * 2;// Resetea la posición de la bala
                bullet.transform.rotation = planeTransform.rotation;                             // Resetea la rotación de la bala
                return bullet;                                                                   // Para que no cree una nueva bala, devuelvo la bala para que sea disparada en PlayerController
            }
        }
        return null;
    }

    /**/
    IEnumerator BulletInstantiator()
    {
        for (int i = 0; i < bulletSize; i++)                                                     // Crea las balas
        {
            GameObject bullet = Instantiate(bulletPrefab);                                       // Se posiciona en el morro del avión
            bullet.SetActive(false);
            bulletPool.Add(bullet);                                                              // Añade la bala al pool
            yield return new WaitForEndOfFrame();
        }
    }
}