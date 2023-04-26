using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region references 
    // men�s
    [SerializeField] private GameObject _StartMenu;
    [SerializeField] private GameObject _IntroMenu;
    [SerializeField] private GameObject _HUD;
    [SerializeField] private GameObject _PauseMenu;
    [SerializeField] private GameObject _GameOverMenu;
    [SerializeField] private GameObject _scoreMenu;
    [SerializeField] private GameObject _levelSelector;
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _ControlesMenu;
    [SerializeField] private GameObject _optionsMenu;
    [SerializeField] private GameObject _credits;

    // instancia
    private static UIManager _instance;
    public static UIManager Instance { get { return _instance; } }

    // EVENT SYSTEM -> input de teclado para menus
    [SerializeField] private GameObject _firstButton; // bot�n inicial del men� pausa
    public GameObject FirstButton { get { return _firstButton; } set { _firstButton = value; } }
    // array de botones iniciales por escenas NO REIRSE DE MI >:(
    [SerializeField] private GameObject[] _firstButtons;
    // 0 inicial, 1 intro, 2 game, 3 pausa, 4 gameover,
    // 5 puntuaci�n, 6 selector de niveles, 7  controles, 8 opciones, 9 credits
    public GameObject[] FirstButtons { get { return _firstButtons; } }

    /// HUD
    [SerializeField] private Image[] _images;    // imagenes dentro del ui
    [SerializeField] private Sprite[] _sprites;    // sprites en los assets
    //imagenes tarjetas puntos
    //[SerializeField] public 

    // Sliders de menu de opciones
    [SerializeField] private Slider _sliderBGM;
    [SerializeField] private Slider _sliderSFX;
    [SerializeField] private Slider _sliderAmbience;

    // time, score text y finales
    [SerializeField] private TMP_Text _timeText;
    [SerializeField] private TMP_Text _scoreHUDText; // score en hud
    [SerializeField] private TMP_Text _scoreMenuText;
    [SerializeField] private GameObject[] endings;
    #endregion

    #region properties
    public GameManager.GameStates _activeMenu;          // Men� actual
    private GameObject[] _menus;                         // Array de men�s totales

    private int _posCabeza;
    private int _posBrazo1;
    private int _posBrazo2;
    private int _posPiernas;
    private int _posAlubiat;
    private int _posCostillas;
    #endregion

    public void RequestStateChange(GameManager.GameStates newState)
    {
        if (GameManager.Instance != null)
            GameManager.Instance.RequestStateChange(newState);
    }

    // MENUS
    public bool SetMenu(GameManager.GameStates newMenu)  // Desactiva el men� anterior, actualiza el actual y lo activa
    {
        _menus[(int)_activeMenu].SetActive(false);
        _activeMenu = newMenu;
        _menus[(int)_activeMenu].SetActive(true);
        return _menus[(int)_activeMenu].activeSelf;
    }

    public void SetFirstButton(int index)
    {
        _firstButton = _firstButtons[index];
        if (_firstButton != null && EventSystem.current != null)
            EventSystem.current.SetSelectedGameObject(_firstButton); // cambia el bot�n seleccionado
    }

    /* public void UpdateMenu() // esto lo usariamos si se cambia informacion in real time (SEGURAMENTE CUANDO SPEEDRUNNING CON EL TEMA CRON�METRO!!)
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

    #region STATE MOVEMENT
    public void StartToIntro()      // menu iniacial -> intro (empezar a jugar)
    {
        //if (LevelManager.Instance != null) ResetRoom();
        RequestStateChange(GameManager.GameStates.INTRO); // referenciando al gamemanager (importante! si no no cambia de estado)

        // activa el input
        _player.GetComponent<InputController>().enabled = true;
        CameraMovement.Instance.enabled = true;
    }

    public void IntroToGame()       // intro -> juego
    {
        RequestStateChange(GameManager.GameStates.GAME); // referenciando al gamemanager (importante! si no no cambia de estado)

        _player.GetComponent<InputController>().enabled = true;
    }

    public void ResumeGame()        // menu de pausa -> juego (reanudar)
    {
        RequestStateChange(GameManager.GameStates.GAME); // referenciando al gamemanager (importante! si no no cambia de estado)
        _player.GetComponent<InputController>().enabled = true;
    }

    public void PauseToStart()      // menu de pausa -> menu inicial
    {
        RequestStateChange(GameManager.GameStates.START); // referenciando al gamemanager (importante! si no no cambia de estado)
        _player.GetComponent<InputController>().enabled = true;
    }

    public void PauseToControles()  // menu pausa -> controles
    {
        RequestStateChange(GameManager.GameStates.CONTROLES); // referenciando al gamemanager (importante! si no no cambia de estado)
        _player.GetComponent<InputController>().enabled = false;
    }

    public void ControlesToPause()  // controles -> menu pausa
    {
        RequestStateChange(GameManager.GameStates.PAUSE); // referenciando al gamemanager (importante! si no no cambia de estado)
        _player.GetComponent<InputController>().enabled = false;
    }

    public void PauseToOptions()    // pause -> opciones
    {
        RequestStateChange(GameManager.GameStates.OPCIONES); // referenciando al gamemanager (importante! si no no cambia de estado)
        _player.GetComponent<InputController>().enabled = false;
    }

    public void OptionsToPause()    // opciones -> pausa
    {
        RequestStateChange(GameManager.GameStates.PAUSE); // referenciando al gamemanager (importante! si no no cambia de estado)
        _player.GetComponent<InputController>().enabled = false;
    }

    public void GoToScore()         // a las puntuaciones
    {
        RequestStateChange(GameManager.GameStates.SCORE); // referenciando al gamemanager (importante! si no no cambia de estado)
        _player.GetComponent<InputController>().enabled = false;

    }

    public void GoToLevelSelector() // al selector de niveles
    {
        RequestStateChange(GameManager.GameStates.LEVELSELECTOR); // referenciando al gamemanager (importante! si no no cambia de estado)
        _player.GetComponent<InputController>().enabled = false;
    }

    public void GoToCredits()       // a creditos
    {
        RequestStateChange(GameManager.GameStates.CREDITS); // referenciando al gamemanager (importante! si no no cambia de estado)
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
    #endregion

    public void Quit()
    {
        Debug.Log("shipit");
        Application.Quit();
    }

    #region HUD
    // PARTES
    public void SetPartes(PlayerManager.TimmyStates state, PlayerManager.Partes parte) // Inicializa el HUD
    { // Act�a tanto como SetUp y Update
        bool cabeza = true;
        bool brazo1 = false;
        bool brazo2 = false;
        bool piernas = false;

        // Secci�n de activar parte principal (la controlada)
        // esto est� work in progress, estoy probando cosas
        if (parte == PlayerManager.Partes.PIERNAS) // si est� controlando las piernas, no est� controlando nada m�s
        {
            cabeza = false;
            brazo1 = false;
            brazo2 = false;
            piernas = true;
        }
        else // si la parte ppal es cabeza o brazos
        { // (brazos solo sale en activo si est�n sueltos si las palancas se estan activando/desactivando
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
            else if (parte == PlayerManager.Partes.BRAZO2) { brazo2 = true; } // para cuando animaci�n de palanca
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
    public bool TieneAlubiat() // si est� el sprite de alubiat (no est� vacio)
    {
        if (_images[_posAlubiat].sprite == _sprites[_posAlubiat * 2] || _images[_posAlubiat].sprite == _sprites[_posAlubiat * 2 + 1])
        {
            return true;
        }
        else return false;
    }

    public void SetAlubiat(bool activo) // asigna el sprite de alubiat seg�n est� a activo o no
    {
        _images[_posAlubiat].sprite = _sprites[_posAlubiat * 2 + (activo ? 1 : 0)];
    }

    public void ResetAlubiat() // lo resetea a vac�o
    {
        _images[_posAlubiat].sprite = _sprites[^1]; // el �ltimo sprite es el vac�o
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

    #region TIME & SCORE
    public void TimeUpdate(float value)
    {
        _timeText.text = "Tiempo: " + (int)value;
    }
    public void ScoreSetUp(int score) // score en el HUD
    {
        _scoreHUDText.text = "Puntos: " + score;
    }

    public void ScoreMenuSetUp(int score, int ending) // score en el men� final
    {
        _scoreMenuText.text = "Puntos: " + score;
        Debug.Log("FINAL: " + score + ", " + ending);
        for (int i = 0; i < endings.Length; i++) // desactiva el que no sea y activa el que sea
        {
            if (i == ending)
                endings[i].SetActive(true);
            else
                endings[i].SetActive(false);
        }
    }
    #endregion

    public void SetOptioinsSliders()
    {
        if (AudioManager.Instance != null)
        {
            _sliderBGM.value = AudioManager.Instance.SetSliderValue(0);
            _sliderSFX.value = AudioManager.Instance.SetSliderValue(1);
            _sliderAmbience.value = AudioManager.Instance.SetSliderValue(2);
        }
    }

    // BUCLE
    void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        _menus = new GameObject[10];    // creaci�n del array de men�s y asignaci�n
                                        // *DEBER�AN SEGU�R EL ORDEN Y TAMA�O DE LOS ESTADOS DEL GAMEMANAGER*
        _menus[0] = _StartMenu;         // START
        _menus[1] = _IntroMenu;         // INTRO
        _menus[2] = _HUD;               // GAME
        _menus[3] = _PauseMenu;         // PAUSE
        _menus[4] = _GameOverMenu;      // GAMEOVER
        _menus[5] = _scoreMenu;         // SCORE    
        _menus[6] = _levelSelector;     // LEVELSELECTOR
        _menus[7] = _ControlesMenu;     // CONTROLLES
        _menus[8] = _optionsMenu;       // OPTIONS
        _menus[9] = _credits;           // CREDITOS
        // habr� que poner m�s segun a�adamos menuses
        if (GameManager.Instance != null) _activeMenu = GameManager.Instance.CurrentState; // asocia el men� actual con el estado actual

        // HUD
        _posCabeza = 0; // posiciones concretas de cada parte en el array de im�genes
        _posBrazo1 = 1;
        _posBrazo2 = 2;
        _posPiernas = 3;
        _posAlubiat = 4;
        _posCostillas = 5;

        // REGISTROS
        if (GameManager.Instance != null) GameManager.Instance.RegisterUIManager(this);
        if (PlayerManager.Instance != null) PlayerManager.Instance.RegisterUIManager(this);

        // setea los sliders WIP 
        //SetOptioinsSliders();
    }

    /*
    void Update()
    {
        // aqu� no deber�a haber nada pero lo dejo porsiaca
    }
    */
}
