using TMPro;
using UnityEngine;

public class Contador : MonoBehaviour
{

    private static Contador _instance;
    public static Contador Instance { get { return _instance; } }

    public static float tiempo = 0;
    [SerializeField] public static TextMeshProUGUI time;

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
        if (estado == GameManager.GameStates.GAME)
        {
            tiempo += Time.deltaTime;
            time.text = tiempo.ToString("0");
        }

    }
}
