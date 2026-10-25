using UnityEngine;

public class CamerasManager : MonoBehaviour
{
    [SerializeField] IntGameEvent onCameraSwitched;

    public Camera[] cameras;                    // Cámara del morro

    public void SwitchCamera(int index)
    {
        //En vez de hacer un for o un switch para activar/desactivar las cámaras (que es un tocho grande de código), simplemente se activan las cosas si el índice coincide
        cameras[0].enabled = (index == 1);
        cameras[1].enabled = (index == 2);
        cameras[2].enabled = (index == 3);

        cameras[0].GetComponent<AudioListener>().enabled = (index == 1);
        cameras[1].GetComponent<AudioListener>().enabled = (index == 2);
        cameras[2].GetComponent<AudioListener>().enabled = (index == 3);

        onCameraSwitched.Raise(index);
    }

}