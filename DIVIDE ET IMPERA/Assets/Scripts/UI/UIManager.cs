using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region references
    [SerializeField] private GameObject _HUD;
    [SerializeField] private Image _cabezaIMG;
    [SerializeField] private Image _brazo1IMG;
    [SerializeField] private Image _brazo2IMG;
    [SerializeField] private Image _piernasIMG;
    [SerializeField] private Image _costillasIMG;
    [SerializeField] private Image _piernasAlubiatIMG;
    [SerializeField] private Scrollbar _barra;
    #endregion

    #region properties
    private GameManager.GameStates _activeMenu;          // Menú actual
    private GameObject[] _menus;                         // Array de menús totales
    #endregion

    public void SetMenu(GameManager.GameStates newMenu)  // Desactiva el menú anterior, actualiza el actual y lo activa
    {
        if (_activeMenu != GameManager.GameStates.GAME && GameManager.Instance.CurrentState != GameManager.GameStates.GAMEOVER)
        // El HUD siempre está excepto en GAMEOVER? (optimizable, es un poco chapuza)
        {
            _menus[(int)_activeMenu].SetActive(false);
        }
        _activeMenu = newMenu;
        _menus[(int)_activeMenu].SetActive(true);
    }

    public void SetUpGameHUD() // Inicializa el HUD
    {

    }

    public void UpdateGameHUD() // Actualiza en cada frame los datos del HUD
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        _menus = new GameObject[1]; // creación del array de menús y asignación
        _menus[0] = _HUD; // habrá que poner más segun añadamos menuses
        _activeMenu = GameManager.Instance.CurrentState; // asocia el menú actual con el estado actual
        GameManager.Instance.RegisterUIManager(this);
    }

    // Update is called once per frame
    void Update()
    {
        // aquí no debería haber nada pero lo dejo porsiaca
    }
}
