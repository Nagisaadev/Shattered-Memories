using UnityEngine;
using System.Collections.Generic;

public class Monstre : MonoBehaviour
{
    public Transform player;
    public float speed = 2f;
    public float detectionRange = 5f;
    public List<Transform> patrolPoints; // Liste des points de patrouille
    private int currentPatrolIndex = 0; // Indice du point de patrouille actuel

    private bool isPlayerDetected = false;
    private bool isPlayerInRange = false;
    private Vector2 noiseLocation;
    private bool isNoiseDetected = false;
    private bool hasInvestigatedNoise = false; // Variable pour suivre si le bruit a été investigué
    private float noiseDetectionTime = 5f; // Temps pour constater le bruit
    private float noiseTimer = 0f;
    private float noiseInvestigationStartTime; // Heure à laquelle le monstre commence à enquêter sur le bruit
    private float noiseInvestigationTime = 5f; // Temps d'investigation du bruit
    private float noiseInvestigationTimer = 0f; // Timer pour l'investigation du bruit
    private bool hasHeardNoise = false; // Indique si le monstre a entendu un bruit
    private Vector2 lastHeardNoisePosition; // Position du dernier bruit entendu


    private Pathfinding pathfinding;
    private List<Node> path;
    private int targetIndex;

    void Start()
    {
        pathfinding = FindObjectOfType<Pathfinding>();
    }

    void OnEnable()
    {
        PlayerController.OnObjectDropped += OnObjectDropped;
    }

    void OnDisable()
    {
        PlayerController.OnObjectDropped -= OnObjectDropped;
    }

    void Update()
    {
        DetectPlayer();

        if (isPlayerInRange)
        {
            hasInvestigatedNoise = false; // Réinitialiser l'investigation du bruit si le joueur est détecté
            path = pathfinding.FindPath(transform.position, player.position);
            Debug.Log("Player in range. Path length: " + (path != null ? path.Count.ToString() : "null"));
            FollowPath();
        }
        else if (isNoiseDetected && !hasInvestigatedNoise)
        {
            path = pathfinding.FindPath(transform.position, noiseLocation);
            Debug.Log("Noise detected. Path length: " + (path != null ? path.Count.ToString() : "null"));
            FollowPath();

            // Vérifier si le monstre est arrivé à la position du bruit
            if (path == null || path.Count == 0)
            {
                isNoiseDetected = false;
                hasInvestigatedNoise = true; // Marquer le bruit comme investigué
                lastHeardNoisePosition = noiseLocation; // Enregistrer la position du bruit
                noiseInvestigationStartTime = Time.time; // Enregistrer le temps de début de l'enquête sur le bruit
                Debug.Log("No path available.");
                return;
            }

            if (Vector2.Distance(transform.position, noiseLocation) < 0.1f)
            {
                // Ne rien faire tant que le monstre est à proximité du bruit
                return;
            }
        }
        else if (hasInvestigatedNoise)
        {
            // Attendre près du bruit pendant un certain temps
            if (Time.time - noiseInvestigationStartTime >= noiseInvestigationTime)
            {
                // Revenir à la patrouille normale
                hasInvestigatedNoise = false;
                hasHeardNoise = false;
                path = null;
                // Reprendre le chemin interrompu
                if (lastHeardNoisePosition != null)
                {
                    path = pathfinding.FindPath(transform.position, lastHeardNoisePosition);
                }
            }
        }
        
        
        else
        {
            Patrol();
        }
    }

    void DetectPlayer()
    {
        if (Vector2.Distance(transform.position, player.position) < detectionRange)
        {
            isPlayerInRange = true;
            isNoiseDetected = false; // Stop following noise if player is detected
        }
        else
        {
            isPlayerInRange = false;
        }
    }

    void FollowPath()
    {
        if (path == null || path.Count == 0)
        {
            Debug.Log("No path available.");
            return;
        }

        if (isPlayerDetected)
        {
            // Calculer la position du joueur
            Vector3 playerPosition = player.position;

            // Se déplacer directement vers la position du joueur
            transform.position = Vector2.MoveTowards(transform.position, playerPosition, speed * Time.deltaTime);
        }
        else
        {
            // Continuer à suivre le chemin normal
            if (targetIndex < path.Count)
            {
                Node targetNode = path[targetIndex];
                Vector3 targetPosition = targetNode.worldPosition;
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

                if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
                {
                    targetIndex++;
                }
            }
        }
    }

    void Patrol()
    {
        if (patrolPoints.Count == 0)
        {
            Debug.LogWarning("No patrol points assigned!");
            return;
        }

        // Aller au prochain point de patrouille
        Transform nextPatrolPoint = patrolPoints[currentPatrolIndex];
        float step = speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, nextPatrolPoint.position, step);

        // Vérifier si le monstre a atteint le point de patrouille
        if (Vector2.Distance(transform.position, nextPatrolPoint.position) < 0.1f)
        {
            // Passer au prochain point de patrouille
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
        }
    }

    void OnObjectDropped(Vector2 dropLocation)
    {
        noiseLocation = dropLocation;
        isNoiseDetected = true;
        hasInvestigatedNoise = false; // Réinitialiser l'investigation du bruit
        noiseTimer = 0f; // Réinitialiser le timer du bruit
        targetIndex = 0; // Réinitialiser l'index de chemin lorsque qu'un nouveau bruit est détecté
        path = null; // Réinitialiser le chemin lorsque qu'un nouveau bruit est détecté
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("joueur"))
        {
            KillPlayer();
        }
    }

    void KillPlayer()
    {
        // Code pour gérer la mort du joueur
        Destroy(player.gameObject);
        Debug.Log("Player has been killed!");
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}









