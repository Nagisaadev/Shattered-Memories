using UnityEngine;
using System.Collections;
using System;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f; // Vitesse de déplacement du joueur
    private Rigidbody2D rb;
    private Vector3 localScale;
    private bool isInCollisionWithCompteur = false;
    private bool isInCollisionWithCachette = false;
    private bool isHidden = false; 
    private bool isInCollisionWithPortableObject = false;
    private GameObject portableObject = null;
    public GameObject obj;
    private bool isCarryingObject = false;
    public GameObject player;
    private SpriteRenderer spriteRenderer;
    public bool activation = false;
    public GameObject dijoncteur;
    private Animator animator;
    private bool uidijoncteur;
    public bool peutpasbouger = false;
    public bool isInCollisionWithInterupteur1 = false;
    public bool isInCollisionWithInterupteur2 = false;
    public bool isInCollisionWithInterupteur3 = false;
    public bool isInCollisionWithCadrePetit = false;
    public string currentRoom;
    public Monstre monstre;


    public bool interupteursaloncolision1=false;
    public bool interupteursaloncolision2 = false;
    public bool interupteursaloncolision3 = false;
    public bool interupteursaloncolision4 = false;

    // Event to notify when an object is dropped
    public static event Action<Vector2> OnObjectDropped;
    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Récupérer le composant Rigidbody2D attaché au joueur
        spriteRenderer = GetComponent<SpriteRenderer>();
        localScale = transform.localScale; // Sauvegarder l'échelle initiale du joueur
        animator = GetComponent<Animator>();

        dijoncteur.SetActive(false);
    }

    void Update()
    {
        Debug.Log("L'objet est il porté : " + isCarryingObject);
        Debug.Log(portableObject);

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        if (!isHidden && !uidijoncteur && !peutpasbouger)
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

            animator.SetBool("isrunning", rb.velocity != Vector2.zero);
        }
        else
        {
            rb.velocity = Vector2.zero;
        }

        if (isInCollisionWithCachette && Input.GetKeyDown(KeyCode.E))
        {
            isHidden = !isHidden;
            spriteRenderer.enabled = !isHidden;
            Debug.Log(isHidden ? "Hiding in Cachette" : "Leaving Cachette");
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (isCarryingObject)
            {
                Debug.Log("F key pressed to drop the Portable Object");
                DropObject();
            }
            else if (portableObject != null)
            {
                Debug.Log("F key pressed while in collision with Portable Object");
                PickUpObject(portableObject);
            }
        }

        if (isInCollisionWithCompteur && Input.GetButtonDown("Fire1"))
        {
            dijoncteur.SetActive(true);
            uidijoncteur = true;
        }
        if (uidijoncteur && Input.GetButtonDown("Cancel"))
        {
            dijoncteur.SetActive(false);
            uidijoncteur = false;
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("compteur"))
        {
            Debug.Log("Entered trigger with Compteur");
            isInCollisionWithCompteur = true;
        }
        if (other.CompareTag("cachette"))
        {
            Debug.Log("Entered trigger with Cachette");
            isInCollisionWithCachette = true;
        }
        if (other.CompareTag("portableObject"))
        {
            Debug.Log("Entered trigger with Portable Object: " + other.name);
            portableObject = other.gameObject;
        }

        if (other.gameObject.CompareTag("interupteur1"))
        {
            isInCollisionWithInterupteur1 = true;
        }
        if (other.gameObject.CompareTag("interupteur2"))
        {
            isInCollisionWithInterupteur2 = true;
        }
        if (other.gameObject.CompareTag("interupteur3"))
        {
            isInCollisionWithInterupteur3 = true;
        }

        if (other.gameObject.CompareTag("cadrepetit"))
        {
            isInCollisionWithCadrePetit = true;
        }



        if (other.gameObject.CompareTag("interupteursalon1"))
        {
           interupteursaloncolision1 = true;
        }

        if (other.gameObject.CompareTag("interupteursalon2"))
        {
            interupteursaloncolision2 = true;
        }

        if (other.gameObject.CompareTag("interupteursalon3"))
        {
            interupteursaloncolision3 = true;
        }
        if (other.gameObject.CompareTag("interupteursalon4"))
        {
            interupteursaloncolision3 = true;
        }



    }


    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("compteur"))
        {
            Debug.Log("Exited trigger with Compteur");
            isInCollisionWithCompteur = false;
        }
        if (other.CompareTag("cachette"))
        {
            Debug.Log("Exited trigger with Cachette");
            isInCollisionWithCachette = false;
        }
        if (other.CompareTag("portableObject"))
        {
            Debug.Log("Exited trigger with Portable Object");
            portableObject = null;
        }



        if (other.gameObject.CompareTag("interupteur1"))
        {
            isInCollisionWithInterupteur1 = false;
        }
        if (other.gameObject.CompareTag("interupteur2"))
        {
            isInCollisionWithInterupteur2 = false;
        }
        if (other.gameObject.CompareTag("interupteur3"))
        {
            isInCollisionWithInterupteur3 = false;
        }

        if (other.gameObject.CompareTag("cadrepetit"))
        {
            isInCollisionWithCadrePetit = false;
        }


        if (other.gameObject.CompareTag("interupteursalon1"))
        {
            interupteursaloncolision1 = false;
        }
        if (other.gameObject.CompareTag("interupteursalon2"))
        {
            interupteursaloncolision2 = false;
        }
        if (other.gameObject.CompareTag("interupteursalon3"))
        {
            interupteursaloncolision3 = false;
        }
        if (other.gameObject.CompareTag("interupteursalon4"))
        {
            interupteursaloncolision4 = false;
        }
    }
    void PickUpObject(GameObject obj)
    {
        obj.SetActive(false);
        isCarryingObject = true;
        portableObject = obj;
    }

    void DropObject()
    {
        if (portableObject != null)
        {
            portableObject.SetActive(true);

            // Determine drop position based on player's facing direction
            Vector3 dropPosition = transform.position;
            if (transform.localScale.x > 0) // Facing right
            {
                dropPosition += transform.right;
            }
            else // Facing left
            {
                dropPosition -= transform.right;
            }

            portableObject.transform.position = dropPosition;
            HandleDroppedObject(portableObject);
            isCarryingObject = false;
            portableObject = null;
        }
    }

    void HandleDroppedObject(GameObject obj)
    {
        // Check object type and handle accordingly
        if (obj.name.Contains("Buste"))
        {
            // Make noise
            Debug.Log("Dropping Buste, making noise");
            // Faites appel à une méthode du monstre pour gérer le bruit
            monstre.HandleNoise(obj.transform.position);
        }
        else if (obj.name.Contains("Carton"))
        {
            // No noise
            Debug.Log("Dropping Carton, no noise");
        }
        else if (obj.name.Contains("Cadre"))
        {
            // Check for all photo pieces collected
            Debug.Log("Dropping Cadre, checking conditions");
            // Add code to check if all photo pieces are collected
        }
        // Notify about dropped object
        OnObjectDropped?.Invoke(obj.transform.position);
    }


}




