using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField] private GameObject MALAMENTE;
    [SerializeField] private GameObject REGULA;
    [SerializeField] private GameObject ASEPTABLE;
    [SerializeField] private GameObject CRANEOPERSENT;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
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
}
