using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RatingBoxScript : MonoBehaviour
{
    /* Accesos a Scripts */
    [SerializeField] PlayerPrefsInfinityRunner playerPrefsInfinityRunner;          // Acceso a los arrays de los records
    
    /* Sprites */
    [SerializeField] Sprite rightArrow;                                            // Sprite flecha hacia la derecha
    [SerializeField] Sprite leftArrow;                                             // Sprite flecha hacia la izquierda
    [SerializeField] Sprite rightArrowPressed;
    [SerializeField] Sprite leftArrowPressed;

    [SerializeField] Button ratingButton;
    
    /* Textos */
    [SerializeField] TextMeshProUGUI[] scoreRecordsText = new TextMeshProUGUI[10]; // Textos de las mejores puntuaciones
    [SerializeField] TextMeshProUGUI[] timeRecordsText = new TextMeshProUGUI[10];  // Textos de los mejores tiempos

    Vector2 hiddenBoxPosition = new Vector2(-396, 0);                              // Posición objetivo al ocultar el rating box
    Vector2 showBoxPosition = new Vector2(-217, 0);                                // Posición objetivo al mostrar el rating box

    bool showRating = false;                                                       // Estado del rating box (mostrado u oculto)

    private void OnEnable()
    {
        // Actualización de los textos de las mejores puntuaciones y tiempos
        for (int i = 0; i < scoreRecordsText.Length; i++)
        {
            scoreRecordsText[i].text = PlayerPrefs.GetInt("highScore" +(i+1)).ToString();
            timeRecordsText[i].text = TimeSpan.FromSeconds(PlayerPrefs.GetFloat("highTime" + (i+1))).ToString(@"mm\:ss");
        }
    }

    private void Update()
    {
        // Actualización de la posición del rating box según su estado
        if (showRating && transform.localPosition.x != showBoxPosition.x)
        {
            transform.localPosition = Vector2.MoveTowards(transform.localPosition, showBoxPosition, 0.5f);
        }
        else if (!showRating && transform.localPosition.x != hiddenBoxPosition.x)
        {
            transform.localPosition = Vector2.MoveTowards(transform.localPosition, hiddenBoxPosition, 0.5f);
        }
    }

    /* Métodos */
    // Movimiento del rating box al hacer click en el botón
    public void ShowRatingBox()
    {
        if (!showRating) // Abriendo el rating box
        {
            // Cambio del Sprite del botón
            ratingButton.GetComponent<Image>().sprite = leftArrow;
            SpriteState spriteState = ratingButton.spriteState;
            spriteState.pressedSprite = leftArrowPressed;
            ratingButton.spriteState = spriteState;
            showRating = true;                                                     // Actualiza el estado de la tabla
        }
        else // Cerrando el rating box
        {
            // Cambio del Sprite del botón
            ratingButton.GetComponent<Image>().sprite = rightArrow;
            SpriteState spriteState = ratingButton.spriteState;
            spriteState.pressedSprite = rightArrowPressed;
            ratingButton.spriteState = spriteState;
            showRating = false;                                                    // Actualiza el estado de la tabla
        }
    }
}