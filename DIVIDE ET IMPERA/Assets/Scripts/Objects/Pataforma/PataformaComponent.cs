//using UnityEditor.Tilemaps;
using UnityEngine;

public class PataformaComponent : MonoBehaviour
{
    #region TUTORIAL
    /// <summary>
    /// Como poner la Patwoforma en escena es un coñazo asqueroso y es entre muy y demasiado tiquisimiquis
    /// voy a haceros un tutorial dpm para que sepais como cuidar bien a mi pequeña patwoforma:
    /// 
    /// Cuando se pone en escena el objeto, tiene referenciado a Timmy, pero no al Timmy de la escena,
    /// por lo que teneis que arrastrar al Timmy de la escena a la referencia en el componente
    /// 
    /// 
    /// Ya no es necesario lo anterior, cambiado a que este hecho por codigo
    /// 
    /// 
    /// 
    /// y con esto se acabó el tutorial maravilloso y estupendo, recordad darle a like y compartir
    /// y acivar la campanita ;)
    /// 
    /// P.D. Controles antiguos
    /// R ---> conectar piernas
    /// T ---> recuperar piernas
    /// 2+E -> cambiar input de uno a otro
    /// WASD -> moverse 
    /// 
    /// Controles nuevos
    /// E --> conectar piernas
    /// R --> recuperar piernas
    /// 3+T --> cambiar el control
    /// WASD --> moverse 
    /// 
    /// </summary>
    #endregion

    #region Referencias
    private InputController _inputController;
    private SpriteRenderer _mySpriteRenderer;
    private GameObject _player;
    private Rigidbody2D _playerRigidbody;

    private PataformaMovementComponent _pataformaMovementComponent;
    private Rigidbody2D _pataformaRigidbody;
    [SerializeField]
    private PataformaMovementComponent pataformaMovementComponent;

    [SerializeField] GameObject _patas;
    [SerializeField] Animator _animator;

    #endregion

    #region Properties
    // piernas conectadas a la pataforma 
    [SerializeField]
    private bool _piernasConectadas = false;
    public bool PiernasConectadas { get { return _piernasConectadas; } }

    // esta en el área de una pataforma
    public bool _validPataformaHitbox;

    private bool _isPataforma;
    public bool Pataforma { get { return _isPataforma; } }

    //-------------DIRECCIÓN----------------------------
    //Setea la direccion en la que se mueve el jugador, -1 = izq y 1 = drcha
    private int _pDireccion;
    public int PataformaDireccion { get { return _pDireccion; } }


    [SerializeField]
    private bool _activarPataforma;
    #endregion

