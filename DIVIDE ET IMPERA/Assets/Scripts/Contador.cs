using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Contador : MonoBehaviour
{

    private static float tiempo = 0;
    [SerializeField] private static TextMeshProUGUI time;

    private void Start()
    {
        time = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        Comienza(UIManager.Instance._activeMenu);
    }

    public static void Comienza(GameManager.GameStates estado)
    {
        if (estado == GameManager.GameStates.START)
        {
            tiempo = 0;
        }
        else if (estado == GameManager.GameStates.GAME) 
        { 
            tiempo += Time.deltaTime;
            time.text = tiempo.ToString("0");
        }

    }
}
