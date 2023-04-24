using TMPro;
using UnityEngine;

public class Puntuacion : MonoBehaviour
{
    private static Puntuacion _instance;
    public static Puntuacion Instance { get { return _instance; } }

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
        //text = GetComponent<TextMeshProUGUI>(); esto no creo que haga falta
    }

    private void Update()
    {
        text.text = puntos.ToString(" ");
        Debug.Log("PUNTOS: " + puntos + ", text.text: " + text.text + ", text: "+ text);
        if (puntos < 500)
        {
            MALAMENTE.SetActive(true);
            REGULA.SetActive(false);
            ASEPTABLE.SetActive(false);
            CRANEOPERSENT.SetActive(false);
        }
        else if (puntos >= 500 || puntos < 600)
        {
            MALAMENTE.SetActive(false);
            REGULA.SetActive(true);
            ASEPTABLE.SetActive(false);
            CRANEOPERSENT.SetActive(false);
        }
        else if (puntos >= 600 || puntos < 900)
        {
            MALAMENTE.SetActive(false);
            REGULA.SetActive(false);
            ASEPTABLE.SetActive(true);
            CRANEOPERSENT.SetActive(false);
        }
        else if (puntos >= 900 || puntos < 1000)
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
