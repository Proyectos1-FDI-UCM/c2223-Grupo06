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

    public enum Partes { CABEZA, BRAZO1, BRAZO2, PIERNAS }; // probando para la ui (que está siendo controlado ahora mismo)
    public enum Objetos { LLAVE, MUELLE, BOLA, NADA }; // he preferido usar un enum 

    #region References
    // COMPONENTES
    private SpriteRenderer _mySpriteRenderer;
    private CollisionManager _myCollisionManager;
    private Animator _myAnimator;
    private Transform _myTransform;
    private UIManager _UIManager;
    public UIManager UIManager { get { return _UIManager; } }

    // PREFABS
    [SerializeField]
    private GameObject _brazoPrefab; // brazo a instanciar
    [SerializeField]
    private GameObject _piernaPrefab; // piernas a instanciar
    [SerializeField]
    private GameObject[] _objetosPrefabs; // array de objetos (posibles) a instanciar

    // ANIMACIONES
    [SerializeField] // array de sprites de los diferentes estados de Timmy
    private Sprite[] _sprites;
    [SerializeField] // array de máquinas de estado de animaciones normales (aka SIN OBJETOS, solo Timmy normal)
    private RuntimeAnimatorController[] _defaultControllers;
    [SerializeField] // array de máquinas de estado de animaciones de colores (aka POR OBJETOS)
    private RuntimeAnimatorController[] _colorControllers;
    #endregion

    #region Properties
    // Instancia de este componente
    private static PlayerManager _instance;
    public static PlayerManager Instance { get { return _instance; } }

    // Timmy States
    private static TimmyStates _currentState;
    private TimmyStates _nextState;
    public static TimmyStates State { get { return _currentState; } }

    // Objeto en inventario
    private Objetos _objeto;
    public Objetos Objeto { get { return _objeto; } set { _objeto = value; } }

    // Parte principal controlada
    private Partes _parte;
    public Partes Parte { get { return _parte; } set { _parte = value; } }

    // OBJETO QUE ESTÁ SIENDO CONTROLADO
    public GameObject _partInControl;
    #endregion

    #region Parameters
    private int _brazos;     // cuantos brazos tiene (NUNCA menor que 0 o mayor que 2)
    private bool _piernas;   // si las tiene o si no
    private bool _alubiat;   // si tiene sus piernas o no

    public int Brazos
    {
        get { return _brazos; }
        set
        {
            if (value < 0) _brazos = 0;
            else if (value > 2) _brazos = 2;
            else _brazos = value;
        }
    }
    public bool Piernas { get { return _piernas; } set { _piernas = value; } }
    public bool Alubiat { get { return _alubiat; } set { _alubiat = value; } }
    #endregion

    #region Methods
    // REGISTROS
    public void RegisterUIManager(UIManager uiManager)
    {
        _UIManager = uiManager;
    }

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
        { // Asigna los parámetros según el estado
            case TimmyStates.S0: // S0: 2 brazos y piernas
                _brazos = 2;
                _piernas = true;
                break;
            case TimmyStates.S1: // S1: 1 brazo y piernas
                _brazos = 1;
                _piernas = true;
                break;
            case TimmyStates.S2: // S2: piernas
                _brazos = 0;
                _piernas = true;
                break;
            case TimmyStates.S3: // S3: 2 brazos
                _brazos = 2;
                _piernas = false;
                break;
            case TimmyStates.S4: // S4: 1 brazo
                _brazos = 1;
                _piernas = false;
                break;
            case TimmyStates.S5: // S5: nada
                _brazos = 0;
                _piernas = false;
                break;
        }
        _mySpriteRenderer.sprite = _sprites[(int)_nextState];          // cambio de sprite (esto realmente es in case no funcione el controller change)
        RequestControllerChange(_defaultControllers, (int)_nextState); // cambio de animaciones de timmy
        _currentState = _nextState;                                    // cambio de estado finaliza
    }

    private void UpdateState(TimmyStates _state)
    {
        // LÓGICA DE CAMBIO DE CONTROLADOR DE ANIMACIONES
        if (_objeto != Objetos.NADA && _myAnimator.runtimeAnimatorController != _colorControllers[(int)_state * 3 + ((int)_objeto)])
        { // si tiene algún objeto y el control de animaciones no es de *ese* objeto, se lo pone
            RequestControllerChange(_colorControllers, (int)_state * 3 + ((int)_objeto)); // cambio de animaciones de timmy
            _UIManager.SetObject(_objeto);                                             // cambio de imagen en el ui
            //Debug.Log("COLOR: " + _objeto + ", " + (int)_objeto);
        }
        else if (_objeto == Objetos.NADA && _myAnimator.runtimeAnimatorController != _defaultControllers[(int)_state])
        { // si no tiene objeto y el control de animaciones no es el normal, se lo pone
            RequestControllerChange(_defaultControllers, (int)_state); // cambio de animaciones de timmy
            _UIManager.SetObject(_objeto);                          // cambio de imagen en el ui
            //Debug.Log("¡NADA!");
        }

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

        // LOGICA DE UI
        if (_UIManager != null)
        {
            // ALUBIAT
            if (_alubiat && !_UIManager.TieneAlubiat()) // si tiene alubiat pero no está actualizado el hud
            {
                _UIManager.SetAlubiat(false); // actualiza el hud (de momento false porque está inactivo, placeholder)
            }
            else if (!_alubiat && _UIManager.TieneAlubiat()) // si no tiene alubiat y no está actualizado el hud
            {
                _UIManager.ResetAlubiat(); // resetea a vacio el hueco
            }
        }
    }

    // BLOQUE DE ANIMACIONES
    public void RequestControllerChange(RuntimeAnimatorController[] _controllers, int i)
    { // cambia el control de animaciones al especificado en orden del array
        _myAnimator.runtimeAnimatorController = _controllers[i];
    }

    // BLOQUE DE PARTES
    // brazos
    public void AddBrazo() // para interactuables
    {
        if (Brazos < 2)
        {
            _brazos++;
        }
    }
    public void SubBrazo() // para interactuables
    {
        if (_brazos > 0)
        {
            _brazos--;
        }
    }
    public void RecogerBrazo() // para recoger brazos sueltos
    {
        if (_brazos < 2) // si tiene menos de 2 brazos
        {
            _brazos++; // si las destruye, obtiene un brazo más
        }
    }
    public void SoltarBrazo() // para instanciarlo
    {
        if (_brazos > 0) // si algún brazo y está en un espacio libre
        {
            Instantiate(_brazoPrefab, _myTransform.position, _myTransform.rotation); // instanciación
            _brazos--; // un brazo menos
        }
    }
    // piernas
    public void HolaPiernas() // para interactuables
    {
        if (!_piernas) _piernas = !_piernas;
    }
    public void AdiosPiernas() // para interactuables
    {
        if (_piernas) _piernas = !_piernas;
    }
    public void RecogerPiernas() // para recoger piernas sueltas
    {
        if (!_piernas)
        {
            _piernas = true; // si las destruye, obtiene piernas
        }
    }
    public void SoltarPiernas()  // para instanciar las piernas
    {
        if (_piernas) // si tiene piernas 
        {
            Instantiate(_piernaPrefab, _myTransform.position, _myTransform.rotation); // instanciación
            _piernas = false; // sin piernas
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

    public void CambiarObjeto(Objetos nuevoObjeto) // cambia el objeto de la ribcage
    {
        _objeto = nuevoObjeto;
    }

    public bool EliminarObjeto() // elimina el objeto de la ribcage
    {
        if (_objeto != Objetos.NADA)
        {
            _objeto = Objetos.NADA;
            return true;
        }
        else return false;
    }

    public bool TieneObjeto() // un poco por poner si se necestia en algun momento, devuelve true si la ribcage NO está vacía
    {
        if (_objeto != Objetos.NADA)
        {
            return true;
        }
        else return false;
    }

    public void RecogerObjeto() // para recoger piernas sueltas
    {
        if (!TieneObjeto())
        {
            int objeto = _myCollisionManager.DestruirObjeto();
            if (objeto < 3 && objeto > -1) // destruye las piernas con las que está colisionando
            {
                CambiarObjeto((Objetos)objeto);
            }
        }
    }
    public void SoltarObjeto()  // para instanciar las piernas
    {
        if (TieneObjeto())
        {
            var posicion = _myTransform.position;
            //if (Objeto == Objetos.BOLA) posicion += _myTransform.right * _myTransform.localScale.x;
            Instantiate(_objetosPrefabs[(int)_objeto], posicion, _myTransform.rotation, LevelManager.Instance.CurrentLevel.transform); //Se pone el nivel como padre para que en el reseteo los
                                                                                                                             //objetos recogidos y soltados se eliminen tambien 
            EliminarObjeto();
        }
    }

    // BLOQUE DE PARTES
    public void SwitchPartControl(Partes parte) // cambia el control de parte principal (cabeza o piernas)
    {
        _parte = parte;
        if (_UIManager != null) { _UIManager.SetPartes(_currentState, _parte); }
    }
    public void ChangePartInControl(GameObject thing) // lo he refactorizado a "ChangePartInControl" en vez de Object porque he seguido una nomenclatura en la que objeto son los items posibles de la ribcage y parte Timmy y su cuerpo
    {
        Debug.Log("thing: " + thing);
        _partInControl = thing;
        Debug.Log("paasas: " + _partInControl);
        CameraMovement.Instance.ChangeWhoToFollow(_partInControl);
    }

    // alubiat
    public void RecogerAlubiat()
    {
        _alubiat = true;
    }

    public bool SoltarAlubiat()
    {
        if (_alubiat)
        {
            _alubiat = false;
            return true;
        }
        else return false;

    }
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
        _currentState = TimmyStates.S1;           // Valor dummy para inicializar un cambio en cuanto empiece y se ejecute el EnterState
        _nextState = TimmyStates.S0;         // Inicializa el estado de timmy
        _objeto = Objetos.NADA;          // Ningún objeto al iniciar
        _alubiat = false;           // No tiene las piernas de su padre al iniciar
        _parte = Partes.CABEZA; // Control principal al inicio

        _partInControl = gameObject;
    }

    void Update()
    {
        if (_currentState != _nextState) // Si tiene que cambiar de estado
        {
            EnterState(_nextState); // Entrada al estado
            _UIManager.SetPartes(_nextState, _parte); // Cambia el UI acorde a este
        }

        UpdateState(_currentState); // Update según el estado
    }
}
