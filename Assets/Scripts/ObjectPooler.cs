using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;               // Bala
    List<GameObject> bulletPool = new List<GameObject>();   // Como no sé el tamaño exacto que va a tener durante la partida, uso una lista para poder modificar el tamaño
    Quaternion originalRotation = new Quaternion(0,0,0,0);  // Guarda la rotación de aparición
    
    void Start()
    {
        for (int i = 0; i < 20; i++)                        // Crea las balas
        {
            GameObject bullet = Instantiate(bulletPrefab);// Se posiciona en el morro del avión
            bullet.SetActive(false);
            bulletPool.Add(bullet);                         // Añade la bala al pool
        }
    }

    public GameObject GetBullet()
    {
        foreach (GameObject bullet in bulletPool)
        {
            if (!bullet.activeSelf)
            {
                bullet.transform.position = transform.position + transform.forward*2; // Resetea la posición de la bala
                bullet.transform.rotation = originalRotation; // Resetea la rotación de la bala
                return bullet;                                // Para que no cree una nueva bala, devuelvo la bala para que sea disparada en PlayerController
            }
        }
        // Al ser una ametralladora puede que se necesiten más balas de las que hay en el pool, así que se crean nuevas valas
        GameObject newBullet = Instantiate(bulletPrefab);
        newBullet.SetActive(false);
        bulletPool.Add(newBullet);
        return newBullet;
    }
}