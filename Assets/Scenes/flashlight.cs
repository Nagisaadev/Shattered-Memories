using UnityEngine;
using UnityEngine.Rendering.Universal;


public class LightController : MonoBehaviour
{
    public Light2D myLight;
    [SerializeField] public bool isLightOn = true;

    void Update()
    {
        // Si la touche d'action est enfonc�e
        if (Input.GetButtonDown("Jump"))
        {
            // Inverse l'�tat de la lumi�re
            isLightOn = !isLightOn;

            // Active ou d�sactive la lumi�re en fonction de l'�tat
            myLight.enabled = isLightOn;
        }

        if (isLightOn == true);
        {

            myLight.enabled = isLightOn;
        }
        if (isLightOn == false) ;
        {

            myLight.enabled = isLightOn;
        }

    }
}