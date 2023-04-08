using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class puntuacion : MonoBehaviour
{
    private float puntos;
    private TextMeshProUGUI text;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        puntos += Time.deltaTime;
        text.text = puntos.ToString("0");
    }

    public void SumaPuntos(float puntosSumar)
    {
        puntos += puntosSumar;
    }

    public void RestaPuntos(float puntosRestar)
    {
        puntos += puntosRestar;
    }

}
