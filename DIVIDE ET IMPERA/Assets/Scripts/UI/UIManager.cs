using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region references 
    // menús
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
    [SerializeField] private GameObject _firstButton; // botón inicial del menú pausa
    public GameObject FirstButton { get { return _firstButton; } set { _firstButton = value; } }
    // array de botones iniciales por escenas NO REIRSE DE MI >:(
    [SerializeField] private GameObject[] _firstButtons;
    // 0 inicial, 1 intro, 2 game, 3 pausa, 4 gameover, 5 puntuación,
    // 6 selector de niveles, 7  controles, 8 opciones, 9 credits
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
    [SerializeField] private GameObject _timeObject; // objeto tiempo en hud para toggle
    [SerializeField] private TMP_Text _timeText; // texto tiempo en hud
    [SerializeField] private Toggle _timeToggle; // toggle de ver tiempo en opciones
    [SerializeField] private TMP_Text _scoreHUDText; // score en hud
    [SerializeField] private TMP_Text _scoreMenuPoints; // puntos en menú final
    [SerializeField] private TMP_Text _scoreMenuText; // mensaje final
    [SerializeField] private GameObject[] endings; // imágenes del final
    [SerializeField] private string[] messages; // mensajes según el final
    // 0 TERRIBLISIMO, 1 MALAMENTE, 2 REGULA, 3 ASEPTABLE, 4 CRANEOPERSENT
    #endregion

    #region properties
    public GameManager.GameStates _activeMenu;          // Menú actual
    private GameObject[] _menus;                         // Array de menús totales

    private int _posCabeza;
    private int _posBrazo1;
    private int _posBrazo2;
    private int _posPiernas;
    private int _posAlubiat;
    private int _posCostillas;

    private float _bgmVolumeValue;
    private float _sfxVolumeValue;
    private float _ambienceVolumeValue;
    #endregion

    public void RequestStateChange(GameManager.GameStates newState)
    {
        if (GameManager.Instance != null)
            GameManager.Instance.RequestStateChange(newState);
    }

    // MENUS
    public bool SetMenu(GameManager.GameStates newMenu)  // Desactiva el menú anterior, actualiza el actual y lo activa
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
            EventSystem.current.SetSelectedGameObject(_firstButton); // cambia el botón seleccionado
    }

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
    { // Actúa tanto como SetUp y Update
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
        RequestStateChange(GameManager.GameStates.GAME);
        PlayerAccess.Instance.InputController.enabled = true;
        PlayerAccess.Instance.MovementComponent.enabled = true;
        PlayerAccess.Instance.Animator.enabled = true;
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

    public void ScoreMenuSetUp(int score, int ending) // score en el menú final
    {
        _scoreMenuPoints.text = "Puntos: " + score;
        _scoreMenuText.text = messages[ending];
        Debug.Log("FINAL: " + ending + ", SCORE: " + ending + ", MENSAJE: " + messages[ending]);
        for (int i = 0; i < endings.Length; i++) // desactiva el que no sea y activa el que sea
        {
            if (i == ending)
                endings[i].SetActive(true);
            else
                endings[i].SetActive(false);
        }
    }

    public void TimeToggle()
    {
        _timeObject.SetActive(!_timeObject.activeSelf);
    }
    #endregion

    public void SetOptionsSliders()
    {
        if (AudioManager.Instance != null)
        {
            // guarda los valores
            _bgmVolumeValue = AudioManager.Instance.SetSliderValue(0);
            _sfxVolumeValue = AudioManager.Instance.SetSliderValue(1);
            _ambienceVolumeValue = AudioManager.Instance.SetSliderValue(2);

            // los resetea
            AudioManager.Instance.SetUpAllVolumes();

            // pone el valor correcto
            _sliderBGM.value = _bgmVolumeValue;
            _sliderSFX.value = _sfxVolumeValue;
            _sliderAmbience.value = _ambienceVolumeValue;
        }
    }

    // BUCLE
    void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        _menus = new GameObject[10];    // creación del array de menús y asignación
                                        // *DEBERÍAN SEGUÍR EL ORDEN Y TAMAÑO DE LOS ESTADOS DEL GAMEMANAGER*
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
        // habrá que poner más segun añadamos menuses
        if (GameManager.Instance != null) _activeMenu = GameManager.Instance.CurrentState; // asocia el menú actual con el estado actual

        // HUD
        _posCabeza = 0; // posiciones concretas de cada parte en el array de imágenes
        _posBrazo1 = 1;
        _posBrazo2 = 2;
        _posPiernas = 3;
        _posAlubiat = 4;
        _posCostillas = 5;

        // REGISTROS
        if (GameManager.Instance != null) GameManager.Instance.RegisterUIManager(this);
        if (PlayerManager.Instance != null) PlayerManager.Instance.RegisterUIManager(this);

        // setea el tiempo en hud y el toggle view time a lo que sea en el gamemanager
        if (GameManager.Instance != null)
        {
            _timeObject.SetActive(GameManager.Instance.ViewTime);
            
        }

        // setea los sliders WIP 
        //SetOptioinsSliders();
    }

    /*
    void Update()
    {
        // aquí no debería haber nada pero lo dejo porsiaca
    }
    */
}
