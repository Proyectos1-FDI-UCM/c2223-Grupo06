using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class puntuacion : MonoBehaviour
{
    private float puntos;
    private TextMeshPro text;

    private void Start()
    {
        text = GetComponent<TextMeshPro>();
    }

    private void Update()
    {
        puntos += Time.deltaTime;
        text.text = puntos.ToString("0");
    }
}
