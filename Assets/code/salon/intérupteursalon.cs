using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.Rendering.Universal; // Assurez-vous d'importer ce namespace si vous utilisez Light2D
using UnityEngine.Rendering.Universal;

public class interrupteur : MonoBehaviour
{
    public Slider[] sliders;
    public PlayerController playerController;
    public GameObject fond;
    public GameObject[] slidersobj;
    public Light2D[] lights; // Ajouté pour les lumières

    // Start is called before the first frame update
    void Start()
    {
        fond.SetActive(false);
        for (int i = 0; i < slidersobj.Length; i++)
        {
            slidersobj[i].SetActive(false);
        }

        // Désactiver toutes les lumières au début
        foreach (var light in lights)
        {
            light.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Vérifier chaque collision et activer les sliders et lumières en conséquence
        CheckCollisionAndActivate(0, playerController.interupteursaloncolision1);
        CheckCollisionAndActivate(1, playerController.interupteursaloncolision2);
        CheckCollisionAndActivate(2, playerController.interupteursaloncolision3);
        CheckCollisionAndActivate(3, playerController.interupteursaloncolision4);
    }

    // Méthode pour vérifier la collision et activer les sliders et lumières
    void CheckCollisionAndActivate(int index, bool isColliding)
    {
        if (isColliding)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                fond.SetActive(true);
                slidersobj[index].SetActive(true);
                playerController.peutpasbouger = true;
            }

            if (Input.GetButtonDown("Cancel"))
            {
                fond.SetActive(false);
                slidersobj[index].SetActive(false);
                playerController.peutpasbouger = false;
            }

            // Vérifier la valeur du slider et activer la lumière correspondante
            if (sliders[index].value == 1 && !lights[index].enabled)
            {
                lights[index].enabled = true;
                StartCoroutine(TurnOffLightAfterDelay(index, 5f)); // Lancer la coroutine pour éteindre la lumière après 5 secondes
            }
        }
    }

    // Coroutine pour éteindre la lumière après un délai
    IEnumerator TurnOffLightAfterDelay(int index, float delay)
    {
        yield return new WaitForSeconds(delay);
        lights[index].enabled = false;
        sliders[index].value = 0;
    }
}
