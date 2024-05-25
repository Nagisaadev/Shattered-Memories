using UnityEngine;
using System.Collections.Generic;

public class Monstre : MonoBehaviour
{
    public Transform player;
    public float speed = 2f;
    public float detectionRange = 5f;
    public List<Transform> patrolPoints; // Liste des points de patrouille
    private int currentPatrolIndex = 0; // Indice du point de patrouille actuel

    private bool isPlayerInRange = false;
    private Vector2 noiseLocation;
    private bool isNoiseDetected = false;

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
            path = pathfinding.FindPath(transform.position, player.position);
            Debug.Log("Player in range. Path length: " + (path != null ? path.Count.ToString() : "null"));
            FollowPath();
        }
        else if (isNoiseDetected)
        {
            path = pathfinding.FindPath(transform.position, noiseLocation);
            Debug.Log("Noise detected. Path length: " + (path != null ? path.Count.ToString() : "null"));
            FollowPath();
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
        targetIndex = 0; // Reset path index when a new path is generated
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("j"))
        {
            KillPlayer();
        }
    }

    void KillPlayer()
    {
        // Code to handle player death
        // For example, disabling the player object
        Destroy(player.gameObject);
        Debug.Log("Player has been killed!");
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}



