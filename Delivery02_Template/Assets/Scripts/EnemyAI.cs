
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyAI : MonoBehaviour
{
    [Header("Patrulla")]
    public Transform pointA;
    public Transform pointB;
    public float patrolSpeed = 3f;

    [Header("Persecución")]
    public float chaseSpeed = 2.5f;
    public float detectionRadius = 3f;
    public float fieldOfView = 90f;
    public LayerMask obstacleMask;
    public LayerMask playerMask;

    [Header("Referencia al Jugador")]
    public Transform player;
    private Vector3 initialPosition;
    private Transform currentPatrolTarget;
    private SpriteRenderer spriteRenderer;
    private State currentState = State.Patrol;

    private Vector2 lastDirection = Vector2.up; // Guarda la última dirección del movimiento

    private enum State { Patrol, Chase, Return }

    void Start()
    {
        initialPosition = transform.position;
        currentPatrolTarget = pointB;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (player == null) return;

        switch (currentState)
        {
            case State.Patrol:
                Patrol();
                if (CanSeePlayer())
                {
                    currentState = State.Chase;
                    spriteRenderer.color = Color.red;
                }
                break;
            case State.Chase:
                Chase();
                if (!CanSeePlayer())
                {
                    currentState = State.Return;
                    spriteRenderer.color = Color.white;
                }
                if (Vector2.Distance(transform.position, player.position) < 0.5f)
                {
                    Debug.Log("Jugador atrapado");
                    SceneManager.LoadScene("Ending");
                }
                break;
            case State.Return:
                ReturnToInitial();
                if (Vector2.Distance(transform.position, initialPosition) < 0.1f)
                {
                    currentState = State.Patrol;
                    currentPatrolTarget = GetClosestPatrolPoint();
                }
                break;
        }
    }

    void Patrol()
    {
        MoveTowards(currentPatrolTarget.position, patrolSpeed);
        if (Vector2.Distance(transform.position, currentPatrolTarget.position) < 0.1f)
            currentPatrolTarget = (currentPatrolTarget == pointA) ? pointB : pointA;
    }

    void Chase()
    {
        MoveTowards(player.position, chaseSpeed);
    }

    void ReturnToInitial()
    {
        MoveTowards(initialPosition, patrolSpeed);
    }

    void MoveTowards(Vector3 target, float speed)
    {
        Vector2 direction = (target - transform.position).normalized;

        if (direction.magnitude > 0.01f)
        {
            lastDirection = direction; // Guardar la última dirección si hay movimiento
        }

        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
        float angle = Mathf.Atan2(lastDirection.y, lastDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }

    bool CanSeePlayer()
    {
        Vector2 directionToPlayer = (player.position - transform.position).normalized;
        float adjustedAngle = Vector2.Angle(lastDirection, directionToPlayer);

        if (adjustedAngle > fieldOfView / 2) return false;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer > detectionRadius) return false;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, detectionRadius, obstacleMask | playerMask);
        return hit.collider != null && hit.collider.CompareTag("Player");
    }

    private Transform GetClosestPatrolPoint() 
    {
        return Vector2.Distance(transform.position, pointA.position) < Vector2.Distance(transform.position, pointB.position) ? pointA : pointB;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        if (pointA != null && pointB != null) Gizmos.DrawLine(pointA.position, pointB.position);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        // 🔹 Usamos lastDirection para la dirección del cono de visión
        Vector3 forward = lastDirection;
        Vector3 leftBoundary = Quaternion.Euler(0, 0, fieldOfView / 2) * forward * detectionRadius;
        Vector3 rightBoundary = Quaternion.Euler(0, 0, -fieldOfView / 2) * forward * detectionRadius;

        Gizmos.color = new Color(0.5f, 0.5f, 0.5f, 0.3f); // Gris opaco
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary);
    }
}
