using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f; // Vitesse de d�placement du joueur
    private Rigidbody2D rb;
    private Vector3 localScale;
    private bool isInCollisionWithCompteur = false;
    private bool isInCollisionWithCachette = false;
    private bool isHidden = false; // Variable pour suivre l'�tat du joueur (cach� ou non)

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // R�cup�rer le composant Rigidbody2D attach� au joueur
        localScale = transform.localScale; // Sauvegarder l'�chelle initiale du joueur
    }

    void Update()
    {
        // R�cup�rer les entr�es de l'axe horizontal et vertical
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Si le joueur est cach�, ne pas lui permettre de se d�placer
        if (!isHidden)
        {
            // Calculer le vecteur de d�placement
            Vector2 movement = new Vector2(moveHorizontal, moveVertical) * speed;

            // Appliquer la force de d�placement au Rigidbody2D
            rb.velocity = movement;

            // Sym�trie du personnage si d�placement vers la gauche
            if (moveHorizontal < 0)
            {
                transform.localScale = new Vector3(-localScale.x, localScale.y, localScale.z);
            }
            // Sym�trie du personnage si d�placement vers la droite
            else if (moveHorizontal > 0)
            {
                transform.localScale = localScale;
            }
        }

        // Si le joueur est en collision avec une cachette et appuie sur la touche "E"
        if (isInCollisionWithCachette && Input.GetKeyDown(KeyCode.E))
        {
            isHidden = !isHidden; // Inverser l'�tat de cach�/non cach�
            Debug.Log(isHidden ? "Hiding in Cachette" : "Leaving Cachette");
            // Ajoutez ici le code � ex�cuter lorsque le joueur entre/sort de la cachette
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
