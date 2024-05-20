using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjetInvisble : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public bool visible = false;
    public LightController flashlight;
    public float amplitude = 0.3f; // Amplitude du mouvement
    public float vitesse =1.5f; // Vitesse du mouvement
    private float positionVerticaleInitiale;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;

        positionVerticaleInitiale = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        float deplacementVertical = Mathf.Sin(Time.time * vitesse) * amplitude;

        // Déplacement de l'objet en gardant la position verticale initiale
        transform.position = new Vector3(transform.position.x, positionVerticaleInitiale + deplacementVertical, transform.position.z);
    

    }





    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Light2D")& flashlight.isLightOn == true)
        {
            spriteRenderer.enabled = true;
        }
        
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Light2D") & flashlight.isLightOn == true)
        {
            spriteRenderer.enabled = true;
        }
        else if (other.gameObject.CompareTag("Light2D") & flashlight.isLightOn == false)
        {
            spriteRenderer.enabled = false;
        }
    }


    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Light2D"))
        {


            spriteRenderer.enabled = false;

        }
    }





}
