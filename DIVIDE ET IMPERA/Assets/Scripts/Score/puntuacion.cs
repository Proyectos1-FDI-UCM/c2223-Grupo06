using TMPro;
using UnityEngine;

public class Puntuacion : MonoBehaviour
{
    private static Puntuacion _instance;
    public static Puntuacion Instance { get { return _instance; } }

    public static float puntos = 500;
    [SerializeField] private TextMeshProUGUI text;

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        text.text = puntos.ToString(" ");
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
