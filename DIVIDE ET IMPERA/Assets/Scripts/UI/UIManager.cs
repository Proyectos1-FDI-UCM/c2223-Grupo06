using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region references
    [SerializeField] private GameObject _StartMenu;
    [SerializeField] private GameObject _HUD;
    [SerializeField] private GameObject _PauseMenu;
    [SerializeField] private GameObject _GameOverMenu;
    [SerializeField] private GameObject _player;

    // imagenes dentro del ui
    [SerializeField] private Image[] _images;
    // sprites en los assets
    [SerializeField] private Sprite[] _sprites;

    private static UIManager _instance;
    public static UIManager Instance { get { return _instance; } }
    #endregion

    #region properties
    private GameManager.GameStates _activeMenu;          // Menú actual
    private GameObject[] _menus;                         // Array de menús totales

    private int _posCabeza;
    private int _posBrazo1;
    private int _posBrazo2;
    private int _posPiernas;
    private int _posAlubiat;
    private int _posCostillas;

    private GameObject _thingInControl;
    #endregion

    // MENUS
    public void SetMenu(GameManager.GameStates newMenu)  // Desactiva el menú anterior, actualiza el actual y lo activa
    {
        _menus[(int)_activeMenu].SetActive(false);
        _activeMenu = newMenu;
        _menus[(int)_activeMenu].SetActive(true);
    }

    /*
    public void UpdateMenu()
    {
        if(_activeMenu == GameManager.GameStates.START)
        {
            _selectedMenu = _game;
            SetMenu(GameManager.GameStates.GAME);
        }
        else if(_activeMenu == GameManager.GameStates.GAME)
        {
            
        }

    }
    */

    public void StartToGame()
    {
        SetMenu(GameManager.GameStates.GAME);

        // activa el input
        _player.GetComponent<InputController>().enabled = true;
    }

    public void PauseToStart()
    {
        SetMenu(GameManager.GameStates.START);

        // activa el input
        _player.GetComponent<InputController>().enabled = true;
    }

    public void PauseToGame()
    {
        SetMenu(GameManager.GameStates.GAME);

        if (_thingInControl != _player)
        {
            _thingInControl.GetComponent<PataformaComponent>()._activarPataforma= true;
        }
        else
            _player.GetComponent<InputController>().enabled = true;
    }

    public void Quit()
    {
        Debug.Log("shipit");
        Application.Quit();
    }

    // PARTES
    public void SetPartes(PlayerManager.TimmyStates state, PlayerManager.Partes parte) // Inicializa el HUD
    {
        bool cabeza = true;
        bool brazo1 = false;
        bool brazo2 = false;
        bool piernas = false;

        // Sección de activar parte principal (la controlada)
        // esto está work in progress, estoy probando cosas
        if (parte == PlayerManager.Partes.PIERNAS) // si está controlando las piernas, no está controlando nada más
        {
            cabeza = false;
            brazo1 = false;
            brazo2 = false;
            piernas = true;
        }
        else // si la parte ppal es cabeza o brazos
        { // (brazos solo sale en activo si están sueltos si las palancas se estan activando/desactivando
            switch (state)
            {
                case PlayerManager.TimmyStates.S0: // todo
                    brazo1 = true;
                    brazo2 = true;
                    piernas = true;
                    break;
                case PlayerManager.TimmyStates.S1: // 1 brazo y piernas
                    brazo1 = true;
                    brazo2 = false;
                    piernas = true;
                    break;
                case PlayerManager.TimmyStates.S2: // piernas
                    brazo1 = false;
                    brazo2 = false;
                    piernas = true;
                    break;
                case PlayerManager.TimmyStates.S3: // dos brazos
                    brazo1 = true;
                    brazo2 = true;
                    piernas = false;
                    break;
                case PlayerManager.TimmyStates.S4: // un brazo
                    brazo1 = true;
                    brazo2 = false;
                    piernas = false;
                    break;
                case PlayerManager.TimmyStates.S5: // nada
                    brazo1 = false;
                    brazo2 = false;
                    piernas = false;
                    break;
            }

            if (parte == PlayerManager.Partes.BRAZO1) { brazo1 = true; } // para cuando animacion de palanca
            else if (parte == PlayerManager.Partes.BRAZO2) { brazo2 = true; } // para cuando animación de palanca
        }

        // +1 SI EN ACTIVO, NADA SI INACTIVO
        _images[_posCabeza].sprite = _sprites[_posCabeza * 2 + (cabeza ? 1 : 0)];
        _images[_posBrazo1].sprite = _sprites[_posBrazo1 * 2 + (brazo1 ? 1 : 0)];
        _images[_posBrazo2].sprite = _sprites[_posBrazo2 * 2 + (brazo2 ? 1 : 0)];
        _images[_posPiernas].sprite = _sprites[_posPiernas * 2 + (piernas ? 1 : 0)];
    }

    // OBJETOS
    public void SetObject(PlayerManager.Objetos objeto) // no necesita de un metodo reset porque .NADA es 3
    {
        _images[_posCostillas].sprite = _sprites[_posCostillas * 2 + (int)objeto];
    }

    // ALUBIAT
    public bool TieneAlubiat() // si está el sprite de alubiat (no está vacio)
    {
        if (_images[_posAlubiat].sprite == _sprites[_posAlubiat * 2] || _images[_posAlubiat].sprite == _sprites[_posAlubiat * 2 + 1])
        {
            return true;
        }
        else return false;
    }

    public void SetAlubiat(bool activo) // asigna el sprite de alubiat según esté a activo o no
    {
        _images[_posAlubiat].sprite = _sprites[_posAlubiat * 2 + (activo ? 1 : 0)];
    }

    public void ResetAlubiat() // lo resetea a vacío
    {
        _images[_posAlubiat].sprite = _sprites[^1]; // el último sprite es el vacío
    }

    public void ResetLevel()
    {
        GameManager.Instance.DemoReset();
    }

    public void ChangeObjectInControl(GameObject thing)
    {
        _thingInControl= thing;
    }
    // BUCLE
    void Awake()
    {
        _instance= this;
    }

    void Start()
    {
        _menus = new GameObject[4]; // creación del array de menús y asignación
        _menus[0] = _StartMenu;
        _menus[1] = _HUD;
        _menus[2] = _PauseMenu;
        _menus[3] = _GameOverMenu; // habrá que poner más segun añadamos menuses
        _activeMenu = GameManager.Instance.CurrentState; // asocia el menú actual con el estado actual

        _posCabeza = 0; // posiciones concretas de cada parte en el array de imágenes
        _posBrazo1 = 1;
        _posBrazo2 = 2;
        _posPiernas = 3;
        _posAlubiat = 4;
        _posCostillas = 5;

        GameManager.Instance.RegisterUIManager(this);
        PlayerManager.Instance.RegisterUIManager(this);

        _thingInControl = _player;
    }

    /*
    void Update()
    {
        // aquí no debería haber nada pero lo dejo porsiaca
    }
    */
}
