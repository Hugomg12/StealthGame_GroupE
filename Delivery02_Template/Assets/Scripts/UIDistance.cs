
using UnityEngine;  // Esta l�nea es esencial
using UnityEngine.UI; // Aseg�rate de que tambi�n est� incluida si est�s trabajando con UI

public class UIDistance : MonoBehaviour
{
    public static UIDistance Instance;  // Instancia del Singleton
    private Text locura;
    private float totalDistance;  // Variable para almacenar la distancia total

    private void Awake()
    {
       
    }

    private void Start()
    {
        locura = GetComponent<Text>();  // Obtener el componente Text
    }

    private void OnEnable()
    {
        PlayerMovement.OnPlayerMoved += UpdateDistance;
    }

    private void OnDisable()
    {
        PlayerMovement.OnPlayerMoved -= UpdateDistance;
    }

    private void UpdateDistance(float distance)
    {
        totalDistance += distance;  // Sumar la distancia a la distancia total

        if (totalDistance > 999)
        {
            locura.text = "Distance: +999 units";  // Mostrar un l�mite si la distancia supera los 999
        }
        else
        {
            locura.text = "Distance: " + ((int)totalDistance).ToString() + " units";  // Mostrar la distancia con formato
        }
    }

    // M�todo para acceder a la distancia total
    public float GetTotalDistance()
    {
        return totalDistance;
    }

    // M�todo para asociar el texto desde otra escena
    public void SetTextObject(Text newText)
    {
        locura = newText;  // Asignar el nuevo objeto de texto
    }
}
