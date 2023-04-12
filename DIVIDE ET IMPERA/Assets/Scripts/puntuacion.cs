using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class puntuacion : MonoBehaviour
{
    private static float puntos = 500;
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
