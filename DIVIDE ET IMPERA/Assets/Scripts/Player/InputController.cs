using UnityEngine;

public class InputController : MonoBehaviour
{
    #region Referencias
    private JumpComponent _playerJump;
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
    // acceso público a _conectarParte
    public bool RecuperarParte { get { return _recuperarParte; } }
    #endregion

    #region Parameters

    #endregion

    #region Methods
    #endregion
    // Start is called beforse the first frame update
    void Start()
    {
        _playerJump = GetComponentInChildren<JumpComponent>();
    }

    // Update is called once per frame
    void Update()
    {
        //---MOVIMIENTO--------------------------------
        //------Input del movimiento horizontal--------
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

        //------Input del movimiento vertical----------
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _playerJump.Jump();
        }

        //---INTERACTUABLES----------------------------
        //------Input para interactuar con objetos-----
        if (Input.GetKeyDown(KeyCode.E))
        {
            _interactuar = true;
        }
        else
        {
            _interactuar = false;
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
            PlayerManager.Instance.AddTimmyState(PlayerManager.State);
        }
        else if (Input.GetKeyDown(KeyCode.C))
        { // BAJAR ESTADO
            PlayerManager.Instance.SubTimmyState(PlayerManager.State);
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
