using UnityEngine;
using UnityEngine.SceneManagement; // Para cargar la escena de final

public class Bullet : MonoBehaviour
{
    public float speed = 5f;         // Velocidad de la bala
    public float lifetime = 5f;      // Tiempo máximo de vida

    void Start()
    {
        // Destruir la bala después de "lifetime" segundos para evitar que se queden en la escena
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Mover la bala hacia adelante en su dirección local (Vector2.up)
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    // Utilizamos OnTriggerEnter2D si el collider está marcado como Trigger.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si choca con una pared, destruir la bala
        if (collision.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
        // Si choca con el Player, terminar la partida
        else if (collision.gameObject.CompareTag("Player"))
        {
            // Aquí finalizamos la partida. Por ejemplo, cargamos la escena "Ending"
            SceneManager.LoadScene("Ending");
        }
    }
}
