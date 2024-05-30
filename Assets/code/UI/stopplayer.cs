using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stopplayer : MonoBehaviour
{
    private Animator animator;
    public GameObject colision;
    public float delay = 20.0f;
    public PlayerController playerController;
    public float dureeDescendre = 3.0f; // Durée de la transition de l'opacité
    private SpriteRenderer spriteRenderer;
    public int ordre=0;
    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("joueur"))
        {
            ordre = 1;
           playerController.peutpasbouger = true;
            StartCoroutine(DestroyAfterDelay());
           
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }

    IEnumerator DestroyAfterDelay()
    {
        // Attendre le délai spécifié
        yield return new WaitForSeconds(delay);
        // Détruire le GameObject
        Destroy(gameObject);
        playerController.peutpasbouger = false;
        
        ordre = 2;
        

    }


}
