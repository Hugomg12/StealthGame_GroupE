using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public void StartGame()
    {
        // Carga la escena de Gameplay
        SceneManager.LoadScene("Gameplay");
    }
}