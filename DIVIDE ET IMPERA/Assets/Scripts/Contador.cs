using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Contador : MonoBehaviour
{
    private float tiempo = 0;
    [SerializeField] private TextMeshProUGUI time;

    private void Start()
    {
        time = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        tiempo += Time.deltaTime;
        time.text = tiempo.ToString("0");
    }
}
