using UnityEngine;

public class PlaneMovement : MonoBehaviour
{
    Plane planeScript;

    const float maneuverability = 20;                                // Manejo de la maniobrabilidad del jugador, se usa para controlar la velocidad de movimiento

    Vector3 pos;                                                      // Posición del jugador

    int yAxisDir;

    private void Awake()
    {
        InputsManager.Instance.movement = this;
        planeScript = GetComponent<Plane>();
    }

    private void Start()
    {
        yAxisDir = SettingsManager.Instance.data.inverseYAxis ? 1 : -1;
    }

    private void FixedUpdate()
    {
            transform.Translate(maneuverability * Time.deltaTime * new Vector3(InputsManager.Instance.moveInput.x, yAxisDir * InputsManager.Instance.moveInput.y, 0).normalized);           // Se le aplica la velocidad de movimiento
            transform.Rotate(0, 0, -InputsManager.Instance.rollInput * 200 * Time.deltaTime); // Movimiento de rotación del avión

            // Limitación de movimiento del jugador dentro de los límites establecidos
            pos = transform.position;
            pos.x = Mathf.Clamp(pos.x, planeScript.minX, planeScript.maxX);
            pos.y = Mathf.Clamp(pos.y, planeScript.minY, planeScript.maxY);
            pos.z = planeScript.minZ;
            transform.position = pos;
    }

    public void OnInverseYAxisChanged(bool isInversed)
    {
        yAxisDir = isInversed ? 1 : -1;
    }
}