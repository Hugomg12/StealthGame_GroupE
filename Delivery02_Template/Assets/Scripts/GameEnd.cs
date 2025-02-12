using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;


public class GameEnd : MonoBehaviour
{
    public Text distanceText;  // El Text donde mostrar la distancia

    void Start()
    {
        // Asegurarse de que ControladorPuntos.Instance no sea null
        if (ControladorPuntos.Instance != null)
        {
            // Acceder al valor de puntos acumulados desde ControladorPuntos
            float puntos = ControladorPuntos.Instance.puntos;
            if (puntos > 999)
            {
                distanceText.text = "Travelled: +999 units";
            }
            else
            {
                // Mostrar los puntos acumulados en el texto
                distanceText.text = "Travelled: " + ((int)puntos).ToString() + " units";
            }
        }
     
    }
}
