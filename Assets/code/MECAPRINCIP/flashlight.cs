using UnityEngine;
using UnityEngine.Rendering.Universal;


public class LightController : MonoBehaviour
{
    public Light2D myLight;
    public GameObject[] objets = new GameObject[3];
    [SerializeField] public bool isLightOn = true;
    public bool visible =false;


    private void Start()
    {
    }




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
        if (isLightOn == false);
        {

            myLight.enabled = isLightOn;
        }

    }




    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("compteur"))
        {
            Debug.Log("Entered trigger with Compteur");
            visible = true;

        }
    }



    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("compteur"))
        {
            Debug.Log("Exited trigger with Compteur");
            visible = false;

        }
    }

}