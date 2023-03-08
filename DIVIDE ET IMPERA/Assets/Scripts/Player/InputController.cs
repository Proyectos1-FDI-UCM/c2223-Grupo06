using UnityEngine;

public class InputController : MonoBehaviour
{
    #region Referencias
    private JumpComponent _playerJump;
    private PataformaInputComponent _pataformaInputComponent;
    [SerializeField]
    private GameObject _pataforma;
    private ThrowArm _armComp;
    private Rigidbody2D _rb;
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
    public bool _isPataforma;
    // acceso público a _isPataforma
    public bool Pataforma { get { return _isPataforma; } }
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
        _armComp = GetComponent<ThrowArm>();
        _rb = GetComponentInChildren<Rigidbody2D>();
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
        if (Input.GetKey(KeyCode.Alpha1) && Input.GetKeyUp(KeyCode.E))
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
            if (!_isPataforma)
            {
                _isPataforma = true;
                _rb.bodyType = RigidbodyType2D.Kinematic;
                this.enabled = false;
            }
        }

        //---LANZAR BRAZO-----------------------------------
        if (Input.GetKeyDown(KeyCode.F))
        {
            _armComp.LanzarBrazo();
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
    }
}
