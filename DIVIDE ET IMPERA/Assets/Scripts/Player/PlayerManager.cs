using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public enum TimmyStates { S0, S1, S2, S3, S4, S5 }; // Estados de Timmy:
                                                        // (*ver diagrama en carpeta de apuntes para interaciones interestado^)
                                                        // S0: 2 brazos y piernas
                                                        // S1: 1 brazo y piernas
                                                        // S2: piernas
                                                        // S3: 2 brazos
                                                        // S4: 1 brazo
                                                        // S5: (nada)

    #region References
    private SpriteRenderer _mySpriteRenderer;
    private Animator _myAnimator;
    private CollisionManager _myCollisionManager;
    #endregion

    #region Properties
    private static PlayerManager _instance;             // Instancia de este componente
    private static TimmyStates _currentState;           // Timmy States
    private TimmyStates _nextState;
    public static PlayerManager Instance { get { return _instance; } }
    public static TimmyStates State { get { return _currentState; } }
    #endregion

    #region Methods
    public void RequestTimmyState(TimmyStates state)
    {
        _nextState = state;
    }

    public void AddTimmyState(TimmyStates state) // Cicla los estados en sentido incremental / para debug más que otra cosa
    {
        var length = System.Enum.GetValues(typeof(TimmyStates)).Length; // cantidad de estados
        _nextState = state + 1; 

        if (_nextState >= (TimmyStates)(length)) // si se sale
        {
            _nextState = 0; // da la vuelta
        }
    }

    public void SubTimmyState(TimmyStates state) // Cicla los estados en sentido decremental / para debug más que otra cosa
    {
        var length = System.Enum.GetValues(typeof(TimmyStates)).Length;
        _nextState = state - 1; 
        if (((int)_nextState) < 0) // si se sale
        {
            _nextState = (TimmyStates)(length - 1); // da la vuelta
        }
    }

    private void EnterState(TimmyStates _nextState)
    {
        switch (_nextState)
        {
            case TimmyStates.S0: // S0: 2 brazos y piernas
                _mySpriteRenderer.color = Color.white;
                break;
            case TimmyStates.S1: // S1: 1 brazo y piernas
                _mySpriteRenderer.color = Color.red;
                break;
            case TimmyStates.S2: // S2: piernas
                _mySpriteRenderer.color = Color.yellow;
                break;
            case TimmyStates.S3: // S3: 2 brazos
                _mySpriteRenderer.color = Color.green;
                break;
            case TimmyStates.S4: // S4: 1 brazo
                _mySpriteRenderer.color = Color.cyan;
                break;
            case TimmyStates.S5: // S5: nada
                _mySpriteRenderer.color = Color.magenta;
                break;
        }
        Debug.Log("TIMMY: Cambio de estado de " + _currentState + " a " + _nextState);
        _currentState = _nextState;
    }

    private void UpdateState(TimmyStates _state)
    {
        switch (_state)
        {
            case TimmyStates.S0: // S0: 2 brazos y piernas
                break;
            case TimmyStates.S1: // S1: 1 brazo y piernas
                break;
            case TimmyStates.S2: // S2: piernas
                break;
            case TimmyStates.S3: // S3: 2 brazos
                break;
            case TimmyStates.S4: // S4: 1 brazo
                break;
            case TimmyStates.S5: // S5: nada
                break;
        }
    }
    #endregion

    void Awake()
    {
        _instance     = this;                       // Inicializa la instancia de este componente en toda la escena
    }

    void Start()
    {
        // ._. ^.^ :P o....o ¬_¬ [-:<
        _mySpriteRenderer   = GetComponent<SpriteRenderer>();
        _myAnimator         = GetComponent<Animator>();
        _myCollisionManager = GetComponent<CollisionManager>();

        _currentState = TimmyStates.S1;         // Valor dummy para inicializar un cambio en cuanto empiece y se ejecute el EnterState
        _nextState = TimmyStates.S0;         // Inicializa el estado de timmy
    }

    void Update()
    {
        if (_currentState != _nextState)
        {
            EnterState(_nextState);
        }
        UpdateState(_currentState);
    }
}
