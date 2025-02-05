using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Puntos de Patrulla")]
    // Transforms que representarán los dos puntos de patrulla (A y B)
    public Transform pointA;
    public Transform pointB;

    [Header("Configuración de Movimiento")]
    public float moveSpeed = 3f;  // Velocidad de movimiento

    // Variable interna para almacenar el punto destino actual
    private Transform currentTarget;

    void Start()
    {
        // Verificamos que ambos puntos estén asignados
        if (pointA == null || pointB == null)
        {
            Debug.LogError("No se han asignado los puntos de patrulla en " + gameObject.name);
            return;
        }

        // Iniciamos con el punto B como destino (podrías comenzar con A, según prefieras)
        currentTarget = pointB;
    }

    void Update()
    {
        // Si no se asignaron correctamente los puntos, salimos
        if (pointA == null || pointB == null)
            return;

        // Movimiento hacia el objetivo actual
        transform.position = Vector2.MoveTowards(transform.position, currentTarget.position, moveSpeed * Time.deltaTime);

        // Verificamos si hemos alcanzado el destino (con una tolerancia)
        if (Vector2.Distance(transform.position, currentTarget.position) < 0.1f)
        {
            // Cambiamos el destino: si era A, pasamos a B, y viceversa
            currentTarget = (currentTarget == pointA) ? pointB : pointA;

            // Realizamos un giro de 180º en Z
            transform.Rotate(0, 0, 180f);
        }
    }

    // Dibujar Gizmos para visualizar la ruta y los puntos en el editor
    private void OnDrawGizmos()
    {
        // Si se asignaron ambos puntos
        if (pointA != null && pointB != null)
        {
            // Dibuja una línea entre los dos puntos de patrulla
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(pointA.position, pointB.position);

            // Dibuja esferas para representar los puntos A y B
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(pointA.position, 0.2f);
            Gizmos.DrawWireSphere(pointB.position, 0.2f);
        }
    }
}
