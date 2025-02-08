using UnityEngine;
using UnityEngine.SceneManagement; // Si usas SceneManager para cambiar de escena

public class EnemyAI : MonoBehaviour
{
    // Puntos de patrulla
    [Header("Patrulla")]
    public Transform pointA;
    public Transform pointB;
    public float patrolSpeed = 3f; // Velocidad al patrullar

    // Configuración de persecución
    [Header("Persecución")]
    public float chaseSpeed = 2.5f;       // Velocidad al perseguir
    public float detectionRadius = 3f;    // Rango de visión para iniciar la persecución

    // Configuración para atrapar al jugador (opcional)
    public float catchDistance = 0.5f;    // Distancia para considerar que el jugador ha sido atrapado

    // Referencia al jugador (asignar en el Inspector)
    [Header("Referencia al Jugador")]
    public Transform player;

    // Variable para almacenar la posición inicial del enemigo (usada al regresar)
    private Vector3 initialPosition;

    // Variable para almacenar el punto de patrulla actual
    private Transform currentPatrolTarget;

    // Componente para cambiar el color
    private SpriteRenderer spriteRenderer;

    // Variable para ajustar la rotación (prueba con diferentes valores: 90, -90, 0, etc.)
    [Header("Ajuste de Rotación")]
    public float rotationOffset = -90f;

    // Estados del enemigo
    private enum State { Patrol, Chase, Return }
    private State currentState = State.Patrol;

    void Start()
    {
        // Guardamos la posición inicial
        initialPosition = transform.position;

        // Configuramos el primer objetivo de patrulla
        currentPatrolTarget = pointB;

        // Obtenemos el SpriteRenderer para cambiar el color
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("No se encontró un SpriteRenderer en " + gameObject.name);
        }
    }

    void Update()
    {
        // Asegurarse de tener asignado el jugador
        if (player == null)
        {
            Debug.LogWarning("No se ha asignado el jugador en " + gameObject.name);
            return;
        }

        // Dependiendo del estado, se ejecuta una lógica u otra
        switch (currentState)
        {
            case State.Patrol:
                Patrol();
                // Si el jugador entra en el rango de visión, pasamos a estado Chase
                if (Vector2.Distance(transform.position, player.position) <= detectionRadius)
                {
                    currentState = State.Chase;
                    if (spriteRenderer != null)
                        spriteRenderer.color = Color.red; // Cambia el color cuando detecta al jugador
                }
                break;

            case State.Chase:
                Chase();
                // Si el jugador se aleja del rango de detección, cambiamos a estado Return
                if (Vector2.Distance(transform.position, player.position) > detectionRadius)
                {
                    currentState = State.Return;
                    if (spriteRenderer != null)
                        spriteRenderer.color = Color.white; // Restauramos el color original
                }
                // Opcional: si el enemigo atrapa al jugador (se acerca demasiado), se puede ejecutar otra acción
                if (Vector2.Distance(transform.position, player.position) < catchDistance)
                {
                    // Por ejemplo, finalizar la partida o cargar otra escena
                    Debug.Log("Jugador atrapado");
                    SceneManager.LoadScene("Ending");
                    // Ejemplo: SceneManager.LoadScene("Ending");
                }
                break;

            case State.Return:
                ReturnToInitial();
                // Cuando el enemigo llega a su posición inicial, vuelve a patrullar
                if (Vector2.Distance(transform.position, initialPosition) < 0.1f)
                {
                    currentState = State.Patrol;
                    // Se determina el punto de patrulla más cercano para retomar la patrulla
                    currentPatrolTarget = GetClosestPatrolPoint();
                }
                break;
        }
    }

    // Método de patrulla entre los puntos A y B
    void Patrol()
    {
        // Calcular la dirección hacia el punto de patrulla
        Vector2 direction = (currentPatrolTarget.position - transform.position).normalized;
        // Actualizar la rotación para que mire hacia el objetivo
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + rotationOffset);

        // Mover al enemigo hacia el punto de patrulla
        transform.position = Vector2.MoveTowards(transform.position, currentPatrolTarget.position, patrolSpeed * Time.deltaTime);

        // Al llegar, cambiar el objetivo
        if (Vector2.Distance(transform.position, currentPatrolTarget.position) < 0.1f)
        {
            currentPatrolTarget = (currentPatrolTarget == pointA) ? pointB : pointA;
        }
    }

    // Método para perseguir al jugador
    void Chase()
    {
        // Calcular la dirección hacia el jugador
        Vector2 direction = (player.position - transform.position).normalized;
        // Actualizar la rotación para que mire al jugador
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + rotationOffset);

        // Mover al enemigo hacia el jugador
        transform.position = Vector2.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);
    }

    // Método para que el enemigo regrese a su posición inicial
    void ReturnToInitial()
    {
        // Calcular la dirección hacia la posición inicial
        Vector2 direction = (initialPosition - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + rotationOffset);

        // Mover al enemigo hacia la posición inicial
        transform.position = Vector2.MoveTowards(transform.position, initialPosition, patrolSpeed * Time.deltaTime);
    }

    // Ayuda para determinar el punto de patrulla más cercano
    private Transform GetClosestPatrolPoint()
    {
        if (Vector2.Distance(transform.position, pointA.position) < Vector2.Distance(transform.position, pointB.position))
            return pointA;
        else
            return pointB;
    }

    // Dibujar Gizmos para visualizar el rango de visión y la ruta de patrulla en el Editor
    private void OnDrawGizmos()
    {
        // Dibuja la línea entre los puntos de patrulla si están asignados
        if (pointA != null && pointB != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(pointA.position, pointB.position);
            Gizmos.DrawWireSphere(pointA.position, 0.2f);
            Gizmos.DrawWireSphere(pointB.position, 0.2f);
        }
        // Dibuja el rango de detección
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
