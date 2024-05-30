using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering.Universal;

public class InterrupteurCollision1 : MonoBehaviour
{
    public Slider slider;
    public GameObject sliderObject;
    public Light2D light2D;
    public GameObject fond;
    public PlayerController playerController;

    private bool isInCollision = false;

    void Start()
    {
        // Désactiver les éléments d'interface utilisateur au démarrage
        fond.SetActive(false);
        sliderObject.SetActive(false);
        light2D.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("joueur"))
        {
            // Activer les éléments d'interface utilisateur lorsque le joueur entre en collision
            isInCollision = true;

        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("joueur"))
        {
            // Désactiver les éléments d'interface utilisateur lorsque le joueur quitte la collision
            isInCollision = false;

        }
    }
    void Update()
    {

        if (isInCollision)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                fond.SetActive(true);
                sliderObject.SetActive(true);
                playerController.peutpasbouger = true;

            }
            if (Input.GetButtonDown("Cancel"))
            {
                // Désactiver tous les éléments d'interface utilisateur lorsque le joueur appuie sur Cancel

                fond.SetActive(false);
                sliderObject.SetActive(false);
                playerController.peutpasbouger = false;
            }

        }

        if (isInCollision && slider.value == 1 && !light2D.enabled)
        {

            // Activer la lumière lorsque le joueur appuie sur le bouton Fire1
            light2D.enabled = true;

            StartCoroutine(TurnOffLightAfterDelay(5.0f));

        }

        IEnumerator TurnOffLightAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            // Éteindre la lumière après un délai
            light2D.enabled = false;
            slider.value = 0;
        }
    }
}

