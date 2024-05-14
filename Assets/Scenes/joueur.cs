using UnityEngine;
using System.Collections;
public class PlayerController : MonoBehaviour
{
    public float speed = 5f; // Vitesse de déplacement du joueur
    private Rigidbody2D rb;
    private Vector3 localScale;
    private bool isInCollisionWithCompteur = false;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Récupérer le composant Rigidbody2D attaché au joueur
        localScale = transform.localScale; // Sauvegarder l'échelle initiale du joueur
    }

    void Update()
    {
        // Récupérer les entrées de l'axe horizontal et vertical
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Calculer le vecteur de déplacement
        Vector2 movement = new Vector2(moveHorizontal, moveVertical) * speed;

        // Appliquer la force de déplacement au Rigidbody2D
        rb.velocity = movement;

        // Symétrie du personnage si déplacement vers la gauche
        if (moveHorizontal < 0)
        {
            transform.localScale = new Vector3(-localScale.x, localScale.y, localScale.z);
        }
        // Symétrie du personnage si déplacement vers la droite
        else if (moveHorizontal > 0)
        {
            transform.localScale = localScale;

        
        }


        if (isInCollisionWithCompteur == true)
        {
            Debug.Log(Input.GetButtonDown("Fire1"));
            if (Input.GetButtonDown("Fire1"))
            {
                Debug.Log("Fire1 pressed while in collision with Compteur");
                // Ajoutez ici le code à exécuter lorsque Fire1 est pressé
            }
        }



    }
    

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("compteur"))
        {
            Debug.Log("Entered trigger with Compteur");
            isInCollisionWithCompteur = true;
        }
    }

   

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("compteur"))
        {
            Debug.Log("Exited trigger with Compteur");
            isInCollisionWithCompteur = false;
        }
    }







}