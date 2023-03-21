using UnityEngine;

public class InputController : MonoBehaviour
{
    #region Referencias
    private JumpComponent _playerJump;
    private GameManager.GameStates state;
    private ThrowComponent _throwComp;
    private Rigidbody2D _playerRigidBody;
    private PlayerManager _playerManager;
    private CollisionManager _collisionManager;

    [SerializeField] private UIManager _UIManager;

    #endregion

    #region Properties 
    //-------------DIRECCIÓN----------------------------
    //Setea la direccion en la que se mueve el jugador, -1 = izq y 1 = drcha
    private int _direccion;
    public int Direccion { get { return _direccion; } }

    //-------------INTERACTUAR------------------------------
    // Indica si el jugador quiere interactuar con una palanca
    [SerializeField]
    private bool _interactuar = false;
    // acceso público a _interactuar
    public bool Interactuar { get { return _interactuar; } }

    //-------------SOLTAR PARTES-----------------------------
    // Indica si el jugador ha dejado una parte en un objeto
    [SerializeField]
    private bool _conectarParte = false;
    // acceso público a _conectarParte
    public bool ConectarParte { get { return _conectarParte; } }

    //-------------RECUPERAR PARTES-----------------------------
    // Indica si el jugador ha dejado una parte en un objeto
    [SerializeField]
    private bool _recuperarParte = false;
    // acceso público a _recuperarParte
    public bool RecuperarParte { get { return _recuperarParte; } }

    //------------CAMBIAR INPUT-----------------------------
    // indica si el jugador quiere cambiar el input a la Pataforma
    // booleano para saber si se ha cambiado el input a la pataforma
    [SerializeField]
    public bool _changeToPataforma;
    // acceso público a _isPataforma
    public bool ChangeToPataforma { get { return _changeToPataforma; } }

    [SerializeField]
    private bool _conversar = false;
    // acceso público a _conversar
    public bool Conversar { get { return _conversar; } }
    #endregion

    #region Parameters
    private bool _canLetGoArm = true;

    // max cooldown time
    [SerializeField]
    private float _cooldown = 1;
    private float _elpsedTime;

    #endregion

    #region Methods

    // cooldown para que no pueda soltar los brazos como un loco
    private void CoolDown()
    {
        _elpsedTime += Time.deltaTime;

        if (_elpsedTime >= _cooldown)
        {
            _canLetGoArm = true;
            _elpsedTime = 0;
        }
    }

    // input del movimiento lateral y vertical
    private void MovementInput()
    {
        #region HORIZONTAL
        if (Input.GetKey(KeyCode.D))
        {
            _direccion = 1;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            _direccion = -1;
        }
        else
        {
            _direccion = 0;
        }
        #endregion

        #region VERTICAL
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _playerJump.Jump();
        }
        #endregion
    }

    // input de las interacciones de las partes
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

        // soltar y recoge partes
        if (Input.GetKeyUp(KeyCode.E))
        {
            if (Input.GetKey(KeyCode.Alpha1) && _playerManager.Brazos == 2)
            {
                PlayerManager.Instance.SoltarBrazo();
            }
            else if (Input.GetKey(KeyCode.Alpha2) && _playerManager.Brazos == 1)
            {
                PlayerManager.Instance.SoltarBrazo();
            }
            else if (Input.GetKey(KeyCode.Alpha3))
            {
                PlayerManager.Instance.SoltarPiernas();
            }
            else if (Input.GetKey(KeyCode.Alpha4))
            {

            }
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            if (Input.GetKey(KeyCode.Alpha1) && _playerManager.Brazos == 1)
            {
                PlayerManager.Instance.RecogerBrazo();
            }
            else if (Input.GetKey(KeyCode.Alpha2) && _playerManager.Brazos == 0)
            {
                PlayerManager.Instance.RecogerBrazo();
            }
            else if (Input.GetKey(KeyCode.Alpha3) && !_playerManager.Piernas)
            {
                PlayerManager.Instance.RecogerPiernas();
            }
            else if (Input.GetKey(KeyCode.Alpha4))
            {

            }
        }

        // interactuar
        if (Input.GetKeyUp(KeyCode.T))
        {
            // PARA INTERACTUAR
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
            else if (Input.GetKey(KeyCode.Alpha4))
            {

            }
        }
        else
        {
            _interactuar = false;
        }

        // lanzar
        if (Input.GetKeyUp(KeyCode.Q))
        {
            _throwComp.LanzarBrazo();
        }
    }

    private void DebugInput()
    {
        #region ESTADOS
        //      Para ver si cambia de estados bien
        if (Input.GetKeyDown(KeyCode.V))
        { // SUBIR ESTADO
            PlayerManager.Instance.AddObject();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        { // BAJAR ESTADO
            PlayerManager.Instance.SubObject();
        }
        #endregion

        #region RECOGER PIERNAS DE ALUBIA
        // para ver si recoge a alubia bien
        if (Input.GetKeyDown(KeyCode.B))
        {
            PlayerManager.Instance.RecogerAlubiat();
        }
        else if (Input.GetKeyDown(KeyCode.N))
        {
            PlayerManager.Instance.SoltarAlubiat();
        }
        #endregion

        #region SOLTAR OBJETOS
        // para ver si suelta los objetos bien
        if (Input.GetKeyDown(KeyCode.J))
        {
            _throwComp.LanzarBola();
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            PlayerManager.Instance.SoltarObjeto();
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            PlayerManager.Instance.RecogerObjeto();
        }
        #endregion

        #region CAMBIO DE CONTROL
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
        #endregion
    }

    #endregion
    // Start is called beforse the first frame update
    void Start()
    {
        _playerJump = GetComponentInChildren<JumpComponent>();
        _throwComp = GetComponent<ThrowComponent>();
        _playerManager = GetComponent<PlayerManager>();
        _collisionManager = GetComponent<CollisionManager>();


        // rigid body del player
        _playerRigidBody = GetComponentInChildren<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //------MOVIMIENTO-------
        MovementInput();

        //------INTERACTIONS-----
        InteractInput();

        //------DEBUG------------
        DebugInput();

        //---DIALOGO-----------------------------------------
        //------Input para conversar-----------
        if (Input.GetKeyDown(KeyCode.M))
        {
            _conversar = true;
        }

        //---OPCIÓN DE PAUSA-----------------------
        if (Input.GetKeyDown(KeyCode.Z))
        {
            _UIManager.SetMenu(GameManager.GameStates.PAUSE);
        }

        //  CoolDown();
    }
}
