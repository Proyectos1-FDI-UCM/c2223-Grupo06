using TMPro;
using UnityEngine;

public class Contador : MonoBehaviour
{

    public static float tiempo = 0;
    [SerializeField] private static TMP_Text time;

    private void Start()
    {
        time = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        //Comienza(UIManager.Instance._activeMenu);
    }

    public static void Comienza(GameManager.GameStates estado)
    {
        if (estado == GameManager.GameStates.GAME)
        {
            tiempo += Time.deltaTime;
            time.text = tiempo.ToString("0");
            Debug.Log("puñeta");
        }

    }
}
