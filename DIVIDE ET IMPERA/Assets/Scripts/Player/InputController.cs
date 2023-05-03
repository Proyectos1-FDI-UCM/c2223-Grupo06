using UnityEngine;
using UnityEngine.EventSystems;

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
    Shift + Z / X: Lanzar bola si tiene en el ribcage / Chutar enfrente si no (cual de los 3? yo creo que Z)

    A:         Soltar brazos (o recoger si está enfrente**)
    S:         Soltar brazos (o recoger si está enfrente**)
    D:         Soltar piernas (o recoger si está encima o enfrente)
    Shift + A: Interactuar brazo 1 (remoto)
    Shift + S: Interactuar brazo 2 (remoto)
    Shift + D: Intercambiar control entre cuerpo y piernas

    C:         Soltar alubiat (o recoger si está encima o enfrente)
    Shift + C: Interactuar alubiat
        
    **: Lo duplico para que a la hora de asociar en tu mente a qué es cada cosa sea más sencillo 
    porque de esta manera ya entiendes que tanto A como S son brazos whatever the case, y D piernas (toggle) 

    QUEDA CONSIDERAR: ?  */
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
    private PalancaComponent _placaComponent;
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
    public bool Interactuar { get { return _interactuar; } }

    [SerializeField]
    private int _whichArm;
    public int WhichArm { get { return _whichArm; } }

    //-------------SOLTAR PARTES----------------------------
    // Indica si el jugador ha dejado una parte en un objeto
    // brazos
    [SerializeField]
    private bool _conectarBrazo = false;
    public bool ConectarBrazo { get { return _conectarBrazo; } }

    // piernas
    [SerializeField]
    private bool _conectarPiernas = false;
    public bool ConectarPiernas { get { return _conectarPiernas; } }

    // alubiat
    [SerializeField]
    private bool _conectarAlubiat = false;
    public bool ConectarAlubiat { get { return _conectarAlubiat; } }


    //-------------RECUPERAR PARTES----------------------------
    // Indica si el jugador ha dejado una parte en un objeto
    // brazos
    [SerializeField]
    private bool _recuperarBrazo = false;
    public bool RecuperarBrazo { get { return _recuperarBrazo; } }

    // piernas
    [SerializeField]
    private bool _recuperarPiernas = false;
    public bool RecuperarPiernas { get { return _recuperarPiernas; } }

    // alubiat
    [SerializeField]
    private bool _recuperarAlubiat = false;
    public bool RecuperarAlubiat { get { return _recuperarAlubiat; } }

    //------------CAMBIAR INPUT----------------------------
    // indica si el jugador quiere cambiar el input a la Pataforma
    // booleano para saber si se ha cambiado el input a la pataforma
    [SerializeField]
    public bool _changeToPiernas;
    public bool ChangeToPiernas { get { return _changeToPiernas; } }
    [SerializeField]
    public bool _changeToAlubiat;
    public bool ChangeToAlubiat { get { return _changeToAlubiat; } }
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
            _direccion = 1;
        else if (Input.GetKey(KeyCode.LeftArrow))
            _direccion = -1;
        else
            _direccion = 0;

        _movement = (Input.GetKey(KeyCode.RightArrow) ? 1 : 0) - (Input.GetKey(KeyCode.LeftArrow) ? 1 : 0);
        #endregion

        #region VERTICAL
        if (Input.GetKeyDown(KeyCode.Space))
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
    }

    private void MechanicInput()
    {
        #region SOLTAR + RECOGER + CONECTAR + DESCONECTAR PARTES
        //BRAZOS
        if (Input.GetKey(KeyCode.LeftShift) && (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S)))
        { // Shift + A / S para controlar brazos en palancas
            if (Input.GetKeyDown(KeyCode.A))
                _whichArm = 1;
            else if (Input.GetKeyDown(KeyCode.S))
                _whichArm = 2;
            _interactuar = true;
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
            if (!_changeToPiernas && !PlayerManager.Instance.Piernas)
                _changeToPiernas = true;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        { // D para recoger y soltar piernas
            if (!_collisionManager.DestruirPierna())
            { // si no recoge una pierna del suelo
                //Debug.Log("y aqui?");
                if (transform.parent != null)
                {
                    //Debug.Log("matadme");
                    if (transform.parent.GetComponentInChildren<PataformaComponent>() != null && !_conectarPiernas
                        && !transform.parent.GetComponentInChildren<PataformaComponent>().PiernasConectadas
                        && !transform.parent.GetComponentInChildren<PataformaComponent>().AlubiatConectadas)
                    { // si está en una pataforma sin piernas, se las pone
                        _conectarPiernas = true;
                        _recuperarPiernas = false;
                        //Debug.Log("por favor");
                    }
                    else if (transform.parent.GetComponentInChildren<PataformaComponent>() != null && !_recuperarPiernas && transform.parent.GetComponentInChildren<PataformaComponent>().PiernasConectadas)
                    { // si está en una pataforma con piernas, las recoge
                        _recuperarPiernas = true;
                        _conectarPiernas = false;
                        // Debug.Log("as");
                    }
                }
                else
                {
                    PlayerManager.Instance.SoltarPiernas();
                    //Debug.Log("que");
                }
            } // si sí
            else
                PlayerManager.Instance.RecogerPiernas();
        }
        else

        // ALUBIAT
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.C) && GetComponentInChildren<GroundCheck>().IsGrounded)
        { // Shift + C para intercambiar control a Alubiat
            if (!_changeToAlubiat && !PlayerManager.Instance.Alubiat)
            {
                _changeToAlubiat = true;
            }
        }
        else
        if (Input.GetKeyDown(KeyCode.C))
        { // Sólo C
            if (!_collisionManager.DestruirAlubiat())
            { // si no recoge a alubiat del suelo
                if (transform.parent != null) // si está en una plataforma con alubiat
                {
                    if (transform.parent.GetComponentInChildren<PataformaComponent>() != null && !_conectarAlubiat
                        && !transform.parent.GetComponentInChildren<PataformaComponent>().AlubiatConectadas
                        && !transform.parent.GetComponentInChildren<PataformaComponent>().PiernasConectadas)
                    { // si está en una pataforma sin alubiat, se las pone
                        _conectarAlubiat = true;
                        Debug.Log("slay");
                        _recuperarAlubiat = false;
                    }
                    else if (transform.parent.GetComponentInChildren<PataformaComponent>() != null && !_recuperarAlubiat
                        && transform.parent.GetComponentInChildren<PataformaComponent>().AlubiatConectadas)
                    { // si está en una pataforma con alubiat, las recoge
                        _recuperarAlubiat = true;
                        Debug.Log("baby baba ¡ baby babyvyrfbueidjwo");
                        _conectarAlubiat = false;
                    }
                }
                else // si no conecta ni desconecta
                    PlayerManager.Instance.SoltarAlubiat();
            }
            else // si destruye a alubiat
                PlayerManager.Instance.RecogerAlubiat();
        }
        else if (_conectarBrazo) _conectarBrazo = false; // estoy probando no juzgarme (sabré si lo hacéis)
        else if (_recuperarBrazo) _recuperarBrazo = false;
        else if (_conectarPiernas) _conectarPiernas = false;
        else if (_recuperarPiernas) _recuperarPiernas = false;
        else if (_recuperarAlubiat) _recuperarAlubiat = false;
        else if (_conectarAlubiat) _conectarAlubiat = false;
        #endregion

        #region SOLTAR OBJETS + LANZAR / CHUTAR BOLA
        if (Input.GetKey(KeyCode.LeftShift) && (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.X)))
        { // Si es Shift + Z / X
            if (PlayerManager.Instance.Objeto == PlayerManager.Objetos.BOLA)
                _throwComp.LanzarBola();
            else
                _throwComp.ChutarBola();
        }
        else
        #endregion

        #region SOLTAR OBJETOS
        if (Input.GetKeyDown(KeyCode.X) && !Input.GetKey(KeyCode.LeftShift)) // Si solo es X
        {
            if (PlayerManager.Instance.TieneObjeto()) // Si tiene objeto
                PlayerManager.Instance.SoltarObjeto();  // Lo suelta
            else                                      // Si no tiene objeto
                PlayerManager.Instance.RecogerObjeto(); // intenta recogerlo
        }

        if (Input.GetKeyDown(KeyCode.Z) && !Input.GetKey(KeyCode.LeftShift)) // si es solo Z
            _throwComp.LanzarBrazo();
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
        if (Input.GetKeyDown(KeyCode.Keypad8))
            PlayerManager.Instance.AddObject(); // SUBIR ESTADO
        else if (Input.GetKeyDown(KeyCode.Keypad9))
            PlayerManager.Instance.SubObject(); // BAJAR ESTADO
        

        // para ver si recoge a alubia bien
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Alpha0))
        {
            if (PlayerManager.Instance.Alubiat)
                PlayerManager.Instance.SoltarAlubiat();
            else PlayerManager.Instance.RecogerAlubiat();
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            //SFXComponent.Instance.PlayYippie(); 
        }
        #endregion
    }

    public void ResetProperties()
    {
        _interactuar = false;
        _conectarBrazo = false;
        _conectarPiernas = false;
    }

    private void SFXMove()
    {
        if (_direccion != 0 && _groundCheck.IsGrounded)
        {
            if (!SFXComponent.Instance.isPlayingSFX(2))
                SFXComponent.Instance.SFXPlayer(2);
        }
        else SFXComponent.Instance.SFXPlayerStop(2);
    }

    private void MenuInput()
    {
        if (GameManager.Instance != null
            && UIManager.Instance != null
            && EventSystem.current != null
            && UIManager.Instance.FirstButtons[(int)GameManager.Instance.CurrentState] != null
            && EventSystem.current.currentSelectedGameObject != UIManager.Instance.FirstButtons[(int)GameManager.Instance.CurrentState])
        {
            if ((Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)) 
                && GameManager.Instance.CurrentState != GameManager.GameStates.OPCIONES)
            { // para volver a la selección por teclado
                UIManager.Instance.SetFirstButton(GameManager.Instance.FbIndex);
            }
        }
    }
    #endregion

    void Start()
    {
        _throwComp = GetComponent<ThrowComponent>();
        _collisionManager = GetComponent<CollisionManager>();
        _playerJump = GetComponent<JumpComponent>();
        _playerRigidBody = GetComponent<Rigidbody2D>(); // rigidbody del player
        _dialogueManager = GetComponent<DialogueManager>();
        _inputControllerDialogue = GetComponent<InputControllerDialogue>();
        _stayOnComp = GetComponent<StayOnPataforma>();
        _groundCheck = GetComponentInChildren<GroundCheck>();
        _placaComponent = GetComponent<PalancaComponent>();
    }

    void Update()
    {
        //------MOVIMIENTO------------
        MovementInput();

        //------INTERACTIONS----------
        InteractInput();
        if (GameManager.Instance != null && GameManager.Instance.CurrentState == GameManager.GameStates.GAME) MechanicInput();

        //------DEBUG-----------------
        DebugInput();

        //------OPCIÓN DE PAUSA-------
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManager.Instance != null && GameManager.Instance.CurrentState == GameManager.GameStates.GAME)
            {
                GameManager.Instance.RequestStateChange(GameManager.GameStates.PAUSE);
                // desactiva el input
                enabled = false;
                PlayerAccess.Instance.MovementComponent.enabled = false;
                PlayerAccess.Instance.Animator.enabled = false;
            }
        }

        if (SFXComponent.Instance != null)
            SFXMove();

        MenuInput();
        //CoolDown(); // no se usa??
    }
}

