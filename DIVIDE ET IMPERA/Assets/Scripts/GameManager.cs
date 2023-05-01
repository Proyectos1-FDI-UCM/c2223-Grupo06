using UnityEngine;
//using Unity.UI;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public enum GameStates { START, INTRO, GAME, PAUSE, GAMEOVER, SCORE, LEVELSELECTOR, CONTROLES, OPCIONES, CREDITS, LOGO };

    #region references
    private UIManager _UIManager;
    private BGMComponent _bGMComponent;
    #endregion

    #region properties
    // GAME STATES
    private static GameManager _instance;
    private GameStates _currentGameState;
    private GameStates _nextGameState;
    public static GameManager Instance { get { return _instance; } }
    public GameStates CurrentState { get { return _currentGameState; } }
    public GameStates NextState { get { return _nextGameState; } }

    // importante para la persistencia: mi querido viejo amigo alubiat
    private bool _alubiat;
    public bool Alubiat { get { return _alubiat; } set { _alubiat = value; } }

    // CONTROL POR TECLADO
    private int _fbIndex; // first button index en el array del uimanager ESTÁN PUESTOS POR ÓRDEN DEL ENUM DE ESTADOS DEL GAMEMANAGER
    public int FbIndex { get { return _fbIndex; } set { _fbIndex = value; } }

    // PUNTUACIÓN
    private int _ending;
        // 0 TERRIBLISIMO, 1 MALAMENTE, 2 REGULA, 3 ASEPTABLE, 4 CRANEOPERSENT
    private int _score;
    public int Score { get { return _score; } set { _score = value; } }

    // TIEMPO
    private float _tiempo;
    public float Tiempo { get { return _tiempo; } }
    private bool _viewTime;
    public bool ViewTime { get { return _viewTime; } set { _viewTime = value; } }

    // RESET COUNTER
    private int _resetCounter;
    public int ResetCounter { get { return _resetCounter; } set { _resetCounter = value; } }

    // REANUDAR
    private int _previousScene; // de la que vienes si aplica, exclusivamente para el botón reanudar de momento
    public int PreviousScene { get { return _previousScene; } set { _previousScene = value; } }
    #endregion

    #region REGISTROS DE REFERENCIAS
    public void RegisterUIManager(UIManager uiManager)
    {
        _UIManager = uiManager;
    }
    #endregion

    #region BLOQUE DE ESTADOS
    public void RequestStateChange(GameStates newState) // Método público para cambiar el valor privado de estado 
    {
        _nextGameState = newState;

        if (_bGMComponent != null) _bGMComponent.CanPlay = true;
    }

    // Bloque de máquina de estados
    public void EnterState(GameStates newState)
    {
        switch (newState) // Diferentes comportamientos según estado al que se entra
        {
            case GameStates.START:                       //     *MENÚ INICIAL*
                Time.timeScale = 1;
                break;

            case GameStates.INTRO:                       //     *INTRO*
                Time.timeScale = 1;
                break;

            case GameStates.GAME:                        //     *JUEGO*
                if (_UIManager != null) // Inicializa valores del HUD
                    _UIManager.SetPartes(PlayerManager.State, PlayerManager.Instance.Parte);
                Time.timeScale = 1;
                break;

            case GameStates.PAUSE:                       //     *PAUSA*
                Time.timeScale = 0;
                break;

            case GameStates.GAMEOVER:                    //     *FIN DEL JUEGO* -> PUNTUACION
                Time.timeScale = 1;

                // tiempo
                if (_tiempo >= 500)
                    AddScore(-150);
                else if (_tiempo < 500 && _tiempo >= 400)
                    AddScore(-100);
                else if (_tiempo < 400 && _tiempo >= 300)
                    AddScore(-50);
                else if (_tiempo < 300 && _tiempo >= 200)
                    AddScore(-25);
                else if (_tiempo < 200 && _tiempo >= 100)
                    AddScore(-10);
                // si es menor que 100 no resta nada

                // reset counter
                if (_resetCounter <= 0)
                    AddScore(150);
                else if (_resetCounter > 0 && _resetCounter <= 3)
                    AddScore(100);
                else if (_resetCounter > 3 && _resetCounter <= 6)
                    AddScore(50);
                else if (_resetCounter > 6 && _resetCounter <= 10)
                    AddScore(25);
                // si es mayor que 10 no añade nada

                break;
            case GameStates.SCORE:                      //     *PUNTUACIÓN*
                Time.timeScale = 1;

                _ending = 0; // si no tienes las piernas te comes tremendo ñordaco
                if (_alubiat)
                { // pero si sí...
                    if (_score <= 500)
                        _ending = 1;
                    if (_score >= 500 && _score < 700)
                        _ending = 2;
                    else if (_score >= 700 && _score < 1000)
                        _ending = 3;
                    else if (_score >= 1000)
                        _ending = 4;
                } // 0 TERRIBLISIMO, 1 MALAMENTE, 2 REGULA, 3 ASEPTABLE, 4 CRANEOPERSENT
                if (_UIManager != null) _UIManager.ScoreMenuSetUp(_score, _ending);
                break;
            case GameStates.LEVELSELECTOR:              //     *SELECTOR DE NIVELES*
                break;
            case GameStates.CONTROLES:                  //     *CONTROLES*

                break;
            case GameStates.OPCIONES:                   //     *OPCIONES*
                break;
            case GameStates.CREDITS:                    //     *CREDITOS*
                break;
        }

        if (_UIManager != null)
        {
            _UIManager.SetMenu(newState); // como en todos los estados se hace esto, se pone al final según el estado nuevo y listo
            _UIManager.SetFirstButton((int)newState);
            _UIManager.ScoreSetUp(_score);
        }

        _currentGameState = newState;                        // Finaliza el cambio
        Debug.Log("GAMEMANAGER: Current state is " + _currentGameState);
    }

    private void UpdateState(GameStates state)
    {
        if (state == GameStates.PAUSE) // para volver con esc desde la pausa
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (_UIManager != null) _UIManager.PauseToGame();
            }
        }

        if (_UIManager != null)
        {
            if (!_UIManager.SetMenu(state))
            {
                _UIManager.SetMenu(state);
                Debug.Log("Set Menu");
            }
        }

        if (state == GameStates.GAME)
        {
            if (_UIManager != null)
            {
                _UIManager.TimeUpdate(_tiempo);
                _UIManager.ScoreSetUp(_score);
            }
        }

        if (_UIManager != null 
            &&_UIManager.FirstButtons[(int)state] != null
            && EventSystem.current != null
            && EventSystem.current.currentSelectedGameObject == null)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)
               || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
            { // para volver a la selección por teclado
                _UIManager.SetFirstButton((int)state);
            }

            _UIManager.SetFirstButton((int)state);
        }
    }
    #endregion

    #region BLOQUE DE PUNTUACIÓN
    public void AddScore(int value)
    {
        _score += value;
        if (_UIManager != null)
            _UIManager.ScoreSetUp(_score);
        //Debug.Log("SCORE++ " + value);
    }
    private void Contador()
    {
        if (_currentGameState == GameStates.GAME)
            _tiempo += Time.deltaTime;
    }
    #endregion

    void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        gameObject.transform.parent = null;

        // Cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        _currentGameState = GameStates.LEVELSELECTOR; // Valor dummy para que se realice el cambio nada más empezar
        // Estado inicial, es diferente al current para que el EnterState del primer update se realice
        // _nextGameState = GameStates.START;     // ESTADO EN LA ESCENA 1
        _nextGameState = GameStates.GAME; // ESTADO EN LA ESCENA 0

        _ending = 0;
        _resetCounter = 0;
        _tiempo = 0;
        _score = 500;
        //_alubiat = true;
        _viewTime = false;
        _previousScene = -1;
    }

    void Update()
    {
        if (_nextGameState != _currentGameState) // Si se requiere cambiar de estado (si current == next es que seguimos en el mismo)
        {
            EnterState(_nextGameState);      // Entramos al siguiente estado
        }
        Contador();
        UpdateState(_currentGameState);      // Update según el estado
    }
}
