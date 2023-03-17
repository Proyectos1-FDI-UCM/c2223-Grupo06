using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameStates { START, GAME, PAUSE, GAMEOVER };    // Estados del juego (faltan)

    #region references
    private UIManager _UIManager;
    private CollisionManager _collisionManager;
    #endregion

    #region properties
    // Game States
    private static GameManager _instance;
    private GameStates _currentGameState;
    private GameStates _nextGameState;
    public static GameManager Instance { get { return _instance; } }
    public GameStates CurrentState { get { return _currentGameState; } }
    public  GameStates NextState { get { return _nextGameState; } }
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
    public void RequestStateChange(GameStates newState) // Método público para cambiar el valor privado de estado 
    {
        _nextGameState = newState;
    }

    // Bloque de máquina de estados
    public void EnterState(GameStates newState)
    {
        switch (newState) // Diferentes comportamientos según estado al que se entra
        { // En sí, solo cambia el grupo de UI por cada estado y en GAME carga el nivel
            case GameStates.START:                       //     *MENÚ INICIAL*
                //_UIManager.SetMenu(GameStates.START);    // Activa menú inicial
                break;
            case GameStates.GAME:                        //     *JUEGO*
                _UIManager.SetMenu(GameStates.GAME);     // Activa HUD
                if (_UIManager != null) _UIManager.SetPartes(PlayerManager.State, PlayerManager.Instance.Parte); // Inicializa valores del HUD
                break;
            case GameStates.PAUSE:
                //_UIManager.SetMenu(GameStates.PAUSE);
                break;
            case GameStates.GAMEOVER:                    //     *FIN DEL JUEGO*
                //_UIManager.SetMenu(GameStates.GAMEOVER); // Activa el texto de GameOver
                break;
        }
        _currentGameState = newState;                        // Finaliza el cambio
        Debug.Log("CURRENT: " + _currentGameState);
    }

    private void UpdateState(GameStates state)
    {/*
        if (state == GameStates.INTRO)
        {
            
        }

        if (state == GameStates.GAME)
        {

        }*/
    }
    #endregion

    void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        _currentGameState = GameStates.GAMEOVER; // Valor dummy para que se realice el cambio nada más empezar
        _nextGameState = GameStates.START;    // Estado inicial, es diferente al current para que el EnterState del primer update se realice
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
