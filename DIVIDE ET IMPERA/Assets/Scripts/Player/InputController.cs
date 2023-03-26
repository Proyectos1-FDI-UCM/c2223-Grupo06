using UnityEngine;

public class InputController : MonoBehaviour
{
    #region CONTROLES (NUEVO INTENTO YA IMPLEMENTADO)
    /* *ESTILO: metroidvania oriented*
    (mano der)
    ← y →:     Movimiento lateral
    ↑:         Interactuar con palanca enfrente** + Diálogo

    (mano izq)
    Z:         Saltar
    X:         Soltar y recoger objetos
    C:         Lanzar brazo*
    Shift + C: Lanzar bola delante*
    Shift + X: Lanzar bola ribcage*
    ó
    Shift + C / X: Lanzar bola si tiene en el ribcage / Chutar enfrente si no (posible, sin implementar, opiniones?)

    A:         Soltar brazos (o recoger si está enfrente**)
    S:         Soltar brazos (o recoger si está enfrente**)
    D:         Soltar piernas (o recoger si está encima o enfrente)
    Shift + A: Interactuar brazo 1 (remoto)
    Shift + S: Interactuar brazo 2 (remoto)
    Shift + D: Intercambiar control entre cuerpo y piernas
    
    *: Hago distinción porque es molesto cuando lanzas un brazo y querías lanzar una bola
    
    **: Lo duplico para que a la hora de asociar en tu mente a qué es cada cosa sea más sencillo 
    porque de esta manera ya entiendes que tanto A como S son brazos whatever the case, y D piernas (toggle) 

    QUEDA CONSIDERAR: alubiat? creo que ya
    */
    #endregion

    #region Referencias
    private JumpComponent _playerJump;
    private ThrowComponent _throwComp;
    private CollisionManager _collisionManager;
    private StayOnPataforma _stayOnComp;
    private UIManager _UIManager;
    private DialogueManager _dialogueManager;
    private Rigidbody2D _playerRigidBody;
    private InputControllerDialogue _inputControllerDialogue;
    #endregion

    #region Properties 
    //-------------DIRECCIÓN----------------------------
    //Setea la direccion en la que se mueve el jugador, -1 = izq y 1 = drcha
    private int _direccion;
    public int Direccion { get { return _direccion; } }

    //-------------INTERACTUAR----------------------------
    // Indica si el jugador quiere interactuar con una palanca
    [SerializeField]
    private bool _interactuar = false;
    // acceso público a _interactuar
    public bool Interactuar { get { return _interactuar; } }

    //-------------SOLTAR PARTES----------------------------
    // Indica si el jugador ha dejado una parte en un objeto
    [SerializeField]
    private bool _conectarParte = false;
    // acceso público a _conectarParte
    public bool ConectarParte { get { return _conectarParte; } }

    //-------------RECUPERAR PARTES----------------------------
    // Indica si el jugador ha dejado una parte en un objeto
    [SerializeField]
    private bool _recuperarParte = false;
    // acceso público a _recuperarParte
    public bool RecuperarParte { get { return _recuperarParte; } }

    //------------CAMBIAR INPUT----------------------------
    // indica si el jugador quiere cambiar el input a la Pataforma
    // booleano para saber si se ha cambiado el input a la pataforma
    [SerializeField]
    public bool _changeToPataforma;
    // acceso público a _isPataforma
    public bool ChangeToPataforma { get { return _changeToPataforma; } }
    #endregion

    #region Parameters
    //private bool _canLetGoArm = true; comentado para que unity deje de decirme que hay un parámetro que no se usa

    // max cooldown time
    [SerializeField]
    private float _cooldown = 1;
    private float _elapsedTime;
    #endregion

    #region Methods
    // INPUT: MOVIMENTO LATERAL Y VERTICAL
    private void MovementInput()
    {
        #region HORIZONTAL
        if (Input.GetKey(KeyCode.RightArrow))
            _direccion = 1;
        else if (Input.GetKey(KeyCode.LeftArrow))
            _direccion = -1;
        else
            _direccion = 0;
        #endregion

        #region VERTICAL
        if (Input.GetKeyDown(KeyCode.Z))
            _playerJump.Jump();
        #endregion
    }

    // INPUT: INTERACCIONES DE LAS PARTES
    private void CoolDown() // cooldown para que no pueda soltar los brazos como un loco
    {
        _elapsedTime += Time.deltaTime;

        if (_elapsedTime >= _cooldown)
        {
            //_canLetGoArm = true; comentado para que unity deje de decirme que hay un parámetro que no se usa
            _elapsedTime = 0;
        }
    }

