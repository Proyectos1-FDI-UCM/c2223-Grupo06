﻿using UnityEngine;

public class InputController : MonoBehaviour
{
    #region CONTROLES (NUEVO INTENTO)
    /* *ESTILO 2: más metroidvania de verdad*
    (mano der)
    ← y →:     Movimiento lateral
    ↑:         Interactuar con palanca enfrente** + Diálogo

    (mano izq)
    Z:         Saltar
    X:         Soltar y recoger objetos
    C:         Lanzar brazo*
    A:         Soltar brazos (o recoger si está enfrente**)
    S:         Soltar brazos (o recoger si está enfrente**)
    D:         Soltar piernas (o recoger si está encima o enfrente)

    Shift + C: Lanzar bola delante*
    Shift + X: Lanzar bola ribcage*
    
    *: Hago distinción porque es molesto cuando lanzas un brazo y querías lanzar una bola
    
    **: Lo duplico para que a la hora de asociar en tu mente a qué es cada cosa sea más sencillo porque pretendo entablar esto:
    Shift + A: Interactuar brazo 1 (remoto)
    Shift + S: Interactuar brazo 2 (remoto)
    Shift + D: Intercambiar control entre cuerpo y piernas
       De esta manera ya entiendes que tanto A como S son brazos whatever the case, y D piernas (toggle) 

    QUEDA CONSIDERAR: alubiat? creo que ya
    */
    #endregion

    #region Referencias
    private JumpComponent _playerJump;
    private ThrowComponent _throwComp;
    private PlayerManager _playerManager;
    private UIManager _UIManager;
    private CollisionManager _collisionManager;
    private DialogueManager _dialogueManager;
    private Rigidbody2D _playerRigidBody;
    private GameManager.GameStates state;
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

    //------------DIÁLOGO----------------------------
    // booleano para saber si se está en conversación 
    [SerializeField]
    public bool _enConversacion = false;
    public bool Conversacion { get { return _enConversacion; } }
    #endregion

    #region Parameters
    private bool _canLetGoArm = true;

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
        {
            _direccion = 1;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            _direccion = -1;
        }
        else
        {
            _direccion = 0;
        }
        #endregion

        #region VERTICAL
        if (Input.GetKeyDown(KeyCode.Z))
        {
            _playerJump.Jump();
        }
        #endregion
    }

    // INPUT: INTERACCIONES DE LAS PARTES
    private void CoolDown() // cooldown para que no pueda soltar los brazos como un loco
    {
        _elapsedTime += Time.deltaTime;

        if (_elapsedTime >= _cooldown)
        {
            _canLetGoArm = true;
            _elapsedTime = 0;
        }
    }

    private void InteractInput()
    {
        // TIMMY
        #region PONER PARTES A COSAS
        if (Input.GetKeyDown(KeyCode.E))
        {
            _conectarParte = true;
        }
        else
        {
            _conectarParte = false;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            _recuperarParte = true;
        }
        else
        {
            _recuperarParte = false;
        }
        #endregion

        #region SOLTAR Y RECOGER PARTES
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S))
        {
            PlayerManager.Instance.SoltarBrazo();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            PlayerManager.Instance.SoltarPiernas();
        }
        #endregion

        // INTERACTUAR
        #region INTERACTUAR
        if (Input.GetKeyUp(KeyCode.T))
        {
            // PARA INTERACTUAR (1,2,3,4)
            if (Input.GetKey(KeyCode.Alpha1))
            {
                _interactuar = true;
            }
            else if (Input.GetKey(KeyCode.Alpha2))
            {
                _interactuar = true;
            }
            else if (Input.GetKey(KeyCode.Alpha3))
            {
                if (!_changeToPataforma
                && (PlayerManager.State == PlayerManager.TimmyStates.S3
                || PlayerManager.State == PlayerManager.TimmyStates.S4
                || PlayerManager.State == PlayerManager.TimmyStates.S5))
                {
                    _changeToPataforma = true;
                    //_playerRigidBody.bodyType = RigidbodyType2D.Kinematic;

                    this.enabled = false;
                }
            }
            else if (Input.GetKey(KeyCode.Alpha4)) // WIP
            {
                
            }
        }
        else
        {
            _interactuar = false;
        }
        #endregion

        // LANZAR
        if (Input.GetKeyUp(KeyCode.Q))
        {
            _throwComp.LanzarBrazo();
        }

        #region SOLTAR OBJETOS
        if (Input.GetKeyDown(KeyCode.J))
        {
            _throwComp.LanzarBola();
        }
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.X)) // Si es Shift + X
        {
            _throwComp.LanzarBola(); // Si es una bola en el ribcage, la lanza
        }
        else if (Input.GetKeyDown(KeyCode.X)) // Si solo es X
        {
            if (PlayerManager.Instance.TieneObjeto()) // Si tiene objeto
            {
                PlayerManager.Instance.SoltarObjeto(); // Lo suelta
            }
            else                                      // Si no tiene objeto
            {
                PlayerManager.Instance.RecogerObjeto(); // Intenta recogerlo
            }
        }
        #endregion
    }

    // PARA PROBAR COSAS DEL INPUT
    private void DebugInput()
    {
        #region DEBUG
        // para ver si cambia de control bien
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            PlayerManager.Instance.SwitchPartControl(PlayerManager.Partes.CABEZA);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            PlayerManager.Instance.SwitchPartControl(PlayerManager.Partes.BRAZO1);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            PlayerManager.Instance.SwitchPartControl(PlayerManager.Partes.BRAZO2);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            PlayerManager.Instance.SwitchPartControl(PlayerManager.Partes.PIERNAS);
        }

        // Para ver si cambia de estados bien
        if (Input.GetKeyDown(KeyCode.Keypad5))
        { // SUBIR ESTADO
            PlayerManager.Instance.AddObject();
        }
        else if (Input.GetKeyDown(KeyCode.Keypad6))
        { // BAJAR ESTADO
            PlayerManager.Instance.SubObject();
        }

        // para ver si recoge a alubia bien
        if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            if (PlayerManager.Instance.Alubiat)
            {
                PlayerManager.Instance.SoltarAlubiat();
            }
            else PlayerManager.Instance.RecogerAlubiat();
        }
        #endregion
    }

    #endregion

    void Start()
    {
        _playerJump = GetComponentInChildren<JumpComponent>();
        _throwComp = GetComponent<ThrowComponent>();
        _playerManager = GetComponent<PlayerManager>();
        _collisionManager = GetComponent<CollisionManager>();
        _playerRigidBody = GetComponent<Rigidbody2D>(); // rigidbody del player
        _dialogueManager = GetComponent<DialogueManager>();
        _inputControllerDialogue = GetComponent<InputControllerDialogue>();
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
            //_UIManager.SetMenu(GameManager.GameStates.PAUSE);
            PlayerManager.Instance.UIManager.SetMenu(GameManager.GameStates.PAUSE);

            // desactiva el input
            enabled = false;
            PlayerAccess.Instance.MovementComponent.enabled = false;
            PlayerAccess.Instance.Animator.enabled = false;
        }

        CoolDown();
    }
}

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

/* #region CONTROLES (TO BE DEPRECATED)
    *MOVIMENTO LATERAL Y VERTICAL*
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
    #endregion */
