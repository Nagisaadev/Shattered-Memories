using UnityEngine;
using System.Collections;
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

    private bool isAppeared = false;
    public List<Transform> patrolPointsCuisine; // Liste des points de patrouille pour la cuisine
    public List<Transform> patrolPointsSalleAManger; // Liste des points de patrouille pour la salle à manger
    public List<Transform> patrolPointsGarage; // Liste des points de patrouille pour le garage

    // Positions des portes
    public Vector2 porteCuisine; // Définir dans l'inspecteur de Unity
    public Vector2 porteSalleAManger; // Définir dans l'inspecteur de Unity
    public Vector2 porteGarage; // Définir dans l'inspecteur de Unity

    private Coroutine apparitionCoroutine;

    void Start()
    {
        pathfinding = FindObjectOfType<Pathfinding>();
        apparitionCoroutine = StartCoroutine(ApparitionCoroutine());
        player = GameObject.FindGameObjectWithTag("joueur").transform;
        if (player == null)
        {
            Debug.LogError("Aucun joueur trouvé !");
        }
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
        // Déterminer la salle actuelle
        string salleActuelle = DeterminerSalleActuelle();

        // Assigner la patrouille en fonction de la salle actuelle
        switch (salleActuelle)
        {
            case "Cuisine":
                patrolPoints = patrolPointsCuisine;
                break;
            case "Salle à manger":
                patrolPoints = patrolPointsSalleAManger;
                break;
            case "Garage":
                patrolPoints = patrolPointsGarage;
                break;
        }

        // Détection du joueur
        DetectPlayer();

        // Logique de suivi du joueur ou de patrouille selon la situation
        if (isPlayerInRange)
        {
            hasInvestigatedNoise = false;
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
                hasInvestigatedNoise = true;
                lastHeardNoisePosition = noiseLocation;
                noiseInvestigationStartTime = Time.time;
                Debug.Log("No path available.");
                return;
            }

            if (Vector2.Distance(transform.position, noiseLocation) < 0.1f)
            {
                return;
            }
        }
        else if (hasInvestigatedNoise)
        {
            if (Time.time - noiseInvestigationStartTime >= noiseInvestigationTime)
            {
                hasInvestigatedNoise = false;
                hasHeardNoise = false;
                path = null;
                GoToClosestPatrolPoint();
            }
        }
        else
        {
            Patrol();
        }
    }

    string DeterminerSalleActuelle()
    {
        return GetComponent<DetectionSalle>().salleActuelle;
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

    void GoToClosestPatrolPoint()
    {
        if (patrolPoints.Count == 0)
        {
            Debug.LogWarning("No patrol points assigned!");
            return;
        }

        float closestDistance = float.MaxValue;
        int closestIndex = 0;

        for (int i = 0; i < patrolPoints.Count; i++)
        {
            float distance = Vector2.Distance(transform.position, patrolPoints[i].position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestIndex = i;
            }
        }

        currentPatrolIndex = closestIndex;
        Patrol();
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

    IEnumerator ApparitionCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(15f); // Attendre 15 secondes

            // Téléporter le monstre à la porte de la salle du joueur
            TeleportToPlayerRoom();
        }
    }

    void TeleportToPlayerRoom()
    {
        // Obtenir la salle actuelle du joueur
        string salleActuelle = player.GetComponent<DetectionSalle>().salleActuelle;

        // Déterminer la position de la porte de la salle actuelle du joueur
        Vector2 targetPosition = Vector2.zero;

        switch (salleActuelle)
        {
            case "Cuisine":
                targetPosition = porteCuisine;
                break;
            case "Salle à manger":
                targetPosition = porteSalleAManger;
                break;
            case "Garage":
                targetPosition = porteGarage;
                break;
            default:
                Debug.LogWarning("Salle actuelle non reconnue: " + salleActuelle);
                break;
        }

        // Téléporter le monstre à la porte de la salle où se trouve le joueur
        transform.position = targetPosition;
        Debug.Log("Le monstre est apparu à la porte de la salle: " + salleActuelle + " à la position: " + targetPosition);
    }
}












