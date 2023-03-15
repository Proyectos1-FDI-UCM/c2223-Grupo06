using UnityEngine;

public class InputController : MonoBehaviour
{
    #region Referencias
    private JumpComponent _playerJump;
    [SerializeField]
    private GameObject _pataforma;
    private ThrowComponent _throwComp;
    private Rigidbody2D _playerRigidBody;
    
    #endregion

    #region Properties 
    //-------------DIRECCI�N----------------------------
    //Setea la direccion en la que se mueve el jugador, -1 = izq y 1 = drcha
    private int _direccion;
    public int Direccion { get { return _direccion; } }

    //-------------INTERACTUAR------------------------------
    // Indica si el jugador quiere interactuar con una palanca
    [SerializeField]
    private bool _interactuar = false;
    // acceso p�blico a _interactuar
    public bool Interactuar { get { return _interactuar; } }

    //-------------SOLTAR PARTES-----------------------------
    // Indica si el jugador ha dejado una parte en un objeto
    [SerializeField]
    private bool _conectarParte = false;
    // acceso p�blico a _conectarParte
    public bool ConectarParte { get { return _conectarParte; } }

    //-------------RECUPERAR PARTES-----------------------------
    // Indica si el jugador ha dejado una parte en un objeto
    [SerializeField]
    private bool _recuperarParte = false;
    // acceso p�blico a _recuperarParte
    public bool RecuperarParte { get { return _recuperarParte; } }

    //------------CAMBIAR INPUT-----------------------------
    // indica si el jugador quiere cambiar el input a la Pataforma
    // booleano para saber si se ha cambiado el input a la pataforma
    [SerializeField]
    public bool _changeToPataforma;
    // acceso p�blico a _isPataforma
    public bool ChangeToPataforma { get { return _changeToPataforma; } }

    [SerializeField]
    private bool _conversar = false;
    // acceso p�blico a _conversar
    public bool Conversar { get { return _conversar; } }
    #endregion

    #region Parameters

    #endregion

    #region Methods
    #endregion
    // Start is called beforse the first frame update
    void Start()
    {
        _playerJump = GetComponentInChildren<JumpComponent>();
        //_pataformaInputComponent = _pataforma.GetComponent<PataformaInputComponent>();
        // desactiva el input de la pataforma
        //_pataformaInputComponent.enabled = false;
        _throwComp = GetComponent<ThrowComponent>();

        // rigid body del player
        _playerRigidBody = GetComponentInChildren<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        #region uwu
        //---MOVIMIENTO--------------------------------
        //------Input del movimiento horizontal del jugador--------
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

        //------Input del movimiento vertical el jugador----------
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _playerJump.Jump();
        }

        //---INTERACTUABLES----------------------------
        //------Input para interactuar con objetos-----
        if (Input.GetKey(KeyCode.Alpha1) && Input.GetKeyDown(KeyCode.E))
        {
            _interactuar = true;
        }
        else
        {
            _interactuar = false;
        }
        #endregion

        //---PATAFOMA---------------------------------------
        //------Input para interactuar con las piernas-----
        //--------- Hay que dejar pulsado primero el numero y luego la E para interactuar
        if (Input.GetKey(KeyCode.Alpha2) && Input.GetKeyUp(KeyCode.E))
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

        //---LANZAR BRAZO-----------------------------------
        if (Input.GetKeyDown(KeyCode.F))
        {
            _throwComp.LanzarBrazo();
        }

        //---PARTES-----------------------------------------
        //------Input para poner partes a objetos-----------
        if (Input.GetKeyDown(KeyCode.R))
        {
            _conectarParte = true;
        }
        else
        {
            _conectarParte = false;
        }
        //------Input para recuperar partes de objetos------
        if (Input.GetKeyDown(KeyCode.T))
        {
            _recuperarParte = true;
        }
        else
        {
            _recuperarParte = false;
        }



        //---DEBUG-------------------------------------
        //      Para ver si cambia de estados bien
        if (Input.GetKeyDown(KeyCode.V))
        { // SUBIR ESTADO
            PlayerManager.Instance.AddObject();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        { // BAJAR ESTADO
            PlayerManager.Instance.SubObject();
        }

        // para ver si recoge a alubia bien
        if (Input.GetKeyDown(KeyCode.B))
        {
            PlayerManager.Instance.RecogerAlubiat();
        }
        else if (Input.GetKeyDown(KeyCode.N))
        {
            PlayerManager.Instance.SoltarAlubiat();
        }

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

        // para ver si suelta y recoge partes bien
        if (Input.GetKeyDown(KeyCode.P))
        { // SOLTAR BRAZO
            PlayerManager.Instance.SoltarBrazo();
        }
        else if (Input.GetKeyDown(KeyCode.O))
        { // COGER BRAZO
            PlayerManager.Instance.RecogerBrazo();
        }

        if (Input.GetKeyDown(KeyCode.I))
        { // SOLTAR PIERNAS
            PlayerManager.Instance.SoltarPiernas();
        }
        else if (Input.GetKeyDown(KeyCode.U))
        { // COGER PIERNAS
            PlayerManager.Instance.RecogerPiernas();
        }

        //---DIALOGO-----------------------------------------
        //------Input para conversar-----------
        if (Input.GetKeyDown(KeyCode.M))
        {
            _conversar = true;
        }
    }
}
