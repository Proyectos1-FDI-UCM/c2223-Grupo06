using UnityEngine;

public class InputController : MonoBehaviour
{
    #region Referencias
    private JumpComponent _playerJump;
    #endregion


    #region Properties 
    //-------------DIRECCIÓN----------------------------
    //Setea la direccion en la que se mueve el jugador, -1 = izq y 1 = drcha
    public int _direccion;
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


    //Tiempo entre acciones de interaccion
    [SerializeField]
    private float _interactionInterval = 1;
    //Tiempo desde la ultima accion
    private float _timeSinceLastInteraction;
    #endregion

    #region Parameters


    #endregion


    #region Methods
    private void SpamProtection()
    {
        if (_timeSinceLastInteraction > 0)
        {
            _timeSinceLastInteraction -= Time.deltaTime;
        }
    }
    #endregion
    // Start is called beforse the first frame update
    void Start()
    {
        _playerJump = GetComponentInChildren<JumpComponent>();
    }

    private void FixedUpdate()
    {

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
        if (Input.GetKeyDown(KeyCode.E)) //  && _timeSinceLastInteraction <= 0
        {
            _interactuar = true;
            //_timeSinceLastInteraction = _interactionInterval;
        }
        else
        {
            _interactuar = false;
        }
        //------Proteccion contra spam de interaccion------
        //SpamProtection();

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
        {
            PlayerManager.Instance.AddTimmyState(PlayerManager.State);
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            PlayerManager.Instance.SubTimmyState(PlayerManager.State);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            PlayerManager.Instance.SoltarBrazo();
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            PlayerManager.Instance.RecogerBrazo();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            PlayerManager.Instance.SoltarPiernas();
        }
        else if (Input.GetKeyDown(KeyCode.U))
        {
            PlayerManager.Instance.RecogerPiernas();
        }
    }
}
