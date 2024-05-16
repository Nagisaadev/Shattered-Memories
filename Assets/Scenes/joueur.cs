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
    private bool isInCollisionWithPortableObject = false;
    private GameObject portableObject = null;
    public GameObject obj;
    private bool isCarryingObject = false;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // R�cup�rer le composant Rigidbody2D attach� au joueur
        localScale = transform.localScale; // Sauvegarder l'�chelle initiale du joueur
    }

    void Update()
    {
        Debug.Log("L'objet est il port� : " +isCarryingObject);
        Debug.Log(isInCollisionWithPortableObject);
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        

        if (!isHidden)
        {
            Vector2 movement = new Vector2(moveHorizontal, moveVertical) * speed;
            rb.velocity = movement;
            if (moveHorizontal < 0)
            {
                transform.localScale = new Vector3(-localScale.x, localScale.y, localScale.z);
            }
            else if (moveHorizontal > 0)
            {
                transform.localScale = localScale;
            }
        }
        if (isHidden)
        {
            rb.velocity = new Vector2(0, 0);

        }

            // Si le joueur est en collision avec une cachette et appuie sur la touche "E"
            if (isInCollisionWithCachette && Input.GetKeyDown(KeyCode.E))
        {
            isHidden = !isHidden; // Inverser l'�tat de cach�/non cach�
            Debug.Log(isHidden ? "Hiding in Cachette" : "Leaving Cachette");
            // Ajoutez ici le code � ex�cuter lorsque le joueur entre/sort de la cachette
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (isCarryingObject) // Si le joueur porte d�j� un objet, rel�chez-le
            {
                Debug.Log("F key pressed to drop the Portable Object");
                DropObject();
            }
            else if (isInCollisionWithPortableObject && portableObject != null) // Si un objet est en collision et n'est pas d�j� port�, ramassez-le
            {
                Debug.Log("F key pressed while in collision with Portable Object");
                PickUpObject(portableObject);
            }
        }
        Debug.Log(portableObject);
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
        if (other.gameObject.CompareTag("portableObject"))
        {
            Debug.Log("Entered trigger with Portable Object");
            isInCollisionWithPortableObject = true;
            portableObject = other.gameObject;
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
        if (other.gameObject.CompareTag("portableObject"))
        {
            Debug.Log("Entered trigger with Portable Object");
            isInCollisionWithPortableObject = true;
            portableObject = other.gameObject; // Initialisation de portableObject avec l'objet en collision
        }
    }
    void PickUpObject(GameObject obj)
    {
        obj.SetActive(false); // D�sactivez le rendu de l'objet
        isCarryingObject = true; // Activez l'�tat de portage de l'objet

        // Positionnez l'objet pr�s du joueur
        
    }

    void DropObject()
    {
        if (portableObject != null) // V�rifiez que l'objet est diff�rent de null avant de l�cher
        {
            portableObject.SetActive(true); // R�activez le rendu de l'objet
            obj.transform.position = transform.position + transform.right;
            isCarryingObject = false; // D�sactivez l'�tat de portage de l'objet
            portableObject = null; // R�initialisez l'objet port�
            
        }

    }
}
