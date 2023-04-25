using UnityEngine;
using Unity.UI;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public enum GameStates { START, INTRO, GAME, PAUSE, GAMEOVER, SCORE, LEVELSELECTOR, CONTROLES, OPCIONES, CREDITS };    // Estados del juego (faltan)

    #region references
    private UIManager _UIManager;
    private CollisionManager _collisionManager;
    private BGMComponent _bGMComponent;
    #endregion

    #region properties
    // Game States
    private static GameManager _instance;
    private GameStates _currentGameState;
    private GameStates _nextGameState;
    public static GameManager Instance { get { return _instance; } }
    public GameStates CurrentState { get { return _currentGameState; } }
    public GameStates NextState { get { return _nextGameState; } }

    // control por teclado
    private int _fbIndex; // first button index en el array del uimanager ESTÁN PUESTOS POR ÓRDEN DEL ENUM DE ESTADOS DEL GAMEMANAGER
    public int FbIndex { get {  return _fbIndex; } set { _fbIndex = value; } }

    // puntuación
    private int _score;
    public int Score { get { return _score; } set { _score = value; } }
    #endregion

    #region REGISTROS DE REFERENCIAS
    public void RegisterUIManager(UIManager uiManager)
    {
        _UIManager = uiManager;
    }

    public void RegisterCollisionManager(CollisionManager collisionManager)
    {
        _collisionManager = collisionManager;
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
        { // En sí, solo cambia el grupo de UI por cada estado y en GAME carga el nivel

            case GameStates.START:                       //     *MENÚ INICIAL*
                break;
            case GameStates.INTRO:                       //     *INTRO*
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
                if (Contador.tiempo > 500)
                    SubScore(150);
                else if (Contador.tiempo <= 500 || Contador.tiempo > 400)
                    SubScore(100);
                else if (Contador.tiempo <= 400 || Contador.tiempo > 300)
                    SubScore(90);
                else if (Contador.tiempo <= 300 || Contador.tiempo > 200)
                    SubScore(80);
                else if (Contador.tiempo <= 200 || Contador.tiempo > 100)
                    SubScore(20);
                else if (Contador.tiempo <= 100)
                    SubScore(10);

                break;
            case GameStates.SCORE:                      //     *PUNTUACIÓN*
                if (UIManager.Instance != null) UIManager.Instance.ScoreMenuSetUp(_score);
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
        if (_UIManager != null) _UIManager.SetMenu(newState); // como en todos los estados se hace esto, se pone al final según el estado nuevo y listo
        if (UIManager.Instance != null) UIManager.Instance.SetFirstButton((int)newState);
        if (UIManager.Instance != null) UIManager.Instance.ScoreSetUp(_score);

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

            if (UIManager.Instance.FirstButtons[(int)state] != null
                && EventSystem.current != null
                && EventSystem.current.currentSelectedGameObject != UIManager.Instance.FirstButtons[(int)state])
            {
                if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
                { // para volver a la selección por teclado
                    UIManager.Instance.SetFirstButton(3);
                }
            }
        }

        if (state == GameStates.GAME)
        {
            if (_UIManager != null && !_UIManager.SetMenu(state))
            {
                _UIManager.SetMenu(state);
                Debug.Log("Set Menu");
            }
        }
    }
    #endregion

    #region BLOQUE DE PUNTUACIÓN
    public void AddScore(int value)
    {
        _score += value;
        if (UIManager.Instance != null) UIManager.Instance.ScoreSetUp(_score);
    }
    public void SubScore(int value)
    {
        _score -= value;
        if (UIManager.Instance != null) UIManager.Instance.ScoreSetUp(_score);
    }
    #endregion

    void Awake()
    {
        _instance = this;
        gameObject.transform.parent = null;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        _currentGameState = GameStates.GAMEOVER; // Valor dummy para que se realice el cambio nada más empezar
        _nextGameState = GameStates.START;       // Estado inicial, es diferente al current para que el EnterState del primer update se realice
        //SCORE DEBUG:
        _score = 1500;
        //_score = 750;
        //_score = 550;
        //_score = 250;
    }

    void Update()
    {
        if (_nextGameState != _currentGameState) // Si se requiere cambiar de estado (si current == next es que seguimos en el mismo)
        {
            EnterState(_nextGameState);      // Entramos al siguiente estado
        }
        UpdateState(_currentGameState);      // Update según el estado
    }
}
