using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region references
    [SerializeField] private GameObject _StartMenu;
    [SerializeField] private GameObject _HUD;

    // imagenes dentro del ui
    [SerializeField] private Image[] _images;
    // sprites en los assets
    [SerializeField] private Sprite[] _sprites;
    #endregion

    #region properties
    private GameManager.GameStates _activeMenu;          // Men� actual
    private GameObject[] _menus;                         // Array de men�s totales
    #endregion

    public void SetMenu(GameManager.GameStates newMenu)  // Desactiva el men� anterior, actualiza el actual y lo activa
    {
        _menus[(int)_activeMenu].SetActive(false);
        _activeMenu = newMenu;
        _menus[(int)_activeMenu].SetActive(true);
    }

    public void SetUpGameHUD(PlayerManager.TimmyStates state) // Inicializa el HUD
    {
        switch (state)
        {
            case PlayerManager.TimmyStates.S0: // todo
                _images[0].sprite = _sprites[1];
                _images[1].sprite = _sprites[3];
                _images[2].sprite = _sprites[5];
                break;
            case PlayerManager.TimmyStates.S1: // 1 brazo y piernas
                _images[0].sprite = _sprites[1];
                _images[1].sprite = _sprites[2];
                _images[2].sprite = _sprites[5];
                break;
            case PlayerManager.TimmyStates.S2: // piernas
                _images[0].sprite = _sprites[1];
                _images[1].sprite = _sprites[2];
                _images[2].sprite = _sprites[5];
                break;
            case PlayerManager.TimmyStates.S3: // dos brazos
                _images[0].sprite = _sprites[1];
                _images[1].sprite = _sprites[3];
                _images[2].sprite = _sprites[4];
                break;
            case PlayerManager.TimmyStates.S4: // un brazo
                _images[0].sprite = _sprites[1];
                _images[1].sprite = _sprites[2];
                _images[2].sprite = _sprites[4];
                break;
            case PlayerManager.TimmyStates.S5: // nada
                _images[0].sprite = _sprites[1];
                _images[1].sprite = _sprites[2];
                _images[2].sprite = _sprites[4];
                break;
        }
    }

    public void UpdateGameHUD(PlayerManager.TimmyStates state) // Actualiza en cada frame los datos del HUD
    {
        switch (state)
        {
            case PlayerManager.TimmyStates.S0:
                
                break;
            case PlayerManager.TimmyStates.S1:
                
                break;
            case PlayerManager.TimmyStates.S2:
                
                break;
            case PlayerManager.TimmyStates.S3:
                
                break;
            case PlayerManager.TimmyStates.S4:
                
                break;
            case PlayerManager.TimmyStates.S5:
                
                break;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _menus = new GameObject[4]; // creaci�n del array de men�s y asignaci�n
        //_menus[0] = _StartMenu;
        _menus[1] = _HUD; // habr� que poner m�s segun a�adamos menuses
        _activeMenu = GameManager.Instance.CurrentState; // asocia el men� actual con el estado actual
        GameManager.Instance.RegisterUIManager(this);
        PlayerManager.Instance.RegisterUIManager(this);
    }

    // Update is called once per frame
    void Update()
    {
        // aqu� no deber�a haber nada pero lo dejo porsiaca
    }
}
