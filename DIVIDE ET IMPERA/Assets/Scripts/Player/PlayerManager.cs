using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public enum TimmyStates { S0, S1, S2, S3, S4, S5 }; // Estados de Timmy:
                                                        // (*ver diagrama en carpeta de apuntes para interaciones)
                                                        // S0: 2 brazos y piernas
                                                        // S1: 1 brazo y piernas
                                                        // S2: piernas
                                                        // S3: 2 brazos
                                                        // S4: 1 brazo
                                                        // S5: (nada)
                                                        // Por cada estado, hay 3 variantes según objeto portado (Red, green y blue)
                                                        // Por ejemplo: S0 es Timmy normal con todas las partes, S3R es Timmy con sólo 2 brazos y una llave dentro
    public enum Objetos { LLAVE, MUELLE, BOLA, NADA }; // probando probando

    #region References
    // COMPONENTES
    private SpriteRenderer _mySpriteRenderer;
    private CollisionManager _myCollisionManager;
    private Animator _myAnimator;
    private Transform _myTransform;

    // PREFABS
    [SerializeField]
    private GameObject _brazo; // brazo a instanciar
    [SerializeField]
    private GameObject _pierna; // piernas a instanciar
    [SerializeField]
    private GameObject[] _objetos; // array de objetos (posibles) a instanciar

    // SPRITES
    [SerializeField] // array de sprites de los diferentes estados de Timmy
    private Sprite[] _sprites;
    [SerializeField] // array de máquinas de estado de animaciones normales (aka SIN OBJETOS, solo Timmy normal)
    private RuntimeAnimatorController[] _defaultControllers;
    [SerializeField] // array de máquinas de estado de animaciones de colores (aka POR OBJETOS)
    private RuntimeAnimatorController[] _colorControllers;
    #endregion

    #region Properties
    private static PlayerManager _instance;             // Instancia de este componente
    public static PlayerManager Instance { get { return _instance; } }
    private static TimmyStates _currentState;         // Timmy States
    private TimmyStates _nextState;
    public static TimmyStates State { get { return _currentState; } }
    #endregion

    #region Parameters
    private int _brazos;     // cuantos brazos tiene (NUNCA menor que 0 o mayor que 2)
    private bool _piernas;   // si las tiene o si no
    private Objetos _objeto; // el objeto que tiene (enum)
    //private GameObject _objeto2; // el objeto que tiene DEBUGGGGGG
    #endregion

    #region Methods
    // MÉTODOS DE LA MÁQUINA DE ESTADO
    public void RequestTimmyState(TimmyStates state) // Cambia manualmente a X estado especificado
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
    { // ACCIONES AL ENTRAR A ESTADO
        switch (_nextState)
        {
            case TimmyStates.S0: // S0: 2 brazos y piernas
                //_mySpriteRenderer.color = Color.white;
                _brazos = 2;
                _piernas = true;
                break;
            case TimmyStates.S1: // S1: 1 brazo y piernas
                //_mySpriteRenderer.color = Color.red;
                _brazos = 1;
                _piernas = true;
                break;
            case TimmyStates.S2: // S2: piernas
                //_mySpriteRenderer.color = Color.yellow;
                _brazos = 0;
                _piernas = true;
                break;
            case TimmyStates.S3: // S3: 2 brazos
                //_mySpriteRenderer.color = Color.green;
                _brazos = 2;
                _piernas = false;
                break;
            case TimmyStates.S4: // S4: 1 brazo
                //_mySpriteRenderer.color = Color.cyan;
                _brazos = 1;
                _piernas = false;
                break;
            case TimmyStates.S5: // S5: nada
                //_mySpriteRenderer.color = Color.magenta;
                _brazos = 0;
                _piernas = false;
                break;
        }
        _mySpriteRenderer.sprite = _sprites[(int)_nextState];
        RequestControllerChange(_defaultControllers, (int)_nextState);
        _currentState = _nextState;
        //Debug.Log("TIMMY: Cambio de estado de " + _currentState + " a " + _nextState);
        //Debug.Log("TIMMY: Brazos: " + _brazos + " y piernas: " + _piernas);
        //Debug.Log("TIMMY: Animator Controller: " + _myAnimator.runtimeAnimatorController);
    }

    private void UpdateState(TimmyStates _state)
    { 
        // LÓGICA DE CAMBIO DE ESTADOS 
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

        // LÓGICA DE CAMBIO DE CONTROLADOR DE ANIMACIONES
        if (_objeto != Objetos.NADA && _myAnimator.runtimeAnimatorController != _colorControllers[(int)_state * 3 + ((int)_objeto)])
        { // si tiene algún objeto y el control de animaciones no es de *ese* objeto, se lo pone
            RequestControllerChange(_colorControllers, (int)_state * 3 + ((int)_objeto));
            Debug.Log("COLOR: " + _objeto + ", " + (int)_objeto);
        }
        else if (_objeto == Objetos.NADA && _myAnimator.runtimeAnimatorController != _defaultControllers[(int)_state])
        { // si no tiene objeto y el control de animaciones no es el normal, se lo pone
            RequestControllerChange(_defaultControllers, (int)_state);
            Debug.Log("¡NADA!");
        }
    }

    public void RequestControllerChange(RuntimeAnimatorController[] _controllers, int i)
    { // cambia el control de animaciones al especificado en orden del array
        _myAnimator.runtimeAnimatorController = _controllers[i];
    }

    // BLOQUE DE ACCIONES
    public void SoltarBrazo()
    {
        if (_brazos > 0) // si algún brazo y está en un espacio libre
        {
            Instantiate(_brazo, _myTransform.position, _myTransform.rotation); // instanciación
            _brazos--; // un brazo menos
        }
    }

    public void RecogerBrazo()
    {
        if (_brazos < 2) // si tiene menos de 2 brazos
        {
            if (_myCollisionManager.DestruirBrazo()) // destruye el brazo con el que está colisionando
            {
                _brazos++; // si las destruye, obtiene un brazo más
            }
        }
    }

    public void SoltarPiernas()
    {
        if (_piernas) // si tiene piernas y está en un espacio libre
        {
            Instantiate(_pierna, _myTransform.position, _myTransform.rotation); // instanciación
            _piernas = false; // sin piernas
        }
    }

    public void RecogerPiernas()
    {
        if (!_piernas)
        {
            if (_myCollisionManager.DestruirPierna()) // destruye las piernas con las que está colisionando
            {
                _piernas = true; // si las destruye, obtiene piernas
            }
        }
    }

    // BLOQUE DE OBJETOS
    public void AddObject() // Cicla los estados en sentido incremental / para debug más que otra cosa
    {
        var length = System.Enum.GetValues(typeof(Objetos)).Length; // cantidad de estados
        _objeto += 1;

        if (_objeto >= (Objetos)(length)) // si se sale
        {
            _objeto = 0; // da la vuelta
        }
    }

    public void SubObject() // Cicla los estados en sentido decremental / para debug más que otra cosa
    {
        var length = System.Enum.GetValues(typeof(Objetos)).Length;
        _objeto -= 1;
        if (((int)_objeto) < 0) // si se sale
        {
            _objeto = (Objetos)(length - 1); // da la vuelta
        }
    }
    public void CambiarObjeto(Objetos nuevoObjeto)
    {
        _objeto = nuevoObjeto;
    }
    /*
    public void AñadirObjeto2(GameObject nuevoObjeto)
    {
        _objeto2 = nuevoObjeto;
    }
    */
    public bool EliminarObjeto()
    {
        if (_objeto != Objetos.NADA)
        {
            _objeto = Objetos.NADA;
            return true;
        }
        else return false;
    }
    /*
    public bool EliminaObjeto2()
    {
        if (_objeto2 != null)
        {
            _objeto2 = null;
            return true;
        }
        else return false;
    }
    */
    public bool TieneObjeto()
    {
        if (_objeto != Objetos.NADA)
        {
            return true;
        }
        else return false;
    }
    /*
    public bool TieneObjeto2()
    {
        if (_objeto2 != null)
        {
            return true;
        }
        else return false;
    }
    */
    #endregion

    void Awake()
    {
        _instance = this;                       // Inicializa la instancia de este componente en toda la escena
    }

    void Start()
    {
        // ._. ^.^ :P o....o ¬_¬ [-:<

        // Inicialización de referencias por componentes
        _mySpriteRenderer = GetComponent<SpriteRenderer>();
        _myCollisionManager = GetComponent<CollisionManager>();
        _myAnimator = GetComponent<Animator>();
        _myTransform = transform;

        // Ejecución de la entrada a estado inicial
        _currentState = TimmyStates.S1;         // Valor dummy para inicializar un cambio en cuanto empiece y se ejecute el EnterState
        _nextState = TimmyStates.S0;         // Inicializa el estado de timmy
        _objeto = Objetos.NADA;
        //_objeto2 = null;
    }

    void Update()
    {
        if (_currentState != _nextState)
        {
            EnterState(_nextState); // Entrada al estado
        }

        UpdateState(_currentState); // Update según el estado
    }
}
