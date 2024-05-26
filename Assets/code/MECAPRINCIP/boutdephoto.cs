using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boutdephoto : MonoBehaviour
{
    public float amplitude = 0.3f; // Amplitude du mouvement
    public float vitesse = 1.5f; // Vitesse du mouvement
    private float positionVerticaleInitiale;
    private bool photo1=false;
    private bool photo2 = false;
    private bool photo3 = false;
    private bool colisionphoto1 = false;
    private bool colisionphoto2 = false;
    private bool colisionphoto3 = false;
    void Start()
    {
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
        if (other.gameObject.CompareTag("joueur")&&colisionphoto1==true)
        {
            photo1 = true;

        }
        if (other.gameObject.CompareTag("joueur") && colisionphoto2 == true)
        {
            photo2 = true;

        }
        if (other.gameObject.CompareTag("joueur") && colisionphoto3 == true)
        {
            photo3 = true;

        }
    }
}