    private void InteractInput()
    {
        // INTERACTUAR
        #region INTERACTUAR
        if (Input.GetKeyUp(KeyCode.UpArrow))
            _interactuar = true;
        else if (_interactuar)
            _interactuar = false;
        #endregion

        #region SOLTAR + RECOGER + CONECTAR + DESCONECTAR PARTES
        //BRAZOS
        if (Input.GetKey(KeyCode.LeftShift) && (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S)))
        { // Shift + A / S para controlar brazos en palancas
            _interactuar = true;
            if (EstaEnPalancaConBrazo()) // me estoy volviendo loca no me tengais mucho en cuenta esto
            {
                _collisionManager.HitboxColisionada.GetComponent<PalancaComponent>().Activar();
            }
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S))
        { // Sólo A ó S
            if (!_collisionManager.DestruirBrazo())
            { // Si no recoge un brazo del suelo
                if (EstaEnPalancaConBrazo())
                { // si está colisionando con una palanca que tiene brazo
                    _collisionManager.HitboxColisionada.GetComponent<PalancaComponent>().ConectarBrazo(false); // lo desconecta
                    PlayerManager.Instance.RecogerBrazo(); // y lo recoge
                }
                else PlayerManager.Instance.SoltarBrazo(); // si no, lo suelta
            } // Si sí
            else PlayerManager.Instance.RecogerBrazo(); // lo recoge
        }
        else
        // PIERNAS 
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.D))
        { // Shift + D para intercambiar control a las piernas
            if (!_changeToPataforma && !PlayerManager.Instance.Piernas)
            {
                _changeToPataforma = true;
                enabled = false;
            }
        }
        else if (Input.GetKeyDown(KeyCode.D))
        { // D para recoger y soltar piernas
            if (!_collisionManager.DestruirPierna())
            { // si no recoge una pierna del suelo
                if (GetComponentInParent<PataformaComponent>() != null && !_conectarParte && !GetComponentInParent<PataformaComponent>().PiernasConectadas)
                { // si está en una pataforma sin piernas, se las pone
                    _conectarParte = true;
                    Debug.Log("conecta");
                }
                else if (GetComponentInParent<PataformaComponent>() != null && !_recuperarParte && GetComponentInParent<PataformaComponent>().PiernasConectadas)
                { // si está en una pataforma con piernas, las recoge
                    _recuperarParte = true;
                    Debug.Log("recupera");
                }
                else PlayerManager.Instance.SoltarPiernas(); // si no, las suelta
            } // si sí
            else PlayerManager.Instance.RecogerPiernas(); // la recoge
        }
        #endregion

        #region LANZAR / CHUTAR
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.C))
        { // Si es Shift + C
            _throwComp.ChutarBola(); // Chuta una bola delante
        }
        else if (Input.GetKeyDown(KeyCode.C))
        { // Si solo pulsa C
            _throwComp.LanzarBrazo(); // Lanza un brazo
        }
        #endregion

        #region SOLTAR OBJETOS
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.X)) // Si es Shift + X
        {
            _throwComp.LanzarBola(); // Si es una bola en el ribcage, la lanza
        }
        else if (Input.GetKeyDown(KeyCode.X)) // Si solo es X
        {
            if (PlayerManager.Instance.TieneObjeto()) // Si tiene objeto
                PlayerManager.Instance.SoltarObjeto();  // Lo suelta
            else                                      // Si no tiene objeto
                PlayerManager.Instance.RecogerObjeto(); // intenta recogerlo
        }
        #endregion
    }

    private bool EstaEnPalancaConBrazo()
    {
        if (_collisionManager.HitboxColisionada != null
         && _collisionManager.HitboxColisionada.GetComponent<PalancaComponent>() != null
         && _collisionManager.HitboxColisionada.GetComponent<PalancaComponent>().BrazoConectado)
        { // si está colisionando con una palanca con un brazo conectado a ella
            return true;
        }
        else return false;
    }

    // PARA PROBAR COSAS DEL INPUT
    private void DebugInput()
    {
        #region DEBUG
        // para ver si cambia de control bien
        if (Input.GetKeyDown(KeyCode.Keypad1))
            PlayerManager.Instance.SwitchPartControl(PlayerManager.Partes.CABEZA);
        else if (Input.GetKeyDown(KeyCode.Keypad2))
            PlayerManager.Instance.SwitchPartControl(PlayerManager.Partes.BRAZO1);
        else if (Input.GetKeyDown(KeyCode.Keypad3))
            PlayerManager.Instance.SwitchPartControl(PlayerManager.Partes.BRAZO2);
        else if (Input.GetKeyDown(KeyCode.Keypad4))
            PlayerManager.Instance.SwitchPartControl(PlayerManager.Partes.PIERNAS);

        // Para ver si cambia de estados bien
        if (Input.GetKeyDown(KeyCode.Keypad5))
            PlayerManager.Instance.AddObject(); // SUBIR ESTADO
        else if (Input.GetKeyDown(KeyCode.Keypad6))
            PlayerManager.Instance.SubObject(); // BAJAR ESTADO

        // para ver si recoge a alubia bien
        if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            if (PlayerManager.Instance.Alubiat)
                PlayerManager.Instance.SoltarAlubiat();
            else PlayerManager.Instance.RecogerAlubiat();
        }
        #endregion
    }
    #endregion

    void Start()
    {
        _throwComp               = GetComponent<ThrowComponent>();
        _collisionManager        = GetComponent<CollisionManager>();
        _playerJump              = GetComponent<JumpComponent>();
        _playerRigidBody         = GetComponent<Rigidbody2D>(); // rigidbody del player
        _dialogueManager         = GetComponent<DialogueManager>();
        _inputControllerDialogue = GetComponent<InputControllerDialogue>();
        _stayOnComp              = GetComponent<StayOnPataforma>();
    }

    void Update()
    {
        //------MOVIMIENTO-------
        MovementInput();

        //------INTERACTIONS-------
        InteractInput();

        //------DEBUG-------
        DebugInput();

        //------OPCIÓN DE PAUSA-------
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //_UIManager.SetMenu(GameManager.GameStates.PAUSE); esto no funcionaba
            PlayerManager.Instance.UIManager.SetMenu(GameManager.GameStates.PAUSE);

            // desactiva el input
            enabled = false;
            PlayerAccess.Instance.MovementComponent.enabled = false;
            PlayerAccess.Instance.Animator.enabled = false;
        }

        CoolDown(); // no se usa??

        if (Input.GetKeyDown(KeyCode.Y))
        {
            LevelManager.Instance.ResetCurrentLevel();
        }
    }
}

