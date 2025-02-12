using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class UITime : MonoBehaviour
{
    private Text scriu;

    private void Start()
    {
        scriu = GetComponent<Text>();
    }

    private void OnEnable()
    {
        PlayerMovement.OnTimeUpdated += UpdateTime;
    }

    private void OnDisable()
    {
        PlayerMovement.OnTimeUpdated -= UpdateTime;
    }

    private void UpdateTime(float totalTime)
    {
        if (totalTime > 999)
        {
            scriu.text = "Time: +999 seconds";
        }
        else
        {
            scriu.text = "Time: " + ((int)totalTime).ToString() + " sec.";
        }
    }
}
