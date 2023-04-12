using TMPro;
using UnityEngine;

public class puntuacion : MonoBehaviour
{
    private static puntuacion _instance;
    public static puntuacion Instance { get { return _instance; } }

    public static float puntos = 500;
    private TextMeshProUGUI text;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        text.text = puntos.ToString("0");
    }

    public static void SumaPuntos(float puntosSumar)
    {
        puntos += puntosSumar;
    }

    public static void RestaPuntos(float puntosRestar)
    {
        puntos -= puntosRestar;
    }

}