#region CONTROLES (TO BE DEPRECATED)
/*MOVIMENTO LATERAL Y VERTICAL*
*
* (TO BE DEPRECATED, ES EL MAPPING ACTUAL)
*
AD ----------- > moverse a los lados
SPACE -------- > saltar

*PALANCA*
E ------------- > conectar parte
R ------------- > recuperar parte
Q ------------- > lanzar

*SOLTAR PARTES*
1 + E -------- > soltar brazo
2 + E -------- > soltar brazo
3 + E -------- > soltar piernas
4 + E -------- > WIP

*RECUPERAR PARTES*
1 + R -------- > recuperar brazo
2 + R -------- > recuperar brazo
3 + R -------- > recuperar piernas
4 + R -------- > WIP

*INTERACCIÓN DE OBJETOS*
1 + T -------- > interactuar con palanca
2 + T -------- > interactuar con palanca
3 + T -------- > interactuar con pataforma
4 + T -------- > WIP 

*COGER Y SOLTAR OBJETOS*
L  -------- > coger
K  -------- > soltar
/*

/* *ESTILO 1: parecido al planteamiento inical*
(mano izq)
A y D  : Movimiento lateral
W: Interactuar con palanca enfrente y por remoto** + diálogo
Espacio: Saltar

E: recoger y soltar objetos (si tiene objeto lo suelta, si no, intenta recoger algo*)
Q: recoger partes (brazo / piernas del suelo / palanca / plataforma)
F: lanzar brazo / bola si tiene

(mano der)
K: soltar brazos  (si lo haces delante de una palanca, se conecta auto)
L: soltar piernas (si lo haces encima de una pataforma, se conectan auto)

    *: quizá? si vemos que no es útil pues un botón para cada, pero creo que puede estar guay eso
    **: para que quede acorde con el HUD, yo pondría las cosas que sean combinaciones según el orden:
        1 + <tecla>: controlar cabeza (PUM a la cabeza)
        2 + <tecla>: si hay dos brazos cada uno en una palanca, activa la primera palanca
        3 + <tecla>:                                                 " la segunda palanca
        4 + <tecla>: controlar piernas
        <tecla>: E? (es la tecla de interaccion por excelencia) */
#endregion

