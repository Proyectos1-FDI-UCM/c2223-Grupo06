using UnityEngine;

public class InputController : MonoBehaviour
{
    #region CONTROLES (NUEVO INTENTO YA IMPLEMENTADO)
    /* *ESTILO: metroidvania oriented*
    (mano der)
    ← y →:     Movimiento lateral
    ↑:         Interactuar con palanca enfrente** + Diálogo
    Espacio:   Saltar

    (mano izq)
    Z:         Lanzar brazo
    X:         Soltar y recoger objetos
    Shift + Z / X / C: Lanzar bola si tiene en el ribcage / Chutar enfrente si no (cual de los 3? yo creo que Z)

    A:         Soltar brazos (o recoger si está enfrente**)
    S:         Soltar brazos (o recoger si está enfrente**)
    D:         Soltar piernas (o recoger si está encima o enfrente)
    Shift + A: Interactuar brazo 1 (remoto)
    Shift + S: Interactuar brazo 2 (remoto)
    Shift + D: Intercambiar control entre cuerpo y piernas
        
    **: Lo duplico para que a la hora de asociar en tu mente a qué es cada cosa sea más sencillo 
    porque de esta manera ya entiendes que tanto A como S son brazos whatever the case, y D piernas (toggle) 

    QUEDA CONSIDERAR: alubiat? creo que ya    */
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
    private GroundCheck _groundCheck;
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


    #region Piernas
    //-------------SOLTAR PARTES----------------------------
    // Indica si el jugador ha dejado una parte en un objeto
    [SerializeField]
    private bool _conectarPiernas = false;
    // acceso público a _conectarParte
    public bool ConectarPiernas { get { return _conectarPiernas; } }

    //-------------RECUPERAR PARTES----------------------------
    // Indica si el jugador ha dejado una parte en un objeto
    [SerializeField]
    private bool _recuperarPiernas = false;
    // acceso público a _recuperarParte
    public bool RecuperarPiernas { get { return _recuperarPiernas; } }
    #endregion

    #region Brazos
    //-------------SOLTAR PARTES----------------------------
    // Indica si el jugador ha dejado una parte en un objeto
    [SerializeField]
    private bool _conectarBrazo = false;
    // acceso público a _conectarParte
    public bool ConectarBrazo { get { return _conectarBrazo; } }

    //-------------RECUPERAR PARTES----------------------------
    // Indica si el jugador ha dejado una parte en un objeto
    [SerializeField]
    private bool _recuperarBrazo = false;
    // acceso público a _recuperarParte
    public bool RecuperarBrazo { get { return _recuperarBrazo; } }
    #endregion


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
    private int _movement;
    public int Movement { get { return _movement; } }
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

        _movement = (Input.GetKey(KeyCode.RightArrow) ? 1 : 0) - (Input.GetKey(KeyCode.LeftArrow) ? 1 : 0);
        #endregion

        #region VERTICAL
        if (Input.GetKeyDown(KeyCode.Space)) // Input.GetKeyDown(KeyCode.Z) || (lanzaba y saltaba a la vez oop)
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
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.D) && GetComponentInChildren<GroundCheck>().IsGrounded)
        { // Shift + D para intercambiar control a las piernas
            if (!_changeToPataforma && !PlayerManager.Instance.Piernas)
            {
                _changeToPataforma = true;
            }
        }
        else if (Input.GetKeyDown(KeyCode.D))
        { // D para recoger y soltar piernas
            if (!_collisionManager.DestruirPierna())
            { // si no recoge una pierna del suelo
                Debug.Log("asbjadsbcjhc");
                if(transform.parent != null)
                {
                    if (transform.parent.GetComponentInChildren<PataformaComponent>() != null && !_conectarPiernas && !transform.parent.GetComponentInChildren<PataformaComponent>().PiernasConectadas)
                    { // si está en una pataforma sin piernas, se las pone
                        _conectarPiernas = true;
                        _recuperarPiernas = false;
                        Debug.Log("conecta");
                    }
                    else if (transform.parent.GetComponentInChildren<PataformaComponent>() != null && !_recuperarPiernas && transform.parent.GetComponentInChildren<PataformaComponent>().PiernasConectadas)
                    { // si está en una pataforma con piernas, las recoge
                        _recuperarPiernas = true;
                        _conectarPiernas = false;
                        Debug.Log("recupera");
                    }
                }
                else
                {
                    Debug.Log("aaaaaaaaaaaaaaaa");
                    PlayerManager.Instance.SoltarPiernas(); // si no, las suelta
                }
            } // si sí
            else 
            {
                PlayerManager.Instance.RecogerPiernas(); // la recoge
            }
        }
        else if (_conectarBrazo) _conectarBrazo = false; // estoy probando no juzgarme (sabré si lo hacéis)
        else if (_recuperarBrazo) _recuperarBrazo = false;
        else if (_conectarPiernas) _conectarPiernas = false;
        else if (_recuperarPiernas) _recuperarPiernas = false;
        #endregion

        #region SOLTAR OBJETS + LANZAR / CHUTAR BOLA
        if (Input.GetKey(KeyCode.LeftShift) && (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.C)))
        { // Si es Shift + Z / X / C
            if (PlayerManager.Instance.Objeto == PlayerManager.Objetos.BOLA)
            {
                _throwComp.LanzarBola(); // Si es una bola en el ribcage, la lanza
            }
            else
            {
                _throwComp.ChutarBola(); // Chuta una bola delante
            }
           
        }
        else
        #endregion

        #region SOLTAR OBJETOS
        if (Input.GetKeyDown(KeyCode.X)) // Si solo es X
        {
            if (PlayerManager.Instance.TieneObjeto()) // Si tiene objeto
                PlayerManager.Instance.SoltarObjeto();  // Lo suelta
            else                                      // Si no tiene objeto
                PlayerManager.Instance.RecogerObjeto(); // intenta recogerlo
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {                                     // Si solo pulsa Z
            _throwComp.LanzarBrazo(); // Lanza un brazo
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

        if (Input.GetKeyDown(KeyCode.Y))
        {
            SFXComponent.Instance.PlayYippie();
        }
        #endregion
    }

    public void ResetProperties()
    {
        _interactuar = false;
        _conectarBrazo = false;
        _conectarPiernas =false;
    }

    private void SFXMove()
    {
        if (_direccion != 0 && _groundCheck.IsGrounded)
        {
            if (!SFXComponent.Instance.isPlayingSFX(2))
                SFXComponent.Instance.SFXPlayer(2);
        }
        else
        {
            SFXComponent.Instance.SFXPlayerStop(2);
        }

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
        _groundCheck             = GetComponentInChildren<GroundCheck>();
    }

    void Update()
    {
        //------MOVIMIENTO------------
        MovementInput();

        //------INTERACTIONS----------
        InteractInput();

        //------DEBUG-----------------
        DebugInput();

        //------OPCIÓN DE PAUSA-------
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManager.Instance.CurrentState == GameManager.GameStates.GAME)
            {
                GameManager.Instance.RequestStateChange(GameManager.GameStates.PAUSE);
            }
            

            // desactiva el input
            enabled = false;
            PlayerAccess.Instance.MovementComponent.enabled = false;
            PlayerAccess.Instance.Animator.enabled = false;
        }

        if (SFXComponent.Instance != null)
            SFXMove();

        CoolDown(); // no se usa??
    }
}

