using UnityEngine;

public class PlayerSafetyBehaviour : MonoBehaviour
{
    [SerializeField] GameEvent startWarningRequest, stopWarningRequest;

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.Instance.currentState == GameManager.Instance.InGame)
            startWarningRequest.Raise();
    }

    private void OnTriggerExit(Collider other) => stopWarningRequest.Raise();
}