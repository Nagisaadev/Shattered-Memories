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
    private bool isHidden = false; // Variable pour suivre l'état du joueur (caché ou non)
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
        Debug.Log(isInCollisionWithPortableObject);
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        if (!isHidden || !uidijoncteur || !peutpasbouger)
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

            if (rb.velocity == movement)
            {
                animator.SetBool("isrunning", true);
            }

            if (rb.velocity == new Vector2(0f, 0f))
            {
                animator.SetBool("isrunning", false);
            }
        }

        if (isHidden || uidijoncteur || peutpasbouger)
        {
            rb.velocity = new Vector2(0, 0);
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
                DropObject(); // Passer portableObject à la fonction DropObject
            }

        
        else if (isInCollisionWithPortableObject && portableObject != null)
            {
                Debug.Log("F key pressed while in collision with Portable Object");
                PickUpObject(portableObject);
            }
        
    }
        Debug.Log(portableObject);

        if (isInCollisionWithCompteur == true)
        {
            Debug.Log(Input.GetButtonDown("Fire1"));
            if (Input.GetButtonDown("Fire1"))
            {
                Debug.Log("Fire1 pressed while in collision with Compteur");
                dijoncteur.SetActive(true);
                uidijoncteur = true;
            }
            if (Input.GetButtonDown("Cancel"))
            {
                dijoncteur.SetActive(false);
                uidijoncteur = false;
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
        if (other.gameObject.CompareTag("cachette"))
        {
            Debug.Log("Entered trigger with Cachette");
            isInCollisionWithCachette = true;
        }
        if (other.gameObject.CompareTag("portableObject"))
        {
            Debug.Log("Entered trigger with Portable Object: " + other.gameObject.name);
            isInCollisionWithPortableObject = true;
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
        if (other.gameObject.CompareTag("compteur"))
        {
            Debug.Log("Exited trigger with Compteur");
            isInCollisionWithCompteur = false;
        }
        if (other.gameObject.CompareTag("cachette"))
        {
            Debug.Log("Exited trigger with Cachette");
            isInCollisionWithCachette = false;
        }
        if (other.gameObject.CompareTag("portableObject"))
        {
            Debug.Log("Exited trigger with Portable Object");
            isInCollisionWithPortableObject = false;
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
        portableObject.SetActive(false);
        
        isCarryingObject = true;
    }

    void DropObject()
    {
        if (portableObject != null)
        {
            portableObject.SetActive(true); // Réactiver l'objet
            portableObject.transform.position = transform.position + transform.right; // Repositionner l'objet près du joueur
            isCarryingObject = false;

            OnObjectDropped?.Invoke(portableObject.transform.position); // Déclencher l'événement
            portableObject = null;
        }
        else
        {
            Debug.LogWarning("Trying to drop a null object!");
        }
    }



}
