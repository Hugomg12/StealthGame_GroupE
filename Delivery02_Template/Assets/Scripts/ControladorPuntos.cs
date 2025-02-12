using UnityEngine;

public class ControladorPuntos : MonoBehaviour
{
   public static ControladorPuntos Instance;  // Instancia �nica

    public float puntos;  // Variable para almacenar los puntos

    private void Awake()
    {
        // Comprobamos si ya existe una instancia
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Mantener este objeto entre escenas
        }
        else
        {
            Destroy(gameObject);  // Si ya existe, destruir este objeto
        }
    }

    // M�todo para actualizar los puntos
    public void UpdatePuntos(float puntos)
    {
        this.puntos += puntos;
    }

}
