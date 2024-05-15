using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f; // Vitesse de déplacement du joueur
    private Rigidbody2D rb;
    private Vector3 localScale;
    private bool isInCollisionWithCompteur = false;
    private bool isInCollisionWithCachette = false;
    private bool isHidden = false; // Variable pour suivre l'état du joueur (caché ou non)

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

        // Si le joueur est caché, ne pas lui permettre de se déplacer
        if (!isHidden)
        {
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
        }

        // Si le joueur est en collision avec une cachette et appuie sur la touche "E"
        if (isInCollisionWithCachette && Input.GetKeyDown(KeyCode.E))
        {
            isHidden = !isHidden; // Inverser l'état de caché/non caché
            Debug.Log(isHidden ? "Hiding in Cachette" : "Leaving Cachette");
            // Ajoutez ici le code à exécuter lorsque le joueur entre/sort de la cachette
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("compteur"))
        {
            Debug.Log("Entered trigger with Compteur");
            isInCollisionWithCompteur = true;
        }
        if (other.gameObject.CompareTag("Cachette"))
        {
            Debug.Log("Entered trigger with Cachette");
            isInCollisionWithCachette = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("compteur"))
        {
            Debug.Log("Exited trigger with Compteur");
            isInCollisionWithCompteur = false;
        }
        if (other.gameObject.CompareTag("Cachette"))
        {
            Debug.Log("Exited trigger with Cachette");
            isInCollisionWithCachette = false;
        }
    }
}
