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
    private CollisionManager _myCollisionManager;
    private Transform _myTransform;

    [SerializeField]
    private GameObject _brazo;
    [SerializeField]
    private GameObject _pierna;

    //array de sprites de los diferentes estados de Timmy
    [SerializeField]
    private Sprite[] _sprites;
    #endregion

    #region Properties
    private static PlayerManager _instance;             // Instancia de este componente
    private static TimmyStates _currentState;           // Timmy States
    private TimmyStates _nextState;
    public static PlayerManager Instance { get { return _instance; } }
    public static TimmyStates State { get { return _currentState; } }
    #endregion

    #region Parameters
    private int _brazos;   // cuantos brazos tiene (NUNCA menor que 0 o mayor que 2)
    private bool _piernas; // si las tiene o si no
    #endregion

    #region Methods
    // M�TODOS DE LA M�QUINA DE ESTADO
    public void RequestTimmyState(TimmyStates state)
    {
        _nextState = state;
    }

    public void AddTimmyState(TimmyStates state) // Cicla los estados en sentido incremental / para debug m�s que otra cosa
    {
        var length = System.Enum.GetValues(typeof(TimmyStates)).Length; // cantidad de estados
        _nextState = state + 1;

        if (_nextState >= (TimmyStates)(length)) // si se sale
        {
            _nextState = 0; // da la vuelta
        }
    }

    public void SubTimmyState(TimmyStates state) // Cicla los estados en sentido decremental / para debug m�s que otra cosa
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
                //_mySpriteRenderer.color = Color.white;
                _mySpriteRenderer.sprite = _sprites[0];
                _brazos = 2;
                _piernas = true;
                break;
            case TimmyStates.S1: // S1: 1 brazo y piernas
                //_mySpriteRenderer.color = Color.red;
                _mySpriteRenderer.sprite = _sprites[1];
                _brazos = 1;
                _piernas = true;
                break;
            case TimmyStates.S2: // S2: piernas
                //_mySpriteRenderer.color = Color.yellow;
                _mySpriteRenderer.sprite = _sprites[2];
                _brazos = 0;
                _piernas = true;
                break;
            case TimmyStates.S3: // S3: 2 brazos
                //_mySpriteRenderer.color = Color.green;
                _mySpriteRenderer.sprite = _sprites[3];
                _brazos = 2;
                _piernas = false;
                break;
            case TimmyStates.S4: // S4: 1 brazo
                //_mySpriteRenderer.color = Color.cyan;
                _mySpriteRenderer.sprite = _sprites[4];
                _brazos = 1;
                _piernas = false;
                break;
            case TimmyStates.S5: // S5: nada
                //_mySpriteRenderer.color = Color.magenta;
                _mySpriteRenderer.sprite = _sprites[5];
                _brazos = 0;
                _piernas = false;
                break;
        }
        //Debug.Log("TIMMY: Cambio de estado de " + _currentState + " a " + _nextState);
        //Debug.Log("TIMMY: Brazos: " + _brazos + " y piernas: " + _piernas);
        _currentState = _nextState;
    }

    private void UpdateState(TimmyStates _state)
    {
        switch (_state)
        {
            case TimmyStates.S0: // S0: 2 brazos y piernas
                if (_brazos == 1)
                { // dejar brazo
                    _nextState = TimmyStates.S1;
                }
                if (_piernas == false)
                { // dejar piernas
                    _nextState = TimmyStates.S3;
                }
                break;

            case TimmyStates.S1: // S1: 1 brazo y piernas
                if (_brazos == 0)
                { // dejar brazo
                    _nextState = TimmyStates.S2;
                }
                if (_brazos == 2)
                { // coger brazo
                    _nextState = TimmyStates.S0;
                }
                if (_piernas == false)
                { // dejar pienas
                    _nextState = TimmyStates.S4;
                }
                break;

            case TimmyStates.S2: // S2: piernas
                if (_brazos == 1)
                { // coger brazo
                    _nextState = TimmyStates.S1;
                }
                if (_piernas == false)
                { // dejar piernas
                    _nextState = TimmyStates.S5;
                }
                break;

            case TimmyStates.S3: // S3: 2 brazos
                if (_brazos == 1)
                { // dejar brazo
                    _nextState = TimmyStates.S4;
                }
                if (_piernas == true)
                { // coger pierna
                    _nextState = TimmyStates.S0;
                }
                break;

            case TimmyStates.S4: // S4: 1 brazo
                if (_brazos == 0)
                { // dejar brazo
                    _nextState = TimmyStates.S5;
                }
                if (_brazos == 2)
                { // coger brazo
                    _nextState = TimmyStates.S3;
                }
                if (_piernas == true)
                { // coger piernas
                    _nextState = TimmyStates.S1;
                }
                break;

            case TimmyStates.S5: // S5: nada
                if (_brazos == 1)
                { // coger brazo
                    _nextState = TimmyStates.S4;
                }
                if (_piernas == true)
                { // coger piernas
                    _nextState = TimmyStates.S2;
                }
                break;
        }
    }

    // BLOQUE DE ACCIONES
    public void SoltarBrazo()
    {
        if (_brazos > 0 && _myCollisionManager._objetoColisionado == null) // si alg�n brazo y est� en un espacio libre
        {
            Instantiate(_brazo, _myTransform.position, _myTransform.rotation); // instanciaci�n
            _brazos--; // un brazo menos
        }
    }

    public void RecogerBrazo()
    {
        if (_brazos < 2) // si tiene menos de 2 brazos
        {
            if (_myCollisionManager.DestruirBrazo()) // destruye el brazo con el que est� colisionando
            {
                _brazos++; // si las destruye, obtiene un brazo m�s
            }
        }
    }

    public void SoltarPiernas()
    {
        if (_piernas && _myCollisionManager._objetoColisionado == null) // si tiene piernas y est� en un espacio libre
        {
            Instantiate(_pierna, _myTransform.position, _myTransform.rotation); // instanciaci�n
            _piernas = false; // sin piernas
        }
    }

    public void RecogerPiernas()
    {
        if (!_piernas)
        {
            if (_myCollisionManager.DestruirPierna()) // destruye las piernas con las que est� colisionando
            {
                _piernas = true; // si las destruye, obtiene piernas
            }
        }
    }
    #endregion

    void Awake()
    {
        _instance = this;                       // Inicializa la instancia de este componente en toda la escena
    }

    void Start()
    {
        // ._. ^.^ :P o....o �_� [-:<
        _mySpriteRenderer = GetComponent<SpriteRenderer>();
        _myCollisionManager = GetComponent<CollisionManager>();
        _myTransform = transform;

        _currentState = TimmyStates.S1;         // Valor dummy para inicializar un cambio en cuanto empiece y se ejecute el EnterState
        _nextState = TimmyStates.S0;         // Inicializa el estado de timmy
    }

    void Update()
    {
        if (_currentState != _nextState)
        {
            EnterState(_nextState); // Entrada al estado
        }
        UpdateState(_currentState); // Update seg�n el estado
    }
}
