using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameStates { START, GAME, PAUSE, GAMEOVER, SCORE, LEVELSELECTOR, CONTROLES, OPCIONES };    // Estados del juego (faltan)

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
    #endregion

    #region methods
    // Bloque de registros de referencias
    public void RegisterUIManager(UIManager uiManager)
    {
        _UIManager = uiManager;
    }

    public void RegisterCollisionManager(CollisionManager collisionManager)
    {
        _collisionManager = collisionManager;
    }

    // Bloque de cambios de estado
    public void RequestStateChange(GameStates newState) // M�todo p�blico para cambiar el valor privado de estado 
    {
        _nextGameState = newState;

        if (_bGMComponent != null) _bGMComponent.CanPlay = true;
    }

    // Bloque de m�quina de estados
    public void EnterState(GameStates newState)
    {
        switch (newState) // Diferentes comportamientos seg�n estado al que se entra
        { // En s�, solo cambia el grupo de UI por cada estado y en GAME carga el nivel

            case GameStates.START:                       //     *MEN� INICIAL*
                break;
            case GameStates.GAME:                        //     *JUEGO*
                if (_UIManager != null) // Inicializa valores del HUD
                {
                    _UIManager.SetPartes(PlayerManager.State, PlayerManager.Instance.Parte);
                }
                Time.timeScale = 1;
                break;
            case GameStates.PAUSE:                       //     *PAUSA*
                Time.timeScale = 0;
                break;
            case GameStates.GAMEOVER:                    //     *FIN DEL JUEGO*
                
                break;
            case GameStates.SCORE:                       //     *PUNTUACI�N*
                if(puntuacion.puntos == 1000)
                {
                    // UIManager.Instance._tarjetas[3]; // da error, no compila
                }
                break;
            case GameStates.LEVELSELECTOR:               //     *SELECTOR DE NIVELES*
                break;
            case GameStates.CONTROLES:                   //     *CONTROLES*
                break;
            case GameStates.OPCIONES:                   //      *OPCIONES*
                break;
        }
        if (_UIManager != null) _UIManager.SetMenu(newState); // como en todos los estados se hace esto, se pone al final seg�n el estado nuevo y listo
        _currentGameState = newState;                        // Finaliza el cambio
        Debug.Log("GAMEMANAGER: Current state is " + _currentGameState);
    }

    private void UpdateState(GameStates state)
    {
        if (_currentGameState == GameStates.PAUSE) // para volver con esc desde la pausa
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (_UIManager != null) _UIManager.PauseToGame();
            }
        }
    }

    #endregion

    void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        _currentGameState = GameStates.GAMEOVER; // Valor dummy para que se realice el cambio nada m�s empezar
        _nextGameState = GameStates.START; // Estado inicial, es diferente al current para que el EnterState del primer update se realice
    }

    void Update()
    {
        if (_nextGameState != _currentGameState) // Si se requiere cambiar de estado (si current == next es que seguimos en el mismo)
        {
            EnterState(_nextGameState);      // Entramos al siguiente estado
        }
        UpdateState(_currentGameState);      // Update seg�n el estado
    }
}