    #region Methods
    private void ConectaPiernas()
    {
        //-------CONECTAR PIERNAS-------------------
        // se pulsa R y se esta cerca de la pataforma
        if (_inputController.ConectarParte && _validPataformaHitbox
            && (PlayerManager.State == PlayerManager.TimmyStates.S0
            || PlayerManager.State == PlayerManager.TimmyStates.S1
            || PlayerManager.State == PlayerManager.TimmyStates.S2))
        {
            // conecta el brazo
            _piernasConectadas = true;

            // SI TIENE BRAZOS
            if (PlayerManager.State == PlayerManager.TimmyStates.S0)
            {
                _player.GetComponent<PlayerManager>().RequestTimmyState(PlayerManager.TimmyStates.S3);
            }
            // SI NO TIENE UN BRAZO
            else if (PlayerManager.State == PlayerManager.TimmyStates.S1)
            {
                _player.GetComponent<PlayerManager>().RequestTimmyState(PlayerManager.TimmyStates.S4);
            }
            // SI NO TIENE BRAZOS
            else if (PlayerManager.State == PlayerManager.TimmyStates.S2)
            {
                _player.GetComponent<PlayerManager>().RequestTimmyState(PlayerManager.TimmyStates.S5);
            }

            Debug.Log(PlayerManager.State);
            // cambia el color (deberia ser sprite)
            _mySpriteRenderer.color = Color.cyan;
        }
        // se pulsa T, está cerca de la pataforma, está en los estados correctos y hay patas conectadas
        else if (_inputController.RecuperarParte && _validPataformaHitbox
            && (PlayerManager.State == PlayerManager.TimmyStates.S3
            || PlayerManager.State == PlayerManager.TimmyStates.S4
            || PlayerManager.State == PlayerManager.TimmyStates.S5)
            && _piernasConectadas)
        {
            // desconecta el brazo
            _piernasConectadas = false;

            // si tiene los brazos cambia de estado
            if (PlayerManager.State == PlayerManager.TimmyStates.S3)
            {
                _player.GetComponent<PlayerManager>().RequestTimmyState(PlayerManager.TimmyStates.S0);
            }
            // si no tiene un brazo cambia de estado
            else if (PlayerManager.State == PlayerManager.TimmyStates.S4)
            {
                _player.GetComponent<PlayerManager>().RequestTimmyState(PlayerManager.TimmyStates.S1);
            }
            // si no tiene brazos cambia de estado
            else if (PlayerManager.State == PlayerManager.TimmyStates.S5)
            {
                _player.GetComponent<PlayerManager>().RequestTimmyState(PlayerManager.TimmyStates.S2);
            }

            // cambia de color (deberia ser sprite)
            _mySpriteRenderer.color = Color.white;
        }
    }
    private void Visual()
    {
        if (_piernasConectadas)
        {
            _patas.SetActive(true);
        }
        else
        {
            _patas.SetActive(false);
        }

    }
    private void ChangeInput()
    {
        //---INPUT CHANGE---------------------------------------
        //------de pataforma a player---------------------------
        // 2 + E para cambiar de vuelta
        if (Input.GetKey(KeyCode.Alpha3) && Input.GetKeyUp(KeyCode.T))
        {
            // si el input del player esta desactivado y el de la pataforma específica
            // esta activado procede a invertirlos, asi se asegura de que el cambio de
            // input está individualizado
            if (!_inputController.enabled && _activarPataforma)
            {
                //_player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                // cambia el rigidbody de la pataforma a kinematic para que no se mueva cuando
                // timmy se suba encima
                _pataformaRigidbody.bodyType = RigidbodyType2D.Kinematic;
                // cambia el rigidbody del player para que siga las físicas
                _playerRigidbody.bodyType = RigidbodyType2D.Dynamic;
                // cambia el constraint al eje Z
                _playerRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;

                // desactiva la plataforma
                _activarPataforma = false;

                //_myRigidbody.bodyType = RigidbodyType2D.Kinematic;

                // activa el input del player
                _inputController.enabled = true;

                _mySpriteRenderer.color = Color.cyan;

                // cambio de control de parte (es para el HUD)
                PlayerManager.Instance.SwitchPartControl(PlayerManager.Partes.CABEZA);

                //Cambia que la camara siga a Timmy
                CameraMovement.Instance.ChangeWhoToFollow(_player);
            }

        }
        //------de player a pataforma----------------------------
        // 2 + E para reactivar al player
        if (_inputController.ChangeToPataforma && _piernasConectadas)
        {
            // cambia el rb de la pataforma a dynamic, para que se choque con los obstaculos
            _pataformaRigidbody.bodyType = RigidbodyType2D.Dynamic;
            // cambia el rb del player para que siga a la pataforma (?)
            _playerRigidbody.bodyType = RigidbodyType2D.Kinematic;
            // cambia las contraints del player para que no resbale
            _playerRigidbody.constraints = RigidbodyConstraints2D.FreezePositionX;

            // activa la plataforma
            _activarPataforma = true;

            // desactiva el input del player
            _inputController._changeToPataforma = false;
            _mySpriteRenderer.color = Color.blue;

            // cambio de control de parte (es para el HUD)
            PlayerManager.Instance.SwitchPartControl(PlayerManager.Partes.PIERNAS);
            Debug.Log("Piernas controlan");

            //Cambia el movimiento de la camara para que siga a las piernas
            CameraMovement.Instance.ChangeWhoToFollow(gameObject);
        }
    }
    private void PataformaInput()
    {
        //---MOVIMIENTO--------------------------------
        //------Input del movimiento horizontal de la pataforma--------
        if (Input.GetKey(KeyCode.D))
        {
            _pDireccion = 1;

            // inicia el animator
            _animator.SetBool("move", true);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            _pDireccion = -1;

            // inicia el animator
            _animator.SetBool("move", true);
        }
        else
        {
            _pDireccion = 0;

            // inicia el animator
            _animator.SetBool("move", false);
        }
    }
    #endregion

    void Start()
    {
        _player = PlayerAccess.Instance.gameObject;
        _inputController = _player.GetComponent<InputController>();
        _mySpriteRenderer = GetComponent<SpriteRenderer>();
        _pataformaMovementComponent = GetComponent<PataformaMovementComponent>();
        _pataformaRigidbody = GetComponent<Rigidbody2D>();
        _playerRigidbody = _player.GetComponent<Rigidbody2D>();

    }

    void Update()
    {
        ConectaPiernas();

        Visual();

        ChangeInput();

        if (_activarPataforma)
        {
            PataformaInput();
        }
    }
}
