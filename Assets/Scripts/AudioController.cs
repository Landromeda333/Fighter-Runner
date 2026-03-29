using UnityEngine;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    /* Scripts */
    [SerializeField] PlayerController playerController;                      // Referencia al controlador del jugador
    
    /* Textos */
    [SerializeField] Text masterVolumeText;                                  // Texto volumen general
    [SerializeField] Text musicVolumeText;                                   // Texto volumen música

    /* Sliders */
    [SerializeField] Slider masterVolumeSlider;                              // Slider de volumen general
    [SerializeField] Slider musicVolumeSlider;                               // Slider de volumen música

    /* Sprites */
    [SerializeField] Sprite soundOnSprite;                                   // Sprite sonido encendido
    [SerializeField] Sprite muteSprite;                                      // Sprite mute

    /* Images */
    [SerializeField] Image masterMuteButtonImage;                            // Imagen botón sonido general
    [SerializeField] Image musicMuteButtonImage;                             // Imagen botón música

    /* AudioSource */
    public AudioSource[] masterAudioSource = new AudioSource[14];            // Array con AudioSources que no reproducen música (general(radio)[0], motores[1], disparo[2] y cajas[3-8])
    public AudioSource musicAudioSource;                                     // AudioSource que reproduce música

    /*AudioClips*/
    public AudioClip[] audioClipsBackground = new AudioClip[3];              // Array con AudioClips de música de fondo (menú principal[0], partida[1], gameOver[2])
    public AudioClip[] audioClipsAircraft = new AudioClip[3];                // Array con AudioClips de aviones (motor estático[0], encendido motor[1], postquemadores[2])
    public AudioClip[] audioClipsShooting = new AudioClip[3];                // Array con AudioClips de disparo (inicio tambor[0], disparo continuo[1], enfriamiento disparo[2])
    
    private void Awake()
    {
        foreach (AudioSource audioSource in masterAudioSource)               // Se inicializan los AudioSources del array
        {
            if (audioSource)                                                 // Comprueba que haya un AudioSource
            {
                audioSource.volume = PlayerPrefs.GetFloat("masterSound");    // No se reproducen al iniciar
            }
        }
        masterVolumeSlider.value = masterAudioSource[0].volume;              // Se iguala el slider al volumen actual
        masterVolumeText.text = (masterVolumeSlider.value * 100).ToString("0");// Se pone el texto del volumen general
        masterAudioSource[1].clip = audioClipsAircraft[0];                   // Clip motor estático
        musicAudioSource.volume = PlayerPrefs.GetFloat("musicSound");        // Se carga el volumen de música desde PlayerPrefs, por defecto 0.5f
        musicVolumeSlider.value = musicAudioSource.volume;                   // Se iguala el slider al volumen actual
        musicVolumeText.text = (musicVolumeSlider.value * 100).ToString("0");

        // Comprobación del estado del mute del sonido general
        if (PlayerPrefs.GetInt("masterMuted") == 0)                          // Si no está muteado
        {
            foreach (AudioSource audioSource in masterAudioSource)           // Se actualiza el volumen de todos los AudioSources del array
            {
                if (audioSource)                                             // Comprueba que haya un AudioSource
                {
                    audioSource.mute = false;                                // Se iguala el volumen del slider
                }
            }
        }
        else                                                                 // Si está muteado
        {
            foreach (AudioSource audioSource in masterAudioSource)           // Se actualiza el volumen de todos los AudioSources del array
            {
                if (audioSource)                                             // Comprueba que haya un AudioSource
                {
                    audioSource.mute = true;                                 // Se iguala el volumen del slider
                }
            }
            masterMuteButtonImage.sprite = muteSprite;                       // Se cambia el sprite del botón
            masterVolumeSlider.value = 0;                                    // Se pone el slider a 0
            masterVolumeText.text = "0";                                     // Se pone el texto a 0
            musicVolumeSlider.interactable = false;                          // Se desactiva el slider de música
            PlayerPrefs.SetInt("musicMuted", 1);
            PlayerPrefs.Save();                                              // Se guarda el estado del mute en PlayerPrefs
        }

        //Comprobación del estado de mute de la música
        if (PlayerPrefs.GetInt("musicMuted") == 0)                           // Si no está muteado
        {
            musicAudioSource.mute = false;
            musicMuteButtonImage.sprite = soundOnSprite;                     // Se cambia el sprite del botón de música
            musicVolumeSlider.interactable = true;                           // Se activa el slider de música
            musicVolumeSlider.value = musicAudioSource.volume;               // Se iguala el slider al volumen actual
            musicVolumeText.text = (musicVolumeSlider.value * 100).ToString("0");
        }
        else                                                                 // Si está muteado
        {
            musicAudioSource.mute = true;
            musicMuteButtonImage.sprite = muteSprite;                        // Se cambia el sprite del botón de música
            musicVolumeSlider.value = 0;
            musicVolumeText.text = "0";                                      // Se pone el texto a 0
        }
    }

    void Update()
    {
        // Si el volumen general se modifica
        if (!masterAudioSource[0].mute && masterAudioSource[0].volume != masterVolumeSlider.value)
        {
            masterAudioSource[0].volume = masterVolumeSlider.value;          // Se iguala el volumen del slider
            masterVolumeText.text = (masterVolumeSlider.value * 100).ToString("0"); // Modifica el texto
            if (musicAudioSource.volume > masterAudioSource[0].volume)       // Se reduce el volumen de la música junto al general
            {
                musicAudioSource.volume = masterAudioSource[0].volume;       // Se igualan valores
                musicVolumeSlider.value = musicAudioSource.volume;
                musicVolumeText.text = (musicVolumeSlider.value * 100).ToString("0");
            }
            foreach (AudioSource audioSource in masterAudioSource)           // Se actualiza el volumen de todos los AudioSources del array
            {
                if (audioSource)                                             // Comprueba que haya un AudioSource
                {
                    audioSource.volume = masterAudioSource[0].volume;        // Se iguala el volumen del slider
                }
            }
            PlayerPrefs.SetFloat("masterSound", masterAudioSource[0].volume);// Se actualiza el volumen en PlayerPrefs
            PlayerPrefs.Save();
        }

        // Lo mismo para la música
        if (!musicAudioSource.mute && musicAudioSource.volume != musicVolumeSlider.value && !masterAudioSource[0].mute)
        {
            musicAudioSource.volume = musicVolumeSlider.value;
            musicVolumeText.text = (musicVolumeSlider.value * 100).ToString("0");
            PlayerPrefs.SetFloat("musicSound", musicAudioSource.volume);
            PlayerPrefs.Save();
        }

        // Para evitar que la música tenga un volumen mayor que el del sonido general
        if (musicVolumeSlider.value >= masterVolumeSlider.value)
        {
            musicVolumeSlider.value = masterVolumeSlider.value; // Si el volumen de la música es mayor que el del sonido general, se iguala el slider de sonido general
            musicVolumeText.text = (musicVolumeSlider.value * 100).ToString("0");
        }

        // Quitar el mute con el slider
        if (masterAudioSource[0].mute && masterVolumeSlider.value > 0)
        {
            foreach (AudioSource audioSource in masterAudioSource)          // Se quita el mute de todos los AudioSources del array
            {
                if (audioSource)
                {
                    audioSource.mute = false;                                   // Se quita el mute

                }
            }
            masterMuteButtonImage.sprite = soundOnSprite;                   // Se cambia el sprite del botón
            musicVolumeSlider.interactable = true;                          // Se activa el slider de música
            musicMuteButtonImage.sprite = soundOnSprite;                    // Se cambia el sprite del botón de música
        }

        // Lo mismo para la música
        if (musicAudioSource.mute && musicVolumeSlider.value > 0)
        {
            musicAudioSource.mute = false;
            musicMuteButtonImage.sprite = soundOnSprite;
        }

        // Si no está sonando y el clip es el de inicio del tambor y se pulsa el botón de disparo, se cambia al clip de disparo continuo
        if (!masterAudioSource[2].isPlaying && masterAudioSource[2].clip == audioClipsShooting[0] && InputsManager.Instance.FireInput == 1)
        {
            masterAudioSource[2].clip = audioClipsShooting[1];
            masterAudioSource[2].loop = true;
            masterAudioSource[2].Play();
        }
        // Si se está reproduciendo algo pero no es el final de disparo y se suelta el botón de disparo suena el enfriamiento de disparo
        else if (masterAudioSource[2].isPlaying && masterAudioSource[2].clip != audioClipsShooting[2] && InputsManager.Instance.FireInput == 0)
        {
            masterAudioSource[2].clip = audioClipsShooting[2];
            masterAudioSource[2].loop = false;
            masterAudioSource[2].Play();
        }
        // Si el clip que hay es el de enfriamiento y se pulsa el botón de disparo significa que se empieza el disparo
        else if (masterAudioSource[2].clip == audioClipsShooting[2] && InputsManager.Instance.FireInput == 1)
        {
            masterAudioSource[2].clip = audioClipsShooting[0];
            masterAudioSource[2].Play();
        }
    }

    public void MuteMasterSound()
    {

        if (!masterAudioSource[0].mute)                             // Si ya está muteado
        {
            masterMuteButtonImage.sprite = muteSprite;              // Se cambia el sprite del botón
            musicMuteButtonImage.sprite = muteSprite;               // Se cambia el sprite del botón de música
            foreach (AudioSource audioSource in masterAudioSource)  // Se pone el mute a todos los AudioSources del array
            {
                if (audioSource)
                {
                    audioSource.mute = true;                        // Se pone el mute
                }
            }
            PlayerPrefs.SetInt("masterMuted", 1);                   // Se guarda el estado del mute en PlayerPrefs
            masterVolumeSlider.value = 0;                           // Se pone el slider a 0
            masterVolumeText.text = "0";                            // Se pone el texto a 0
            musicAudioSource.volume = 0;                            // Lo mismo para la música
            musicVolumeText.text = "0";
            musicVolumeSlider.interactable = false;                 // Se desactiva el slider de música
        }
        else   // Si estaba muteado
        {
            masterMuteButtonImage.sprite = soundOnSprite;           // Se cambia el sprite del botón
            masterVolumeSlider.value = masterAudioSource[0].volume; // Se iguala el slider al volumen actual
            foreach (AudioSource audioSource in masterAudioSource)  // Se quita el mute de todos los AudioSources del array
            {
                if (audioSource)
                {
                    audioSource.mute = false;                       // Se quita el mute
                }
            }
            PlayerPrefs.SetInt("masterMuted", 0);                   // Se guarda el estado del mute en PlayerPrefs
            musicVolumeSlider.interactable = true;                  // Se activa el slider de música
            if (!musicAudioSource.mute)                             // Si no estaba muteado
            {
                musicVolumeSlider.value = musicAudioSource.volume;  // Se hace lo mismo
                musicMuteButtonImage.sprite = soundOnSprite;
            }
            masterVolumeText.text = (masterVolumeSlider.value * 100).ToString("0");
            musicVolumeText.text = (musicVolumeSlider.value * 100).ToString("0");
        }
        PlayerPrefs.Save();                                         // Se guardan los cambios en PlayerPrefs
    }
    public void MuteMusic()
    {
        // Si la música ya está muteada y no está muteado el sonido general
        if (musicAudioSource.mute && !masterAudioSource[0].mute)
        {
            musicMuteButtonImage.sprite = soundOnSprite;            // Se cambia el sprite del botón
            musicVolumeSlider.value = musicAudioSource.volume;      // Se iguala el slider al volumen actual
            musicAudioSource.mute = false;                          // Se quita el mute
            musicVolumeText.text = (musicVolumeSlider.value * 100).ToString("0");
            PlayerPrefs.SetInt("musicMuted", 0);                    // Se guarda el estado del mute en PlayerPrefs
        }
        else
        {
            musicMuteButtonImage.sprite = muteSprite;               // Se cambia el sprite del botón
            musicAudioSource.mute = true;                           // Se pone el mute
            musicVolumeSlider.value = 0;                            // Se pone el slider a 0
            musicVolumeText.text = "0";                             // Se pone el texto a 0
            PlayerPrefs.SetInt("musicMuted", 1);                    // Se guarda el estado del mute en PlayerPrefs
        }
        PlayerPrefs.Save();                                         // Se guardan los cambios en PlayerPrefs
    }
}