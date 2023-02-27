using UnityEngine;

public class InputController : MonoBehaviour
{
    #region Referencias
    private JumpComponent _playerJump;
    #endregion
    #region Properties 
    //Setea la direccion en la que se mueve el jugador, -1 = izq y 1 = drcha
    public int _direccion;
    public int Direccion { get { return _direccion; } }

    // Indica si el jugador quiere interactuar con una palanca
    [SerializeField]
    private bool _interactuar = false;
    // acceso p�blico a _interactuar
    public bool Interactuar { get { return _interactuar; } }

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
        if (Input.GetKeyDown(KeyCode.E))
        {
            _interactuar = !_interactuar;
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
    }
}
