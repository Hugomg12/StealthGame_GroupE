using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public bool IsMoving => _isMoving;

    [SerializeField]
    private float Speed = 5.0f;

    private bool _isMoving;
    private Rigidbody2D _rigidbody;

    // Variables para manejar la distancia recorrida
    private Vector2 lastPosition;  // Última posición
    private float totalDistance;  // Distancia total

    private int score = 2000;

    public static Action<float> OnPlayerMoved; // Ahora enviamos distancia en lugar de posición
    public static Action<float> OnTimeUpdated;


    private float elapsedTime; // Tiempo transcurrido

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        lastPosition = transform.position;  // Inicializar la última posición del jugador
    }

    private void Update()
    {
        // Actualizar tiempo transcurrido
        elapsedTime += Time.deltaTime;
        OnTimeUpdated?.Invoke(elapsedTime);  // Enviar el tiempo transcurrido

        // Calcular la distancia recorrida entre la última posición y la posición actual
        float deltaDistance = Vector2.Distance(lastPosition, (Vector2)transform.position);


        // Si la distancia recorrida es significativa, podemos seguir con la lógica de acumulación
        // Si no necesitas la acumulación, puedes omitir el bloque siguiente:
        if (deltaDistance > 0.001f) // Evitar sumar valores pequeños por imprecisión
        {
            totalDistance += deltaDistance; // Acumular la distancia recorrida
            ControladorPuntos.Instance.UpdatePuntos(deltaDistance); // Aumentar los puntos
       
          
            OnPlayerMoved?.Invoke(deltaDistance); // Enviar solo la distancia recorrida este frame
        }

        lastPosition = transform.position; // Actualizar la última posición del jugador para el siguiente frame
    }

    public void OnMove(InputValue value)
    {
        // Leer el valor del control (Vector2) para mover al jugador
        var inputVal = value.Get<Vector2>();

        // Calcular la velocidad a partir del valor de entrada y la velocidad del jugador
        Vector2 velocity = inputVal * Speed;
        _rigidbody.linearVelocity = velocity;  // Establecer la velocidad del Rigidbody2D

        // Verificar si el jugador está en movimiento
        _isMoving = (velocity.magnitude > 0.01f);

        // Si el jugador se está moviendo, se rota hacia la dirección de la velocidad
        if (_isMoving)
            LookAt((Vector2)transform.position + velocity);
        else
            transform.rotation = Quaternion.identity;  // Devolver la rotación a la posición inicial
    }

    // Método para guardar la puntuación
    public void OnSaveScore()
    {
        PlayerPrefs.SetInt("Score", score);
        score = PlayerPrefs.GetInt("Score");
    }

    // Método para hacer que el jugador mire en la dirección de movimiento
    private void LookAt(Vector2 targetPosition)
    {
        // Calcular el ángulo entre la posición actual y el objetivo
        float angle = 0.0f;
        Vector3 relative = transform.InverseTransformPoint(targetPosition);
        angle = Mathf.Atan2(relative.x, relative.y) * Mathf.Rad2Deg;
        transform.Rotate(0, 0, -angle);
    }
}
