using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    [Header("Referencia al Jugador")]
    public Transform player;

    [Header("Detección y Disparo")]
    public float detectionRange = 5f; // Rango de visión para detectar al Player
    public float fireRate = 1f;       // Cantidad de disparos por segundo
    private float nextFireTime = 0f;

    [Header("Prefab de Bala y Punto de Disparo")]
    public GameObject bulletPrefab;      // Asigna el prefab de la bala
    public Transform bulletSpawnPoint;   // Asigna el objeto hijo "BulletSpawnPoint"

    [Header("Ajuste de Rotación")]
    public float rotationOffset = 90f; // Ajusta este valor para que la bala se dispare hacia adelante

    // Para cambiar de color cuando detecta al jugador (opcional)
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogWarning("No se encontró SpriteRenderer en " + gameObject.name);
        }
    }

    void Update()
    {
        if (player == null)
        {
            Debug.LogWarning("No se ha asignado el Player en " + gameObject.name);
            return;
        }

        // Comprobar la distancia al Player
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            // Rotar el enemigo para que mire al Player
            Vector2 direction = (player.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle + rotationOffset);

            // Cambiar color si se desea (por ejemplo, a rojo)
            if (spriteRenderer != null)
                spriteRenderer.color = Color.red;

            // Disparar si se cumple el tiempo
            if (Time.time >= nextFireTime)
            {
                FireBullet();
                nextFireTime = Time.time + 1f / fireRate;
            }
        }
        else
        {
            // Si el jugador está fuera del rango, restaurar el color original
            if (spriteRenderer != null)
                spriteRenderer.color = Color.white;
        }
    }

    void FireBullet()
    {
        // Instanciar la bala en la posición y rotación del BulletSpawnPoint
        Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
    }

    // Opcional: dibujar el rango de detección en la escena
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
