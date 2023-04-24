using TMPro;
using UnityEngine;

public class puntuacion : MonoBehaviour
{
    private static puntuacion _instance;
    public static puntuacion Instance { get { return _instance; } }

    public static float puntos = 500;
    [SerializeField] private TMP_Text text; // Texto de ???
    [SerializeField] private GameObject MALAMENTE;
    [SerializeField] private GameObject REGULA;
    [SerializeField] private GameObject ASEPTABLE;
    [SerializeField] private GameObject CRANEOPERSENT;

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
        if (puntuacion.puntos < 500)
        {
            MALAMENTE.SetActive(true);
            REGULA.SetActive(false);
            ASEPTABLE.SetActive(false);
            CRANEOPERSENT.SetActive(false);
        }
        else if (puntuacion.puntos >= 500 || puntuacion.puntos < 600)
        {
            MALAMENTE.SetActive(false);
            REGULA.SetActive(true);
            ASEPTABLE.SetActive(false);
            CRANEOPERSENT.SetActive(false);
        }
        else if (puntuacion.puntos >= 600 || puntuacion.puntos < 900)
        {
            MALAMENTE.SetActive(false);
            REGULA.SetActive(false);
            ASEPTABLE.SetActive(true);
            CRANEOPERSENT.SetActive(false);
        }
        else if (puntuacion.puntos >= 900 || puntuacion.puntos < 1000)
        {
            MALAMENTE.SetActive(false);
            REGULA.SetActive(false);
            ASEPTABLE.SetActive(false);
            CRANEOPERSENT.SetActive(true);
        }
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
