using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    #region references
    [SerializeField] private GameObject _StartMenu;
    [SerializeField] private GameObject _HUD;
    [SerializeField] private GameObject _PauseMenu;
    [SerializeField] private GameObject _GameOverMenu;
    [SerializeField] private GameObject _scoreMenu;
    [SerializeField] private GameObject _levelSelector;
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _ControlesMenu;
    [SerializeField] private GameObject _optionsMenu;

    // imagenes dentro del ui
    [SerializeField] private Image[] _images;
    // sprites en los assets
    [SerializeField] private Sprite[] _sprites;

    private static UIManager _instance;
    public static UIManager Instance { get { return _instance; } }

    // EVENT SYSTEM -> input de teclado para menus
    [SerializeField] private GameObject _pauseFirstButton;
    [SerializeField] private GameObject _startFirstButton;
    [SerializeField] private GameObject _startClosedButton;
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
    #endregion

    public void RequestStateChange(GameManager.GameStates newState)
    {
        GameManager.Instance.RequestStateChange(newState);
    }

    // MENUS
    public void SetMenu(GameManager.GameStates newMenu)  // Desactiva el menú anterior, actualiza el actual y lo activa
    {
        _menus[(int)_activeMenu].SetActive(false);
        _activeMenu = newMenu;
        _menus[(int)_activeMenu].SetActive(true);
    }


    /* public void UpdateMenu() // esto lo usariamos si se cambia informacion in real time (SEGURAMENTE CUANDO SPEEDRUNNING CON EL TEMA CRONÓMETRO!!)
    {
        if(_activeMenu == GameManager.GameStates.START)
        {
            _selectedMenu = _game;
            SetMenu(GameManager.GameStates.GAME);
        }
        else if(_activeMenu == GameManager.GameStates.GAME)
        {
            
        }
    } */

    public void StartToGame() // menu iniacial -> juego (empezar a jugar)
    {
        if (LevelManager.Instance != null) ResetRoom();
        RequestStateChange(GameManager.GameStates.GAME); // referenciando al gamemanager (importante! si no no cambia de estado)

        // activa el input
        _player.GetComponent<InputController>().enabled = true;
        CameraMovement.Instance.enabled = true;
    }

    public void ResumeGame() // menu de pausa -> juego (reanudar)
    {
        RequestStateChange(GameManager.GameStates.GAME); // referenciando al gamemanager (importante! si no no cambia de estado)

        // activa el input
        _player.GetComponent<InputController>().enabled = true;
    }

    public void PauseToStart() // menu de pausa -> menu inicial
    {
        RequestStateChange(GameManager.GameStates.START); // referenciando al gamemanager (importante! si no no cambia de estado)

        // activa el input
        _player.GetComponent<InputController>().enabled = true;
    }

    public void PauseToControles() // menu pausa -> controles
    {
        RequestStateChange(GameManager.GameStates.OPCIONES); // referenciando al gamemanager (importante! si no no cambia de estado)
        _player.GetComponent<InputController>().enabled = false;
    }

    public void ControlesToPause() // controles -> menu pausa
    {
        RequestStateChange(GameManager.GameStates.PAUSE); // referenciando al gamemanager (importante! si no no cambia de estado)
        _player.GetComponent<InputController>().enabled = false;
    }

    public void PauseToOptions()
    {
        RequestStateChange(GameManager.GameStates.OPCIONES); // referenciando al gamemanager (importante! si no no cambia de estado)
        _player.GetComponent<InputController>().enabled = false;
    }

    public void OptionsToPause()
    {
        RequestStateChange(GameManager.GameStates.PAUSE); // referenciando al gamemanager (importante! si no no cambia de estado)
        _player.GetComponent<InputController>().enabled = false;
    }

    public void GoToScore() // a las puntuaciones
    {
        RequestStateChange(GameManager.GameStates.SCORE); // referenciando al gamemanager (importante! si no no cambia de estado)
        _player.GetComponent<InputController>().enabled = false;
    }

    public void GoToLevelSelector() // al selector de niveles
    {
        RequestStateChange(GameManager.GameStates.LEVELSELECTOR); // referenciando al gamemanager (importante! si no no cambia de estado)
        _player.GetComponent<InputController>().enabled = false;
    }

    public void PauseToGame() // menu pausa -> juego
    {
        RequestStateChange(GameManager.GameStates.GAME);

        if (PlayerManager.Instance._partInControl != _player)
        {
            //PlayerManager.Instance._objectInControl.GetComponent<PataformaComponent>()._activarPataforma= true;
            PlayerManager.Instance._partInControl.GetComponent<PataformaComponent>().enabled = true;
            PlayerManager.Instance._partInControl.GetComponent<PataformaMovementComponent>().enabled = true;
            PlayerManager.Instance._partInControl.GetComponentInChildren<Animator>().enabled = true;
        }
        else
        {
            PlayerAccess.Instance.InputController.enabled = true;
            PlayerAccess.Instance.MovementComponent.enabled = true;
            PlayerAccess.Instance.Animator.enabled = true;
        }
    }

    public void Quit()
    {
        Debug.Log("shipit");
        Application.Quit();
    }

    #region HUD
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
    #endregion

    #region RESET
    public void ResetRoom()
    {
        LevelManager.Instance.ResetCurrentLevel(); //Resetea sala actual
    }

    public void GlobalReset()
    {
        LevelManager.Instance.GlobalReset(); //Resetea todo el nivel
    }
    #endregion

    // BUCLE
    void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        _menus = new GameObject[8]; // creación del array de menús y asignación
        _menus[0] = _StartMenu;
        _menus[1] = _HUD;
        _menus[2] = _PauseMenu;
        _menus[3] = _GameOverMenu;
        _menus[4] = _scoreMenu;
        _menus[5] = _levelSelector;
        _menus[6] = _ControlesMenu;
        _menus[7] = _optionsMenu;
        // habrá que poner más segun añadamos menuses
        _activeMenu = GameManager.Instance.CurrentState; // asocia el menú actual con el estado actual

        _posCabeza = 0; // posiciones concretas de cada parte en el array de imágenes
        _posBrazo1 = 1;
        _posBrazo2 = 2;
        _posPiernas = 3;
        _posAlubiat = 4;
        _posCostillas = 5;

        GameManager.Instance.RegisterUIManager(this);
        PlayerManager.Instance.RegisterUIManager(this);
    }

    /*
    void Update()
    {
        // aquí no debería haber nada pero lo dejo porsiaca
    }
    */
}
