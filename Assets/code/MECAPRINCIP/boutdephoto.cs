using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class boutdephoto : MonoBehaviour
{
    public float amplitude = 0.3f; // Amplitude du mouvement
    public float vitesse = 1.5f; // Vitesse du mouvement
    private float positionVerticaleInitiale;
    public bool photo = false;
    public bool colisionphoto = false;
    public GameObject petitephoto;
    public delegate void OnPhotoCollected();
    public static event OnPhotoCollected PhotoCollectedEvent;


    void Start()
    {
        positionVerticaleInitiale = transform.position.y;
        
    }

    // Update is called once per frame
    void Update()
    {
        float deplacementVertical = Mathf.Sin(Time.time * vitesse) * amplitude;

        // D�placement de l'objet en gardant la position verticale initiale
        transform.position = new Vector3(transform.position.x, positionVerticaleInitiale + deplacementVertical, transform.position.z);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("joueur"))
        {
            photo = true;
            petitephoto.SetActive(false);
            string salleActuelle = other.gameObject.GetComponent<DetectionSalle>().salleActuelle;
            if (PhotoCollectedEvent != null && salleActuelle == "Garage" )
            {
                PhotoCollectedEvent(); // D�clenche l'�v�nement lorsque la photo est ramass�e
            }

        }
    }
}
