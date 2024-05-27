using UnityEngine;
using System.Collections;
using System;

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
    public GameObject player;
    private SpriteRenderer spriteRenderer;
    public bool activation = false;
    public GameObject dijoncteur;
    private Animator animator;
    private bool uidijoncteur;

    // Event to notify when an object is dropped
    public static event Action<Vector2> OnObjectDropped;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // R�cup�rer le composant Rigidbody2D attach� au joueur
        spriteRenderer = GetComponent<SpriteRenderer>();
        localScale = transform.localScale; // Sauvegarder l'�chelle initiale du joueur
        animator = GetComponent<Animator>();

        dijoncteur.SetActive(false);
    }

    void Update()
    {
        Debug.Log("L'objet est il port� : " + isCarryingObject);
        Debug.Log(isInCollisionWithPortableObject);
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        if (!isHidden || !uidijoncteur)
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

        if (isHidden || uidijoncteur)
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
                DropObject();
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
        if (other.gameObject.CompareTag("cachette"))
        {
            Debug.Log("Exited trigger with Cachette");
            isInCollisionWithCachette = false;
        }
        if (other.gameObject.CompareTag("portableObject"))
        {
            Debug.Log("Exited trigger with Portable Object");
            isInCollisionWithPortableObject = false;
            portableObject = other.gameObject;
        }

    }

    void PickUpObject(GameObject obj)
    {
        obj.SetActive(false);
        isCarryingObject = true;
    }

    void DropObject()
    {
        if (portableObject != null)
        {
            portableObject.SetActive(true);
            obj.transform.position = transform.position + transform.right;
            isCarryingObject = false;

            OnObjectDropped?.Invoke(transform.position); // Trigger the event
            portableObject = null;
        }
    }
 }

