using UnityEngine;

public class Monstre : MonoBehaviour
{
    public Transform player;
    public float speed = 2f;
    public float detectionRange = 5f;

    private bool isPlayerInRange = false;

    void Update()
    {
        DetectPlayer();

        if (isPlayerInRange)
        {
            ChasePlayer();
        }
    }

    void DetectPlayer()
    {
        if (Vector2.Distance(transform.position, player.position) < detectionRange)
        {
            isPlayerInRange = true;
        }
        else
        {
            isPlayerInRange = false;
        }
    }

    void ChasePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}